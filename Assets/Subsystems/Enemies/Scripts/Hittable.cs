using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour, IHealth, IPoolObject
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;

    public int MaxHealth => maxHealth;
    public int Health => health;

    public int PoolKey { get; set; }
    public ObjectInstance ObjInstance { get; set; }

    public virtual void Die()
    {
        PoolManager.Instance.ReturnToPool(gameObject);
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void OnGetObject(ObjectInstance objectInstance, int poolKey)
    {
        this.ObjInstance = objectInstance;
        this.PoolKey = poolKey;
        gameObject.SetActive(true);
    }
    public void OnReturnObject()
    {
        gameObject.SetActive(false);
    }
}

public static class HitExtensions
{
    public static void Hit(this GameObject obj, int amount)
    {
        Hittable hit = obj.GetComponent<Hittable>();
        if (hit) hit.TakeDamage(amount);
    }
}