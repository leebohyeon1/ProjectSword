using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidebiteBullet : BulletController
{
    [SerializeField] private bool isMolar;
    [SerializeField] private int molarDamage = 0;
    [SerializeField] private int molarLevel = 0;

    //=============================================================================

    void Start()
    {
        damageRate = 1f;
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    //=============================================================================

    public void SetMolar(bool molar_bool)
    {
        isMolar = molar_bool;
    }

    public void SetMolar(int level)
    {
        molarLevel = level;
    }

    public void SetMolarDamage(int molarDamage)
    {
        this.molarDamage = molarDamage;
    }

    public bool GetMolar() => isMolar;

    //=============================================================================

    public override void SetBulletType(BulletType bulletType)
    {
        base.SetBulletType(bulletType);
    }
    public override void SetDamagebuff(float rate)
    {
        base.SetDamagebuff(rate);
    }
    public override void SetDamage(int Damage)
    {
        base.SetDamage(Damage);
    }
    public override void SetSlowRate(float slowRate)
    {
        base.SetSlowRate(slowRate);
    }
    public override void SetIce(bool ice)
    {
        base.SetIce(ice);
    }
    public override void IncreaseDamage(float damage)
    {
        base.IncreaseDamage(damage);
    }

    public override bool GetSubBullet()
    {
        return base.GetSubBullet();
    }
    public override bool GetIce()
    {
        return base.GetIce();
    }

    //=============================================================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TotalDamage = damage * damageRate;
        if (collision.CompareTag("Enemy"))
        {
            EnemyStat enemyStat = collision.GetComponent<EnemyStat>();


            if (enemyStat.GetIsMolar())
            {
                enemyStat.TakeDamage((int)((damage + (molarDamage * molarLevel)) * damageRate));
            }
            else
            {
                enemyStat.TakeDamage((int)TotalDamage);

            }

            gameObject.SetActive(false);

            if (playerStat.canDrain && !isSkillBullet && !isSubBullet)
            {
                playerStat.Drain((int)TotalDamage);
            }

            if (isIce && !enemyStat.GetIsIce() && !isSkillBullet)
            {
                enemyStat.SetIsIce(true);
                enemyStat.DecreaseSpeed(slowRate);

            }

            if (isMolar)
            {
                enemyStat.SetIsMolar(true);
            }
        }
    }
}