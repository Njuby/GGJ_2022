using UnityEngine;
using DG.Tweening;

public class AmmoMove : MonoBehaviour, IPoolObject
{
    [SerializeField] private Rigidbody rb;
    public int PoolKey { get; set; }
    public ObjectInstance ObjInstance { get; set; }
    private int damage;
    private Vector3 targetPos;
    private Vector3 direction;
    private Vector3 velocity;

    public void Setup(Transform target, Transform camTrans, float speed, int damage)
    {
        Vector3 offset = Vector3.up;
        this.damage = damage;
        targetPos = target != null ? target.position : transform.position + (camTrans.transform.forward * 30) + offset;
        direction = (targetPos - transform.position).normalized;
        transform.LookAt(targetPos);
        //rb.DOMove(targetPos, direction.magnitude / speed);
        velocity = rb.transform.forward * (direction.magnitude * speed);
    }

    public void OnGetObject(ObjectInstance objectInstance, int poolKey)
    {
        ObjInstance = objectInstance;
        PoolKey = poolKey;
    }

    private void Update()
    {
        rb.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        PoolManager.Instance.ReturnToPool(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            collision.gameObject.Hit(damage);
        }

        PoolManager.Instance.ReturnToPool(gameObject);
    }

    public void OnReturnObject()
    {
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}