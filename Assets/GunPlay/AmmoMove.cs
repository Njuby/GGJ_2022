using UnityEngine;

public class AmmoMove : MonoBehaviour, IPoolObject
{
    [SerializeField] private Rigidbody rb;
    public int PoolKey { get; set; }
    public ObjectInstance ObjInstance { get; set; }

    private Vector3 targetPos;
    private Vector3 direction;

    public void Setup(Vector3 targetPos, float speed)
    {
        this.targetPos = targetPos;
        direction = (transform.position - targetPos).normalized * speed;
    }

    public void OnGetObject(ObjectInstance objectInstance, int poolKey)
    {
        ObjInstance = objectInstance;
        PoolKey = poolKey;
    }

    private void Update()
    {
        rb.velocity = direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        PoolManager.Instance.ReturnToPool(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PoolManager.Instance.ReturnToPool(gameObject);
    }

    public void OnReturnObject()
    {
        rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}