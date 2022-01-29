using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObjectEvent destroyedObj;

    public GameObject[] enemy;

    public float radius;
    public int spawnLimit;

    public float time;
    public float spawnTime;

    public List<GameObject> spawnedEnemies = new List<GameObject>();

    public void Update()
    {
        if (spawnedEnemies.Count >= spawnLimit) return;

        time += Time.deltaTime;
        if (time > spawnTime)
        {
            SpawnEnemy();
            time = 0;
        }
    }

    public void OnEnable()
    {
        destroyedObj.Register(DespawnEnemy);
    }

    public void SpawnEnemy()
    {
        int enemyType = Random.Range(0, enemy.Length);
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        float distance = Random.Range(0, radius);

        GameObject obj = PoolManager.Instance.GetFromPool(enemy[enemyType], transform.position, rotation, null);
        obj.transform.position += obj.transform.forward * distance;

        spawnedEnemies.Add(obj);
    }

    public void DespawnEnemy(GameObject enemy)
    {
        if (spawnedEnemies.Contains(enemy))
        {
            spawnedEnemies.Remove(enemy);
            PoolManager.Instance.ReturnToPool(enemy);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

