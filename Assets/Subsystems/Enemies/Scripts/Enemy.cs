using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class Enemy : Hittable
{
    NavMeshAgent agent;
    public GameObject target;
    public Vector3 wanderTarget;

    [Header("Detection")]
    public float detectionRange;
    public float closeToPlayerRange;

    [Header("Attack")]
    public float attackTimer;
    public float attackCooldown;
    [Required] public Attack attack;

    [Header("Idle")]
    public float idleTime;
    public float idleCooldown;

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
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).GetComponent<IPoolObject>() != null)
            {
                PoolManager.Instance.ReturnToPool(transform.GetChild(i).gameObject);
            }
        }

        destroyEvent.Raise(gameObject);
    }

    public void Target(GameObject target)
    {
        this.target = target;
    }

    public void FixedUpdate()
    {
        if (!agent.isOnNavMesh) return;

        if (!MoveToTarget())
        {
            idleTime += Time.deltaTime;
            Wander();
        }
        attackTimer += Time.deltaTime;
    }

    public bool MoveToTarget()
    {
        if (target == null) return false;

        Vector3 direction = (target.transform.position + Vector3.up) - (transform.position + Vector3.up);
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > detectionRange) { return false; }

        if (distance <= closeToPlayerRange)
        {
            if (attackTimer >= attackCooldown)
            {
                attackTimer = 0;
                Attack();
            }
        }

        //if (RaycastTools.RayCastFromPos(transform.position + Vector3.up, direction, mask, out RaycastHit hit))
        //{
                agent.SetDestination(target.transform.position - direction.normalized * closeToPlayerRange);
                return true;
      //  }
    }

    public void Wander()
    {
        if (idleTime < idleCooldown)
            return;

        if (wanderTarget == Vector3.zero)
        {
            wanderTarget = transform.position + new Vector3(Random.Range(-25f, 25f), 0, Random.Range(-25f, 25f));
            if (RaycastTools.RayCastFromPos(wanderTarget + Vector3.up * 20, Vector3.down, mask, out RaycastHit hit))
            {
                wanderTarget = hit.point;
            }
        }


        agent.SetDestination(wanderTarget);

        if (Vector3.Distance(wanderTarget, transform.position) < 0.1f)
        {
            idleTime = 0;
            wanderTarget = Vector3.zero;
        }
    }

    public void Attack()
    {
        attack.PlayAttack(gameObject);
    }

    public void DoDamage()
    {
        if (target) target.Hit(attack.damage);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeToPlayerRange);

        if (!target) return;
        Vector3 direction = (target.transform.position + Vector3.up) - (transform.position + Vector3.up);
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > detectionRange) { return; }

        if (RaycastTools.RayCastFromPos(transform.position + Vector3.up, direction, mask, out RaycastHit hit))
        {
            if (hit.transform == target.transform)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(target.transform.position - direction.normalized * closeToPlayerRange, 1);
            }
        }
    }
}
