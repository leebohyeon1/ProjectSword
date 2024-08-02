using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidebiteBullet : BulletController
{
    [SerializeField] private bool isMolar;
    [SerializeField] private int molarDamage = 0;
    [SerializeField] private int molarLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        damageRate = 1f;
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SetDamagebuff(float rate)
    {
        base.SetDamagebuff(rate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyStat enemyStat = collision.GetComponent<EnemyStat>();
            TotalDamage = damage * damageRate;

            if (enemyStat.isMolar)
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

            if (isIce && !enemyStat.isIce && !isSkillBullet)
            {
                enemyStat.isIce = true;
                enemyStat.DecreaseSpeed(slowRate);

            }

            if (isMolar)
            {
                enemyStat.isMolar = true;
            }
        }


      
    }

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

    public bool GetMolar => isMolar;
}