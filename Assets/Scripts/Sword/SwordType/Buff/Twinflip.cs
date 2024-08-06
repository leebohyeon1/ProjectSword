using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinflip : MagicSword
{
    private PlayerStat playerStat;

    public float[] power = new float[2];

    public float duration = 6f;

    private bool isFireBuff = true;
    private float timer = 0f;

    private bool[] isBuff = new bool[4];
    private float fireBuffTimer;
    private float iceBuffTimer;


    [Header("고유 능력 진화")]

    [Header("레벨 1")]
    public int damageUp = 10;
    private int index = 0;

    [Header("레벨 2")]
    public float[] buffPower = new float[2];

    [Header("레벨 4")]
    public int iceDamageUp = 2;

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
    }

    void Update()
    {
        Follow();
        Fire();
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
    
    public override GameObject GetBulletPrefab() => base.GetBulletPrefab();

    public override void SetSword(Transform Trans, int AttackPower, float AttackSpeed, float BulletSpeed)
    {
        base.SetSword(Trans, AttackPower, AttackSpeed, BulletSpeed);
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
        playerStat.bulletType = BulletType.Ice;
        if (isBuff[3])
        {
            playerStat.SetBulletIce(power[1], iceDamageUp);
        }
        else
        {
            playerStat.SetBulletIce(power[1]);
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

        playerStat.bulletType = playerStat.GetSwords().bulletType;
        playerStat.SetBulletIce(-power[1], -iceDamageUp);


        if (isBuff[2])
        {
            playerStat.upAttackDamage[index] -= damageUp;
            index = playerStat.GetWeaponIndex();
        }
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
                //      Buff3();
                break;
            case 4:
                Buff4();
                break;
        }
    }

    private void Buff1()
    {
        GetComponent<TwinFlipSkill>().isBuff = true;
        isBuff[2] = true;
        index = playerStat.GetWeaponIndex();
        if (isBuff[1] || isBuff[2])
        {
            playerStat.upAttackDamage[index] += damageUp;
        }

        Debug.Log(11);
    }

    private void Buff2()
    {
       
        power[0] += buffPower[0];
    }

    private void Buff3()
    {

    }

    private void Buff4()
    {
        isBuff[3] = true;
        power[1] += buffPower[1];
    }


}