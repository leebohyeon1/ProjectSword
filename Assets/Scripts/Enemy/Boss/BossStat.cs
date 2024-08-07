using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : EnemyStat
{
    private PlayerStat playerStat;

    [Header("================")]
    [SerializeField] private int maxHp = 100;

    [Header("공격")]
    [SerializeField] private float criticalRate;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float bulletSpeed;

    [Header("탄막")]
    [SerializeField] private GameObject bossBulletPrefab;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private BulletType bulletType;
    private List<GameObject> bulletPool = new List<GameObject>();

    //==================================================================================

    void Start()
    {
       
        playerStat = FindFirstObjectByType<PlayerStat>();
     
        hp = maxHp;
        GameUIManager.Instance.UpdateBossUI(maxHp, hp);

        InitializePool();
  
    }

    void Update()
    {
        
    }

    //==================================================================================
   
    private void InitializePool()
    {
        bulletPool.Clear();
        for (int i = 0; i < 10; i++)
        {
            GameObject bullet = Instantiate(bossBulletPrefab);
            bullet.SetActive(false);
            bullet.transform.SetParent(bulletParent, true);
            bulletPool.Add(bullet);
        }
    }
    private void InitializeBullet(GameObject bullet)
    {
        var controller = bullet.GetComponent<BossBulletController>();
        controller.InitializeBullet(CalculateDamage(damage), bulletType);
    }

    private int CalculateDamage(int baseDamage)
    {
        bool isCritical = Random.Range(0f, 100f) < criticalRate;
        return isCritical ? Mathf.RoundToInt(baseDamage * (1 + criticalDamage / 100f)) : baseDamage;
   
    }

    public GameObject GetBullet()
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                InitializeBullet(bullet);
                return bullet;
            }
        }

        var newBullet = Instantiate(bossBulletPrefab);
        newBullet.SetActive(false);
        InitializeBullet(newBullet);
        bulletPool.Add(newBullet);
        return newBullet;
    }
    public Vector2 GetAttackVector()
    {
        Vector2 attackVector = playerStat.transform.position - transform.position;
        return attackVector.normalized;
    }

    public float GetBulletSpeed() => bulletSpeed;
    public float GetAttackSpeed() => attackSpeed;

    //==================================================================================

    public override void TakeDamage(int damage, bool count)
    {
        hp -= damage;
        GameUIManager.Instance.UpdateBossUI(maxHp, hp);

        if (hp <= 0)
        {
            Destroy(gameObject);
            SpawnManager.Instance.KillBoss();

            playerStat.SetMaxSkillGage();
            EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, swapGage);
        }
    }

    public override void DecreaseSpeed(float rate)
    {
       base.DecreaseSpeed(rate);
    }
    public override void IncreaseSpeed(float rate)
    {
       base.IncreaseSpeed(rate);
    }

    protected override void ApplySpeed()
    {
        // 모든 속도 감소율을 적용하여 최종 속도를 계산
        float totalRate = 0f;
        foreach (float modifier in speedModifiers)
        {
            totalRate += modifier;
        }

        float finalSpeed = speed * (1 - totalRate / 100f);
        GetComponent<Rigidbody2D>().velocity = Vector2.down * (finalSpeed + SpawnManager.Instance.PlusAcceleration());
    }

    public override bool GetIsIce() => base.GetIsIce();

    public override bool GetIsMolar() => base.GetIsMolar();

    public override void SetIsIce(bool boolean)
    {
        base.SetIsIce(boolean); 
    }

    public override void SetIsMolar(bool boolean)
    {
        base.SetIsMolar(boolean);   
    }

    public override int HP => base.HP;
}
