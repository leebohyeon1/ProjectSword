using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tenkai : MagicSword
{
    [SerializeField] private Vector3 detectRadius;

    float timer = 0f;
    Vector3 Pos;
    PlayerStat playerStat;

    [Header("고유 능력 진화")]

    [Header("레벨 1")]
    [SerializeField] private int damageUp = 10;

    [Header("레벨 2")]
    [SerializeField] private float buff2Slow = 20f;

    [Header("레벨 3")]
    [SerializeField] private int buff3DiffusionCount = 3;

    [Header("레벨 4")]
    [SerializeField] private int buff4Damage = 4;

    //================================================================================================

    void Start()
    {
        SetTrans();

        SetFire();

        InitializePool();

        Pos = new Vector3(transform.position.x, transform.position.y + (detectRadius.y / 2), 0);
    }

    void Update()
    {
        Follow();
        Attack();
    }

    //================================================================================================

    private void Buff1()
    {
        plusAttackPower += damageUp;
    }
    
    private void Buff2()
    {
        foreach (GameObject bullet in bulletPool)
        {
            TenkaiBullet bullets = bullet.GetComponent<TenkaiBullet>();
            bullets.SetBuff2(true,buff2Slow);
        }
    } 
    
    private void Buff3()
    {
        foreach (GameObject bullet in bulletPool)
        {
            TenkaiBullet bullets = bullet.GetComponent<TenkaiBullet>();
            bullets.SetBuff3(buff3DiffusionCount);
        }
    } 
    
    private void Buff4()
    {
        foreach (GameObject bullet in bulletPool)
        {
            TenkaiBullet bullets = bullet.GetComponent<TenkaiBullet>();
            bullets.SetBuff4(buff4Damage);
        }
    }

    public Vector3 GetDetectRadius() { return detectRadius; }


    public void Attack()
    {
        timer += Time.deltaTime;
        if (timer > attackSpeed)
        {
            Fire();
            timer = 0f;
        }
    }
    public void Evoltion1() { }
    public void Evoltion2() { }
    public void Evoltion3() { }
    public void Evoltion4() { }
    //================================================================================================

    public override void SetTrans()
    {
        base.SetTrans();
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    public override void SetFire()
    {
        firePos.AddComponent<TenkaiFire>();

        firePos.GetComponent<TenkaiFire>().SetMagicSword(this);

        firePos.GetComponent<TenkaiFire>().SetTenkai(bulletPrefab.GetComponent<TenkaiBullet>(), this);
    }
    protected override void Follow()
    {
        base.Follow();
    }

    protected override void InitializePool()
    {
        base.InitializePool();
    }

    public override void SetBullet()
    {
        firePos.GetComponent<TenkaiFire>().enabled = !firePos.GetComponent<TenkaiFire>().enabled;
    }

    protected override GameObject GetBullet()
    {
        return base.GetBullet();
    }

    public override void Fire()
    {
        Transform closestEnemy = null;

        float closestDistance = Mathf.Infinity;
        EnemyStat lowestHealthEnemy = null;

        Collider2D[] enemies = Physics2D.OverlapBoxAll(Pos, detectRadius, 0, bulletPrefab.GetComponent<TenkaiBullet>().GetEnemyLayer());

        foreach (Collider2D enemyCollider in enemies)
        {

            Transform enemyTransform = enemyCollider.transform;
            float distance = Vector3.Distance(transform.position, enemyTransform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyTransform;
                lowestHealthEnemy = enemyCollider.GetComponent<EnemyStat>();
            }
            else if (distance == closestDistance)
            {
                EnemyStat currentEnemyStat = enemyCollider.GetComponent<EnemyStat>();
                if (currentEnemyStat != null && lowestHealthEnemy != null)
                {
                    if (currentEnemyStat.HP < lowestHealthEnemy.HP)
                    {
                        closestEnemy = enemyTransform;
                        lowestHealthEnemy = currentEnemyStat;
                    }
                }
            }
        }

        if (closestEnemy != null)
        {
            GameObject bullet = GetBullet();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.GetComponent<BulletController>().SetSubBullet();
            bullet.SetActive(true);

            //Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bullet.transform.position = closestEnemy.position;
        }
    }

    public override GameObject GetBulletPrefab() => base.GetBulletPrefab();

    public override void SetSword(Transform Trans, int AttackPower, float AttackSpeed, float BulletSpeed)
    {
        base.SetSword(Trans, AttackPower, AttackSpeed, BulletSpeed);
    }

    protected override void ApplyBuffEffects()
    {
        switch (buffLevel)
        {
            case 1:
                Buff1();
                break;
            case 2:
                Buff2();
                break;
            case 3:
                Buff3();
                break;
            case 4:
                Buff4();
                break;
        }
    }
    protected override void ApplyEvolutionEffects()
    {
        switch (evolutionLevel)
        {
            case 1:
                Evoltion1();
                break;
            case 2:
                Evoltion2();
                break;
            case 3:
                Evoltion3();
                break;
            case 4:
                Evoltion4();
                break;
        }
    }


    //================================================================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Pos, detectRadius);
    }

}
