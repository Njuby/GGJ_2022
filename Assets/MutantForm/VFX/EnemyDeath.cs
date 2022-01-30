using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class EnemyDeath : MonoBehaviour, IPoolObject
{
    [SerializeField] private VisualEffect visualEffect;
    public int PoolKey { get; set; }
    public ObjectInstance ObjInstance { get; set; }
    private Sequence seq;

    public void OnGetObject(ObjectInstance objectInstance, int poolKey)
    {
        ObjInstance = objectInstance;
        PoolKey = poolKey;
        gameObject.SetActive(true);
        visualEffect.enabled = true;
        if (seq != null && (seq.active || seq.IsPlaying()))
        {
            seq.Kill();
        }
        seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.AppendCallback(() => PoolManager.Instance.ReturnToPool(gameObject));
    }

    public void OnReturnObject()
    {
        visualEffect.enabled = false;
        gameObject.SetActive(false);
    }
}