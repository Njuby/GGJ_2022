using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolObject
{
    public float velocity;

    public float gravity;
    public float autoLockToDistance;

    public Vector3 GoTowards;
    public float TowardsStrength;

    public float twist = 1;
    public LayerMask mask;

    [Header("Damaging factor")]
    public int damage;

    public float DespawnTime;
    public float DieOutTime;

    public float time;

    public int PoolKey { get; set; }
    public ObjectInstance ObjInstance { get; set; }

    public bool stopped;

    public void OnGetObject(ObjectInstance objectInstance, int poolKey)
    {
        ObjInstance = objectInstance;
        PoolKey = poolKey;
        stopped = false;

        transform.localScale = Vector3.one;
        GetComponent<TrailRenderer>()?.Clear();
        time = 0;
    }

    public void OnReturnObject()
    {
        PoolManager.Instance.SetParent(gameObject);
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (stopped) return;

        time += Time.deltaTime;
        if (time > DieOutTime)
        {
            PoolManager.Instance.ReturnToPool(gameObject);
            return;
        }

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + twist * Time.deltaTime, 0);

        if (GoTowards != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GoTowards - transform.position, Vector3.up), TowardsStrength);

        transform.position += transform.forward * velocity * Time.deltaTime;
        transform.position += Vector3.down * gravity * Time.deltaTime;

        var collider = Physics.OverlapSphere(transform.position, autoLockToDistance, mask);
        collider.OrderBy((x) => Vector3.Distance(x.transform.position, transform.position));

        if (collider.Length != 0)
            GoTowards = collider[0].transform.position + Vector3.up * 0.5f;
        else GoTowards = Vector3.zero;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (stopped) return;

        if (collision.transform.CompareTag("Player")) return;
        collision.gameObject.Hit(damage);
        transform.SetParent(collision.transform);
        if (gameObject.activeInHierarchy) StartCoroutine(WaitForDespawn());
        else PoolManager.Instance.ReturnToPool(gameObject);
        stopped = true;
    }

    public IEnumerator WaitForDespawn()
    {
        yield return new WaitForSeconds(DespawnTime);
        if (!gameObject.activeInHierarchy)
        {
            PoolManager.Instance.ReturnToPool(gameObject);
            yield return null;
        }

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(0, 0.15f));
        sequence.AppendCallback(() => PoolManager.Instance.ReturnToPool(gameObject));
    }
}
