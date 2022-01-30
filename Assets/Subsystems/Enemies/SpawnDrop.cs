using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class SpawnDrop : MonoBehaviour
{
    [SerializeField] private GameObjectEvent enemyDeath;
    [SerializeField] private GameObject prefab;

    public void Start()
    {
        enemyDeath.Register(DropMutantEssence);
    }

    public void DropMutantEssence(GameObject enemy)
    {
        PoolManager.Instance.GetFromPool(prefab, enemy.transform.position, Quaternion.identity);
    }
}
