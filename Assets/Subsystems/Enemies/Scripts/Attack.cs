using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Attack")]
public class Attack : ScriptableObject
{
    [Header("Damage amount")]
    public int directDamage;
    public int damage;

    public void PlayAttack(GameObject source, GameObject reciever = null)
    {
        Animator animator = source.GetComponent<Animator>();
        EnemyAnimationPlayer animationPlayer = source.GetComponent<EnemyAnimationPlayer>();
        if (animationPlayer)
            animationPlayer.DoAttack("Attack");

        if(reciever)
            DoDirectDamage(reciever);
    }

    public void DoDirectDamage(GameObject reciever)
    {
        reciever.Hit(directDamage);
    }
}
