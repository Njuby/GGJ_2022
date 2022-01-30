using UnityEngine;
using UnityAtoms.BaseAtoms;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using System.Collections;
using DG.Tweening;
using UnityEditor;

public class Enemy : Hittable
{
    private NavMeshAgent agent;
    public GameObject target;
    public Vector3 wanderTarget;
    public Animator animator;
    public AudioCue attackCue;
    public AudioCue idleCue;
    public AudioCue aggroCue;
    public GameObject deathPrefab;

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

    public void Awake()
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(0.4f);
        seq.AppendCallback(() => agent.enabled = true);

        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }

    //public IEnumerator EnableAgent()
    //{
    //    while (true)
    //    {
    //        yield return null;
    //        agent.enabled = true;
    //        yield break;
    //    }
    //}

    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    [Button("Kill")]
    public override void Die()
    {
        GameObject death = PoolManager.Instance.GetFromPool(deathPrefab, transform.position, Quaternion.identity);
        death.transform.localScale = Vector3.one * 5;
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

    private bool prevInRange;

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
        if (!prevInRange)
        {
            attackCue.PlayAudioCue(transform);
        }

        agent.SetDestination(target.transform.position - direction.normalized * closeToPlayerRange);
        prevInRange = distance < detectionRange;
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
                idleCue.PlayAudioCue(transform);
            }
        }

        agent.SetDestination(wanderTarget);

        if (Vector3.Distance(wanderTarget, transform.position) < 1f)
        {
            idleTime = 0;
            wanderTarget = Vector3.zero;
        }
    }

    public void Attack()
    {
        attack.PlayAttack(gameObject);
        attackCue.PlayAudioCue(transform);
    }

    public void DoDamage()
    {
        if (target) target.Hit(attack.damage);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        int index = Random.Range(0, 4);
        animator.Play($"Hit_{index}");
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