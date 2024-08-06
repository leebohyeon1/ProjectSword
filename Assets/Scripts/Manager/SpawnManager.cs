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
    public float spawnInterval = 2.0f; // 적 생성 주기
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
    public GameObject[] BossEnemyPatterns;
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

    private void SpawnEnemy()
    {
        curCount++;
        bossCount++;

        if(bossCount >= bossSpawnCount[curEvent])
        {
            SpawnBoss();
            return;
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
            int index = Random.Range(0, enemyScrollController.nextFirstPattern.Length);
            randomRate = 50;
            return enemyScrollController.nextFirstPattern[index];
       
        }
        else if (randomRate < 85)
        {
            int index = Random.Range(0, enemyScrollController.nextSecondPattern.Length);
            randomRate = 35;
            return enemyScrollController.nextSecondPattern[index];
        }
        else
        {
            int index = Random.Range(0, enemyScrollController.nextThirdPattern.Length);
            randomRate = 15;
            return enemyScrollController.nextThirdPattern[index];
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
        Instantiate(bossPrefab[curEvent]);
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
            int index = Random.Range(0, enemyScrollController.nextFirstPattern.Length);
            randomRate = 45;
            return enemyScrollController.nextFirstPattern[index];

        }
        else if (randomRate < 80)
        {
            int index = Random.Range(0, enemyScrollController.nextSecondPattern.Length);
            randomRate = 35;
            return enemyScrollController.nextSecondPattern[index];
        }
        else
        {
            int index = Random.Range(0, enemyScrollController.nextThirdPattern.Length);
            randomRate = 20;
            return enemyScrollController.nextThirdPattern[index];
        }
    }


    public void KillBoss()
    {
        isboss = false;
        GameUIManager.Instance.BossUIOff();
    }
}


