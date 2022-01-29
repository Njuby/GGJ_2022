using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class EnemyAnimationPlayer : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;

    public void OnEnable()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void DoAttack(string attackName)
    {
        animator.Play(attackName);
    }

    public void Update()
    {
        UpdateWalk();
    }

    public void UpdateWalk()
    {
        bool walking = agent.velocity.magnitude > 0.1f;

        animator.SetBool("Walking", walking);
        if (walking)
        {
            float value = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Speed", value);
        }
    }
}
