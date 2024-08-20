using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance {  get; private set; }

    //==================================================================================
    [Header("적 스폰")]
    [SerializeField] private GameObject[] Patterns; // 적 프리팹
    private GameObject curPattern;
    [SerializeField] private float spawnInterval = 2.0f; // 적 생성 주기
    private float spawnTimer = 0.0f;
    [SerializeField] private float plusAcceleration = 0;
    [SerializeField] private int hpUpInterval = 2;
    [SerializeField] private int hpUpAmount = 4;
    private int hpCount = 0;
    public int totalHp = 0;

    [Header("선택지")]
    [SerializeField] private GameObject enchantOption;
    [SerializeField] private int spawnCount = 5;
    [SerializeField]
    private int curCount = 0;
    [SerializeField] private float optionSpeed = 5f;

    [Space(20f)]
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform bossSpawnPosition;

    [Header("보스")]
    [SerializeField] private int bossCount = 0;
    [SerializeField] private int[] bossSpawnCount;
    [SerializeField] private GameObject[] bossPrefab;
    private int curEvent = 0;
    private bool isboss;
    [SerializeField] private GameObject[] BossEnemyPatterns;

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
        if (!isboss)
        {
            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemy();
                spawnTimer = 0f;
            }

            if (curCount >= spawnCount)
            {
                SpawnEnchant();
                curCount = 0;
            }
        }
        else
        {
            if(spawnTimer >= spawnInterval)
            {
                SpawnBossEnemy();
                spawnTimer = 0f;
            }
        }

      
    }

    //==================================================================================
    public float PlusAcceleration() => plusAcceleration;
    public int BossCount() => bossCount; 
    public int[] BossSpawnCount() => bossSpawnCount;
    public int BossLength() => bossSpawnCount.Length;

    //==================================================================================
   
    private void SpawnEnemy()
    {
        curCount++;
        bossCount++;
        if(!isboss)
        {
            hpCount++;
        }
        

        if (bossCount >= bossSpawnCount[curEvent])
        {
            SpawnBoss();
            return;
        }

        if(hpCount >= hpUpInterval)
        {
            hpCount = 0;
            totalHp += hpUpAmount;
        }

        int randomRate = Random.Range(0, 100);
        int nextPatternIndex = GetNextPatternIndex(ref randomRate);

        if (nextPatternIndex >= 0 && nextPatternIndex < Patterns.Length)
        {
            curPattern = Patterns[nextPatternIndex];
            Instantiate(curPattern, spawnPosition.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("유효하지 않은 패턴입니다.");
        }
   
        
    }

    private int GetNextPatternIndex(ref int randomRate)
    {
        var enemyScrollController = curPattern.GetComponent<EnemyScrollController>();

        if (randomRate < 50)
        {
            int index = Random.Range(0, enemyScrollController.NextPattern(0).Length);
            randomRate = 50;
            return enemyScrollController.NextPattern(0)[index];
       
        }
        else if (randomRate < 85)
        {
            int index = Random.Range(0, enemyScrollController.NextPattern(1).Length);
            randomRate = 35;
            return enemyScrollController.NextPattern(1)[index];
        }
        else
        {
            int index = Random.Range(0, enemyScrollController.NextPattern(2).Length);
            randomRate = 15;
            return enemyScrollController.NextPattern(2)[index];
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
        isboss = true;
        spawnTimer = 0f;

        GameUIManager.Instance.BossUIOn(bossPrefab[curEvent].name);
        Instantiate(bossPrefab[curEvent],bossSpawnPosition);
        StartBossPatternSpawn();

        curEvent += 1;
    }

    void StartBossPatternSpawn()
    {
        int randomIndex = Random.Range(0, 3);
        curPattern = BossEnemyPatterns[randomIndex];
        Instantiate(BossEnemyPatterns[randomIndex], spawnPosition.localPosition, Quaternion.identity);
    }

    private void SpawnBossEnemy()
    {
        int randomRate = Random.Range(0, 100);
        int nextPatternIndex = GetNextBossPatternIndex(ref randomRate);

        if (nextPatternIndex >= 0 && nextPatternIndex < Patterns.Length)
        {
            curPattern = Patterns[nextPatternIndex];
            Instantiate(curPattern, spawnPosition.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("유효하지 않은 패턴입니다.");
        }


    }

    private int GetNextBossPatternIndex(ref int randomRate)
    {
        var enemyScrollController = curPattern.GetComponent<EnemyScrollController>();

        if (randomRate < 45)
        {
            int index = Random.Range(0, enemyScrollController.NextPattern(0).Length);
            randomRate = 45;
            return enemyScrollController.NextPattern(0)[index];

        }
        else if (randomRate < 80)
        {
            int index = Random.Range(0, enemyScrollController.NextPattern(1).Length);
            randomRate = 35;
            return enemyScrollController.NextPattern(1)[index];
        }
        else
        {
            int index = Random.Range(0, enemyScrollController.NextPattern(2).Length);
            randomRate = 20;
            return enemyScrollController.NextPattern(2)[index];
        }
    }


    public void KillBoss()
    {
        isboss = false;
        GameUIManager.Instance.BossUIOff();
    }


}


