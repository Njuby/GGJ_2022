using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParticleToPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offSet = new Vector3(0f, 3f, 0f);

    public ParticleSystem[] flame;
    public GameObject trail;

    public float range;
    public float speed = 5f;
    private bool tweenActivated;
    private bool isActive = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tweenActivated = false;
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < range && !isActive)
        {
            for (int i = 0; i < flame.Length; i++)
            {
                flame[i].Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }

            isActive = true;
        }

        if (isActive)
        {
            trail.GetComponent<ParticleSystem>().Play();

            trail.transform.position = Vector3.Lerp(trail.transform.position, player.transform.position + offSet, speed * Time.deltaTime);

        }
    }

    //Cope this in player script

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Matancy"))
    //    {
    //        //Increase matancy of player

    //        Destroy(other.transform.parent);
    //    }
    //}
}