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
    
    private bool[] isBuff = new bool[2];
    private float fireBuffTimer;
    private float iceBuffTimer;


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
            if(fireBuffTimer >= duration)
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

    private void FireBuff()
    {
        playerStat.CriticalRate += power[0];
        fireBuffTimer = 0f;
        isBuff[0] = true;
        Debug.Log("불버프 On");
    }

    private void IceBuff()
    {
        playerStat.bulletType = BulletType.Ice;
        playerStat.SetBulletIce(power[1]);
        iceBuffTimer = 0f;
        isBuff[1] = true;
        Debug.Log("물버프 On");
    }

    private void FireBuffOff()
    {
        isBuff[0] = false;
        fireBuffTimer = 0f;
       
        playerStat.CriticalRate -= power[0];
        
    }

    private void IceBuffOff()
    {
        isBuff[1] = false;
        iceBuffTimer = 0f;
      
        playerStat.bulletType = playerStat.weapon[playerStat.weaponIndex].bulletType;
        playerStat.SetBulletIce(power[1]);
  
    }
}