using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : MonoBehaviour
{
    protected PlayerStat playerStat;


    [SerializeField] protected int damage;
    [SerializeField] protected float damageRate = 1f;
    [SerializeField] protected float TotalDamage;

    [SerializeField] protected BulletType bulletType;

    [SerializeField] protected bool isSkillBullet;
    [SerializeField] protected bool isSubBullet;

    [SerializeField] protected bool isIce = false;
    [SerializeField] protected float slowRate = 0f;
    [SerializeField] protected float damageUp = 0f;

    private void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    public virtual void SetDamagebuff(float rate)
    {
        damageRate = rate;
    }

    public virtual void SetBulletType(BulletType bulletType)
    {
        this.bulletType = bulletType;
    }

    public virtual void SetDamage(int Damage)
    {
        damage = Damage;
    }

    public virtual void SetSlowRate(float slowRate)
    {
        this.slowRate += slowRate;
    }
    public virtual void SetIce(bool ice)
    {
        isIce = ice;
    }
    public virtual void IncreaseDamage(float damage)
    {
        damageUp += damage; 
        if(damageUp < 0f)
        {
            damageUp = 0f;
        }
    }

    public virtual bool GetSubBullet() => isSubBullet;
    public virtual bool GetIce() => isIce;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyStat enemyStat = collision.GetComponent<EnemyStat>();
        
        if (enemyStat.GetIsIce())
        {
            TotalDamage = damage * damageRate;
        }
        else
        {
            TotalDamage = (damage + damageUp) * damageRate;
        }

        if (collision.CompareTag("Enemy"))
        {
            bool canCount = true;
            if (playerStat.canDrain && !isSkillBullet && !isSubBullet)
            {
                playerStat.Drain((int)TotalDamage);              
            }

            if(isIce&& !enemyStat.GetIsIce() && !isSkillBullet)
            {
                enemyStat.SetIsIce(true);
                enemyStat.DecreaseSpeed(slowRate);
            }

         

            if(isSubBullet) canCount = false;
            
            enemyStat.TakeDamage((int)TotalDamage, canCount);
            gameObject.SetActive(false);


        }
    }
}
