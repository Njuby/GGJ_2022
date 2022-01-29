using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Attack")]
public class Attack : ScriptableObject
{
    [Header("Damage amount")]
    public int directDamage;
    public string animation;

    public void PlayAttack(GameObject source, GameObject reciever = null)
    {
        Animator animator = source.GetComponent<Animator>();
        if (animator)
            animator.Play(animation);

        if(reciever)
            DoDirectDamage(reciever);
    }

    public void DoDirectDamage(GameObject reciever)
    {
        Hittable hit = reciever.GetComponent<Hittable>();
        if(hit) hit.TakeDamage(directDamage);
    }
}
