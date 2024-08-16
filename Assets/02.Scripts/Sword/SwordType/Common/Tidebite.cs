
using GooglePlayGames.BasicApi;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Tidebite : MagicSword
{
    private float timer = 0f;
    PlayerStat playerStat;

    [Header("고유 능력 진화")]

    private bool isBuffLevel2 = false;
    private bool isBuffLevel3 = false;
    [Header("레벨 1")]
    [SerializeField] private int damageUp = 10;

    [Header("레벨 2")]
    [SerializeField] private int molarDamage = 5;

    [Header("레벨 4")]
    [SerializeField] private int molarDamageUp = 2;

    [Header("진화의 룬")]
    
    [Header("레벨 1")]
    [SerializeField] private int attackDamageUp1 = 4;

    [Header("레벨 2")]
    [SerializeField] private int attackDamageUp2 = 4;

    [Header("레벨 3")]
    private bool level3 =false;
    [SerializeField] private float Level3Duration = 2f;
    //================================================================================================

    void Start()
    {
        SetTrans();

        SetFire();
        InitializePool();
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
           TidebiteBullet bullets = bullet.GetComponent<TidebiteBullet>();
            //bullets.SetDamagebuff(damageDownRate);
            bullets.SetMolar(true);
            bullets.SetMolar(1);
            bullets.SetMolarDamage(molarDamage);

        }
    }

    private void Buff3()
    {
        foreach (GameObject bullet in bulletPool)
        {
            TidebiteBullet bullets = bullet.GetComponent<TidebiteBullet>();
            bullets.SetMolar(2);
        }
    }

    private void Buff4()
    {
        molarDamage += molarDamageUp;
    }


    public void Evoltion1()
    {
        for (int i = 0; i < 2; i++)
        {
            if (playerStat.GetSwords()[i].GetComponent<Tidebite>() == this)
            {
                playerStat.upAttackDamage[i] += attackDamageUp1;
                break;
            }
        }
    }
    public void Evoltion2()
    {
        for (int i = 0; i < 2; i++)
        {
            if (playerStat.GetSwords()[i].GetComponent<Tidebite>() == this)
            {
                playerStat.upAttackDamage[i] += attackDamageUp2;
                break;
            }
        }
    }
    public void Evoltion3() 
    {
        level3 = true;
            
    }
    public void Evoltion4() 
    {
        GameManager.Instance.SetTidebiteLevel4(true);
    }

    public void SetLevel3() 
    {
        if(level3 )
        {
            StartCoroutine(Level3());
        }
    }

    private IEnumerator Level3()
    {
        GameManager.Instance.SetTidebiteLevel3(true);
        yield return new WaitForSeconds(Level3Duration);
        GameManager.Instance.SetTidebiteLevel3(false);
    }

    public override void SetLevel()
    {
        if (isBuffLevel2)
        {
            foreach (GameObject bullet in bulletPool)
            {
                TidebiteBullet bullets = bullet.GetComponent<TidebiteBullet>();
                //bullets.SetDamagebuff(damageDownRate);
                bullets.SetMolar(true);
                bullets.SetMolar(1);
                bullets.SetMolarDamage(molarDamage);

            }
        }
        if (isBuffLevel3)
        {
            foreach (GameObject bullet in bulletPool)
            {
                TidebiteBullet bullets = bullet.GetComponent<TidebiteBullet>();
                bullets.SetMolar(2);
            }
        }
    }
    //================================================================================================

    public override void SetTrans()
    {
        base.SetTrans();
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    protected override void Follow()
    {
        base.Follow();
    }

    protected override void InitializePool()
    {
        base.InitializePool();
    }

    public override void Fire()
    {
        base.Fire();
    }

    public void Attack()
    {
        timer += Time.deltaTime;
        if (timer > attackSpeed)
        {
            Fire();
            timer = 0f;
        }
    }

    public override void SetFire()
    {
        base.SetFire();
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

}