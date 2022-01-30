using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParticleToPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offSet = new Vector3(0f,3f,0f);

    public ParticleSystem[] flame;
    public GameObject trail;

    public float range;
    public float speed = 5f;

    bool isActive = false;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            trail.transform.DOMove(player.transform.position+offSet, speed);
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
