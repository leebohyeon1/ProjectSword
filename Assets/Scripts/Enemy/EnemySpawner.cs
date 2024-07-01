using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] Pattern; // 利 橇府普
    public float spawnRate = 2.0f; // 利 积己 林扁
    private float nextSpawnTime = 0.0f;

    [SerializeField]
    private Transform spawnPosition;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;
        }
    }


    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, Pattern.Length);
        Instantiate(Pattern[randomIndex], spawnPosition.localPosition, Quaternion.identity);
    }
}
