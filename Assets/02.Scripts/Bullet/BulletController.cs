using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : MonoBehaviour
{
    protected PlayerStat playerStat;

    [SerializeField] protected int damage;
    [SerializeField] protected float criticalDamage;
    [SerializeField] protected float damageRate = 1f;
    [SerializeField] protected float TotalDamage;

    [SerializeField] protected BulletType bulletType;

    [SerializeField] protected bool isSkillBullet;
    [SerializeField] protected bool isSubBullet;

    [SerializeField] protected bool isIce = false;
    [SerializeField] protected bool isTwinflip3 = false;
    [SerializeField] protected bool isCritical = false;
    [SerializeField] protected bool isTwinSwap = false;
   
    [SerializeField] protected float slowRate = 0f;
    [SerializeField] protected float damageUp = 0f;

    //=============================================================================

    private void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    //=============================================================================

    public virtual void SetDamagebuff(float rate)
    {
        damageRate = rate;
    }

    public virtual void SetBulletType(BulletType bulletType)
    {
        this.bulletType = bulletType;
    }

    public virtual void SetDamage(int Damage, float Critical = 0, bool boolean = false )
    {
        damage = Damage;
        isCritical = boolean;
        criticalDamage = Critical;
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

    public virtual void SetTwinflip3(bool twinflip3)
    {
        isTwinflip3 = twinflip3;
    }

    public virtual bool GetSubBullet() => isSubBullet;
    public virtual BulletType GetBulletType() => bulletType;

    public virtual void SetSubBullet()
    {
        isSubBullet = true;
    }

    public virtual bool GetIce() => isIce;

    public virtual void SetTwinSwap(bool bo)
    {
        isTwinSwap = bo;
    }

    protected virtual int CalculateTwinDamage(float dis)
    {
        if (dis >= GameManager.Instance.TwinDis[0])
        {
            TotalDamage += GameManager.Instance.TwinDam[0];
            Debug.Log("거리" + 0);
        }
        else if (dis >= GameManager.Instance.TwinDis[1])
        {
            TotalDamage += GameManager.Instance.TwinDam[1];
            Debug.Log("거리" + 1);
        }
        else
        {
            TotalDamage += GameManager.Instance.TwinDam[2];
            Debug.Log("거리" + 2);
        }

        Debug.Log("데미지: " + TotalDamage);
        return (int)TotalDamage;
    }
    //=============================================================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyStat enemyStat = collision.GetComponent<EnemyStat>();
        
        if (enemyStat.GetIsIce())
        {
            TotalDamage = (damage + damageUp) * damageRate;
        }
        else
        {
            TotalDamage = damage * damageRate;
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

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
