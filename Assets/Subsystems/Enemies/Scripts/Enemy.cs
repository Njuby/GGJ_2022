using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class Enemy : Hittable
{
    NavMeshAgent agent;
    public GameObject target;

    public float detectionRange;
    public float closeToPlayerRange;

    public GameObjectEvent destroyEvent;

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

        if(distance <= closeToPlayerRange)
        {
            DamagePlayer();
        }
    }

    public void DamagePlayer()
    {
        target.GetComponent<Hittable>().TakeDamage(5);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeToPlayerRange);

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
