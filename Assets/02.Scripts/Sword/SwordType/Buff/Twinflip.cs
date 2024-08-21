using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinflip : MagicSword
{
    private PlayerStat playerStat;

    [SerializeField] private float[] power = new float[2];

    [SerializeField] private float duration = 6f;

    private bool isFireBuff = true;
    private float timer = 0f;

    private bool[] isBuff = new bool[4];
    private float fireBuffTimer;
    private float iceBuffTimer;

    [Header("불속성 총알 변화")]
    [SerializeField] private float changeAttackSpeed;
    [SerializeField] private int changeAttackDamage;
    [SerializeField] private float changeBulletSpeed;

    [Header("고유 능력 진화")]

    [Header("레벨 1")]
    [SerializeField] private int damageUp = 10;
    private int index = 0;
    private int iindex = 0;
    [Header("레벨 2")]
    [SerializeField] private float[] buffPower = new float[2];

    [Header("레벨 3")]
    [SerializeField] private int[] damage;
    [SerializeField] private float[] distance;

    [Header("레벨 4")]
    [SerializeField] private int iceDamageUp = 2;


    [Header("진화의 룬")]

    private bool level2 = false;
    private bool level3 = false;
    [Header("레벨 1")]
    [SerializeField] private float attackSpeedUp = 0f;
    [SerializeField] private float slowRateUp = 0f;

    [Header("레벨 2")]
    [SerializeField] private float criticalRateUp = 0f;
    [SerializeField] private float plusDamage = 0f;

    [Header("레벨 3")]
    [SerializeField] private bool canPenetration;

    BulletType type;
    int swapDamage;
    //==================================================================================

    private void Awake()
    {
        for (int i = 0; i < isBuff.Length; i++)
        {
            isBuff[i] = false;
        }
    }

    void Start()
    {
        SetTrans();
        SetFire();
        InitializePool();

        for (int i = 0; i < 2; i++)
        {
            if (playerStat.GetSwords()[i].GetComponent<Twinflip>() == this)
            {
                playerStat.upAttackSpeed[i] -= attackSpeedUp;
                iindex = i;
                break;
            }
        }
    }

    void Update()
    {
        Follow();
        Fire();
    }

    //================================================================================================
    private void FireBuff()
    {
        playerStat.SetCriticalRate(power[0]);

        fireBuffTimer = 0f;
        isBuff[0] = true;
    }

    private void IceBuff()
    {
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            BulletController bul = bullet.GetComponent<BulletController>(); 
            type = bul.GetBulletType();
        }
        playerStat.bulletType = BulletType.Ice;
        if (isBuff[3])
        {
            playerStat.SetBulletIce(power[1], iceDamageUp);
        }
        else
        {
            playerStat.SetBulletIce(power[1], 0);
        }
            
        iceBuffTimer = 0f;
        isBuff[1] = true;
    }

    private void FireBuffOff()
    {
        isBuff[0] = false;
        fireBuffTimer = 0f;

        playerStat.SetCriticalRate(-power[0]);

        if (isBuff[2])
        {
            playerStat.upAttackDamage[index] -= damageUp;
            index = playerStat.GetWeaponIndex();
        }

    }

    private void IceBuffOff()
    {
        isBuff[1] = false;
        iceBuffTimer = 0f;

        playerStat.bulletType = type;
        playerStat.SetBulletIce(-power[1], -iceDamageUp);


        if (isBuff[2])
        {
            playerStat.upAttackDamage[index] -= damageUp;
            index = playerStat.GetWeaponIndex();
        }
    }

    private void Buff1()
    {
        isBuff[2] = true;
        index = playerStat.GetWeaponIndex();
        if (isBuff[1] || isBuff[2])
        {
            playerStat.upAttackDamage[index] += damageUp;
        }
    }

    private void Buff2()
    {
       
        power[0] += buffPower[0];
    }

    private void Buff3()
    {
        GameManager.Instance.SetTwinflip3(true, ref damage, ref distance);
    }

    private void Buff4()
    {
        isBuff[3] = true;
        power[1] += buffPower[1];
    }

    public void Evoltion1()
    {
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
            if (bullets.GetBulletType() == BulletType.Fire)
            {
                playerStat.upAttackSpeed[index] -= attackSpeedUp;
                break;
            }
            else
            {
                power[1] += slowRateUp;
                break;
            }
        }
    }
    public void Evoltion2()
    {
        level2 = true;
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
            if (bullets.GetBulletType() == BulletType.Fire)
            {
                playerStat.SetCriticalRate(criticalRateUp);
                break;
            }
            else
            {
                bullets.IncreaseDamage(plusDamage);
            }
        }
    }
    public void Evoltion3()
    {
        level3 = true;
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
            bullets.Spear(); 
        }
    }
    public void Evoltion4() 
    {
        GameManager.Instance.SetTwinflip4(true);
    }


    public override void SetLevel()
    {
        if (level2)
        {
            foreach (GameObject bullet in playerStat.bulletPool_)
            {
                TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
                if (bullets.GetBulletType() == BulletType.Fire)
                {
                    playerStat.SetCriticalRate(criticalRateUp);
                    break;
                }
                else
                {
                    bullets.IncreaseDamage(plusDamage);
                }
            }
        }
        if (level3)
        {
            foreach (GameObject bullet in playerStat.bulletPool_)
            {
                TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
                bullets.Spear();
            }
        }

        
    }

    public override void ActiveSwapBuff()
    {
        switch (playerStat.swapBuff[index])
        {
            case 0:
            case 1:
            case 2:
                swapDamage = 1;
                break;
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
                swapDamage = 2;
                break;
            case 10:
                swapDamage = 3;
                break;
        }

        StartCoroutine(TwinSwapBuff());
    }

    public IEnumerator TwinSwapBuff()
    {
        for(int i = 0; i < 2; i++)
        {
            List<GameObject> pool = playerStat.GetSwords()[i].GetComponent<MagicSword>().BulletPool;
            playerStat.GetSwords()[i].GetComponent<MagicSword>().IncreaseSubDamage(swapDamage);
            foreach (GameObject bullet in pool)
            {
                bullet.GetComponent<BulletController>().SetTwinSwap(true);   
            }
        }

        yield return new WaitForSeconds(swapBuffPower[playerStat.swapBuff[index]]);
        for (int i = 0; i < 2; i++)
        {
            List<GameObject> pool = playerStat.GetSwords()[i].GetComponent<MagicSword>().BulletPool;
            playerStat.GetSwords()[i].GetComponent<MagicSword>().IncreaseSubDamage(-swapDamage);
            foreach (GameObject bullet in pool)
            {
                bullet.GetComponent<BulletController>().SetTwinSwap(false);
            }
        }
    }

    public void ChangeFireBullet()
    {
       
        playerStat.upAttackDamage[iindex] += changeAttackDamage;
        playerStat.upAttackSpeed[iindex] -= changeAttackSpeed;
        playerStat.upBulletSpeed[iindex] += changeBulletSpeed;
       
    }

    public void ChangeIceBullet()
    {
        
        playerStat.upAttackDamage[iindex] -= changeAttackDamage;
        playerStat.upAttackSpeed[iindex] += changeAttackSpeed;
        playerStat.upBulletSpeed[iindex] -= changeBulletSpeed;
        
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

    public override void Fire()
    {
        // 타이머를 증가시키고, Duration이 지나면 Buff를 변경

        timer += Time.deltaTime;
        if (timer >= attackSpeed)
        {
            timer = 0f; // 타이머 리셋

            if (isBuff[2])
            {
                index = playerStat.GetWeaponIndex();
                playerStat.upAttackDamage[index] += damageUp;
            }

            if (isFireBuff)
            {
                FireBuff();
            }
            else
            {
                IceBuff();
            }

            isFireBuff = !isFireBuff; // 다음에 호출할 Buff 변경
        }

        if (isBuff[0])
        {
            fireBuffTimer += Time.deltaTime;
            if (fireBuffTimer >= duration)
            {
                FireBuffOff();
            }
        }

        if (isBuff[1])
        {
            iceBuffTimer += Time.deltaTime;
            if (iceBuffTimer >= duration)
            {
                IceBuffOff();
            }
        }
    }

    public override void SetFire()
    {
        base.SetFire();
    }

    protected override void InitializePool()
    {
        base.InitializePool();
    }


    public override void IncreaseSubDamage(int dam)
    {
        base.IncreaseSubDamage(dam);
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
        Gizmos.color = new Color(1, 0, 0);

        for (int i = 0; i < GameManager.Instance.TwinDis.Length; i++)
        {
            Gizmos.DrawLine(new Vector3(100, playerStat.transform.position.y + GameManager.Instance.TwinDis[i], 0), new Vector3(-100, transform.position.y + GameManager.Instance.TwinDis[i], 0));
        }
    }

}