using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompAbility : Ability
{
    public CharacterMovement player;
    public Animator playerAnim;
    public GameObject preview;
    public GameObject effect;
    public GameObject spikeCluster;

    public int clusterCount;
    public int spawnPerFrame;

    public float damageRadius;
    public int damage;

    public LayerMask enemyMask;
    public LayerMask groundMask;

    public bool jumping;

    public void OnEnable()
    {

        preview.transform.localScale = Vector3.one * damageRadius;
        effect.transform.localScale = Vector3.one * damageRadius;
        preview.SetActive(false);
        effect.SetActive(false);
    }

    public override void Update()
    {
        base.Update();
        if (jumping)
        {
            if (RaycastTools.RayCastFromPos(transform.position + Vector3.up * 5, Vector3.down, groundMask, out RaycastHit hit))
            {
                preview.transform.position = hit.point;
                effect.transform.position = hit.point;
            }
        }
    }

    public override void DoAbility()
    {
        base.DoAbility();

        player.Jump(false);

        preview.SetActive(true);

        //StartCoroutine(WaitForHitGround());

        jumping = true;
    }

    public IEnumerator WaitForHitGround()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => player.IsGrounded());

        //StartCoroutine(DoEffect(distanceList, colliders));
    }

    public void Stomp()
    {
        Debug.Log("I AM CALLED");
        playerAnim.SetBool("isAttacking", false);
        List<float> distanceList = new List<float>();

        var colliders = Physics.OverlapSphere(transform.position, damageRadius, enemyMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            distanceList.Add(Vector3.Distance(colliders[i].transform.position, transform.position));
        }
        preview.SetActive(false);

        StartCoroutine(DoEffect(distanceList, colliders));

        jumping = false;
    }

    public IEnumerator DoEffect(List<float> distances, Collider[] colliders)
    {
        effect.GetComponentInChildren<MeshRenderer>().material.SetVector("_Pointer", transform.position);
        effect.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Scale", damageRadius);

        effect.SetActive(true);

        float clusterStep = damageRadius / clusterCount;
        for (int i = 0; i < clusterCount; i++)
        {
            effect.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Timer", (i / (float)clusterCount));

            Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            float distance = Random.Range(clusterStep * i - clusterStep, clusterStep * i);
            float size = Random.Range(0.8f, 1.5f);

            for (int j = 0; j < distances.Count; j++)
            {
                if (distances[j] > clusterStep * i - clusterStep && distances[j] < clusterStep * i)
                {
                    colliders[j].gameObject.Hit(damage);
                }
            }

            GameObject spike = PoolManager.Instance.GetFromPool(spikeCluster, transform.position, rotation);
            spike.transform.position += spike.transform.forward * distance;
            spike.transform.localScale = Vector3.zero;

            if (RaycastTools.RayCastFromPos(spike.transform.position + Vector3.up * 5, Vector3.down, groundMask, out RaycastHit hit))
            {
                spike.transform.position = hit.point;
            }

            Sequence seq = DOTween.Sequence();
            seq.Append(spike.transform.DOScale(new Vector3(size, size, size), 0.1f));
            seq.Append(spike.transform.DOPunchScale(Vector3.one, .2f, 5, 0.2f));
            seq.AppendInterval(1);
            seq.Append(spike.transform.DOScale(new Vector3(size, size, size) * 1.2f, 0.05f));
            seq.Append(spike.transform.DOScale(Vector3.zero, 0.1f));
            seq.AppendCallback(() => PoolManager.Instance.ReturnToPool(spike));

            if (i % spawnPerFrame == 0)
                yield return null;
        }

        effect.SetActive(false);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}