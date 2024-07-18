using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance {  get; private set; }
    //==================================================================================

    [Header("적 스폰")]
    public GameObject[] Patterns; // 적 프리팹
    private GameObject curPattern;
    public float spawnRate = 2.0f; // 적 생성 주기
    private float spawnTimer = 0.0f;
    public float plusAcceleration = 0;

    [Header("선택지")]
    public GameObject enchantOption;
    public int spawnCount = 5;
    [SerializeField]
    private int curCount = 0;
    public float optionSpeed = 5f;

    [Space(20f)]
    [SerializeField]
    private Transform spawnPosition;

    [Header("보스")]
    public int bossCount = 0;
    public int[] bossSpawnCount;
    public GameObject[] bossPrefab;
    private int curEvent = 0;
    private bool isboss;
    //==================================================================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        spawnTimer = 0f;
    }

    private void Start()
    {
        StartSpawn();
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnRate)
        {
            SpawnEnemy();
            spawnTimer = 0f;            
        }

        if(curCount >= spawnCount)
        {
            SpawnEnchant();
            curCount = 0;
        }

        SpawnBoss();
    }
    //==================================================================================

    private void SpawnEnemy()
    {
        int randomRate = Random.Range(0, 100);
        int nextPatternIndex = GetNextPatternIndex(ref randomRate);

        if (nextPatternIndex >= 0 && nextPatternIndex < Patterns.Length)
        {
            curPattern = Patterns[nextPatternIndex];
            //Debug.Log($"확률: {randomRate}% , 현재 패턴: {curPattern.name}");
            Instantiate(curPattern, spawnPosition.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("유효하지 않은 패턴입니다.");
        }

        if (!isboss)
        {
            curCount++;
            bossCount++;
        }
    }

    private int GetNextPatternIndex(ref int randomRate)
    {
        var enemyScrollController = curPattern.GetComponent<EnemyScrollController>();

        if (randomRate < 50)
        {
            int index = Random.Range(0, enemyScrollController.next50Pattern.Length);
            randomRate = 50;
            return enemyScrollController.next50Pattern[index];
       
        }
        else if (randomRate < 85)
        {
            int index = Random.Range(0, enemyScrollController.next35Pattern.Length);
            randomRate = 35;
            return enemyScrollController.next35Pattern[index];
        }
        else
        {
            int index = Random.Range(0, enemyScrollController.next15Pattern.Length);
            randomRate = 15;
            return enemyScrollController.next15Pattern[index];
        }
    }

    void StartSpawn()
    {
        int randomIndex = Random.Range(0, 3);
        curPattern = Patterns[randomIndex];
        Instantiate(Patterns[randomIndex], spawnPosition.localPosition, Quaternion.identity);    
    }

    void SpawnEnchant()
    {
        GameObject option  = Instantiate(enchantOption, spawnPosition.localPosition, Quaternion.identity);
        option.GetComponent<Rigidbody2D>().velocity = Vector2.down * optionSpeed;
    }

    void SpawnBoss()
    {
        if(bossCount >= bossSpawnCount[curEvent])
        {
            isboss = true;
            //Instantiate(bossPrefab[curEvent]);
        }
    }
}


