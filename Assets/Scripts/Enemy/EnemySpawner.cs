using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] Pattern; // �� ������
    public float spawnRate = 2.0f; // �� ���� �ֱ�
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
