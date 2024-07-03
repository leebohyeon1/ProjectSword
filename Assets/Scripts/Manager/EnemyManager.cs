using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] Pattern; // 적 프리팹
    private GameObject curPattern;
    public float spawnRate = 2.0f; // 적 생성 주기
    private float nextSpawnTime = 2.0f;

    [SerializeField]
    private Transform spawnPosition;

    private void Start()
    {
        StartSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            Debug.Log("스폰");
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate;            
        }
    }


    void SpawnEnemy()
    {
        int firstRate = Random.Range(0, 100);
        int Secondindex;
        if (firstRate < 50)
        {
            int index = Random.Range(0, curPattern.GetComponent<EnemyScrollController>().next50Pattern.Length);
            Secondindex = curPattern.GetComponent<EnemyScrollController>().next50Pattern[index];
            firstRate = 50;
        }
        else if(firstRate < 85)
        {
            int index = Random.Range(0, curPattern.GetComponent<EnemyScrollController>().next35Pattern.Length);
            Secondindex = curPattern.GetComponent<EnemyScrollController>().next35Pattern[index];
            firstRate = 35;
        }
        else
        {
            int index = Random.Range(0, curPattern.GetComponent<EnemyScrollController>().next15Pattern.Length);
            Secondindex = curPattern.GetComponent<EnemyScrollController>().next15Pattern[index];
            firstRate = 15;
        }

        curPattern = Pattern[Secondindex];
        Debug.Log("확률: " + firstRate + "% , 현재 패턴: " + curPattern.name);
        Instantiate(Pattern[Secondindex], spawnPosition.localPosition, Quaternion.identity);
    }

    void StartSpawn()
    {
        int randomIndex = Random.Range(0, 3);
        curPattern = Pattern[randomIndex];
        Instantiate(Pattern[randomIndex], spawnPosition.localPosition, Quaternion.identity);
    }
}
