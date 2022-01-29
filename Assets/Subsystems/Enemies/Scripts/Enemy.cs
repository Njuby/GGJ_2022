using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class Enemy : Hittable
{
    NavMeshAgent agent;
    public GameObject target;

    [Header("Detection")]
    public float detectionRange;
    public float closeToPlayerRange;

    [Header("Attack")]
    public float attackTimer;
    public float attackCooldown;
    [Required] public Attack attack;

    [Required] public GameObjectEvent destroyEvent;
    public LayerMask mask;

    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    [Button("Kill")]
    public override void Die()
    {
        destroyEvent.Raise(gameObject);
    }

    public void Target(GameObject target)
    {
        this.target = target;
    }

    public void FixedUpdate()
    {
        if (target != null)
        {
            MoveToTarget();
        }
        attackTimer += Time.deltaTime;
    }

    public void MoveToTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > detectionRange) { return; }

        if (RaycastTools.RayCastFromPos(transform.position, direction, mask, out RaycastHit hit))
        {
            if (hit.transform == target.transform)
            {
                agent.SetDestination(target.transform.position - direction.normalized * closeToPlayerRange);
            }
        }

        if (distance <= closeToPlayerRange)
        {
            if (attackTimer >= attackCooldown)
            {
                attackTimer = 0;
                Attack();
            }
        }
    }

    public void Attack()
    {
        attack.PlayAttack(gameObject, target);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeToPlayerRange);

        if (!target) return;
        Vector3 direction = target.transform.position - transform.position;
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > detectionRange) { return; }

        if (RaycastTools.RayCastFromPos(transform.position, direction, mask, out RaycastHit hit))
        {
            if (hit.transform == target.transform)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(target.transform.position - direction.normalized * closeToPlayerRange, 1);
            }
        }
    }
}
