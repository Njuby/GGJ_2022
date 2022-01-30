using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class StompAbility : Ability
{
    public CharacterMovement player;
    public Animator anim;
    public IntEvent mutantStrengthEvent;
    public BoolEvent mutantChangeEvent;

    public IntEvent useAbilityEvent;

    public GameObject preview;
    public GameObject effect;
    public GameObject spikeCluster;

    [Header("Per mutant point")]
    public int clusterCount;
    public int spawnPerFrame;

    public float damageRadius;
    public int damage;

    [Header("Masks")]
    public LayerMask enemyMask;
    public LayerMask groundMask;

    [Header("Non spawing zone around the player")]
    public float deadzone;

    public int mutantcy;

    private bool active;

    public void OnEnable()
    {
        preview.SetActive(false);
        effect.SetActive(false);

        mutantStrengthEvent.Register((x) => mutantcy = x / 5);
        mutantChangeEvent.Register((x) => active = x);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void DoAbility()
    {
        if (!active) return;

        if (!player.IsGrounded()) return;

        useAbilityEvent.Raise(5);

        base.DoAbility();

        player.Jump(false);

        if (RaycastTools.RayCastFromPos(transform.position + Vector3.up * 5, Vector3.down, groundMask, out RaycastHit hit))
        {
            preview.transform.position = hit.point;
            effect.transform.position = hit.point;
        }
        preview.transform.localScale = Vector3.one * damageRadius * mutantcy;
        effect.transform.localScale = Vector3.one * damageRadius * mutantcy;
        preview.SetActive(true);
    }

    public void Stomp()
    {
        
        anim.SetBool("isAttacking", false);
        List<float> distanceList = new List<float>();

        var colliders = Physics.OverlapSphere(transform.position, damageRadius * mutantcy, enemyMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            distanceList.Add(Vector3.Distance(colliders[i].transform.position, transform.position));
        }
        preview.SetActive(false);

        StartCoroutine(DoEffect(distanceList, colliders));
    }

    public IEnumerator WaitForHitGround()
    {
        preview.transform.localScale = Vector3.one * damageRadius * mutantcy;
        effect.transform.localScale = Vector3.one * damageRadius * mutantcy;

        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => player.IsGrounded());

        List<float> distanceList = new List<float>();

        var colliders = Physics.OverlapSphere(transform.position, damageRadius * mutantcy, enemyMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            distanceList.Add(Vector3.Distance(colliders[i].transform.position, transform.position));
        }
        preview.SetActive(false);

        StartCoroutine(DoEffect(distanceList, colliders));
    }

    public IEnumerator DoEffect(List<float> distances, Collider[] colliders)
    {
        effect.GetComponentInChildren<MeshRenderer>().material.SetVector("_Pointer", transform.position);
        effect.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Scale", damageRadius * mutantcy);

        effect.SetActive(true);

        for (int j = 0; j < distances.Count; j++)
        {
            if (distances[j] > 0 && distances[j] < deadzone)
            {
                colliders[j].gameObject.Hit(damage * mutantcy);
            }
        }

        float clusterStep = (damageRadius * mutantcy - deadzone) / (clusterCount * mutantcy);
        for (int i = 0; i < clusterCount * mutantcy; i++)
        {
            effect.GetComponentInChildren<MeshRenderer>().material.SetFloat("_Timer", (i / ((float)clusterCount * mutantcy)));

            Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            float distance = Random.Range(clusterStep * i - clusterStep + deadzone, clusterStep * i + deadzone);
            float size = Random.Range(0.8f, 1.5f);

            for (int j = 0; j < distances.Count; j++)
            {
                if (distances[j] > clusterStep * i - clusterStep && distances[j] < clusterStep * i)
                {
                    colliders[j].gameObject.Hit(damage * mutantcy);
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

            if (i % (spawnPerFrame * mutantcy) == 0)
                yield return null;
        }

        effect.SetActive(false);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius * mutantcy);
    }
}