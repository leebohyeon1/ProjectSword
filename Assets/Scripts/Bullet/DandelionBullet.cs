using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionBullet : BulletController
{
    [SerializeField] private bool isDandelion = false;

    [SerializeField] private bool isLevel3 = false;

    //=============================================================================

    void Start()
    {
        damageRate = 1f;
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    //=============================================================================

    public IEnumerator SkillB(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
           
            for (int i = 0; i < 2; i++)
            {
                if (collision != null)
                {
                    collision.GetComponent<EnemyStat>().TakeDamage((int)TotalDamage);
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                    if (playerStat.canDrain && !isSkillBullet && !isSubBullet)
                    {
                        playerStat.Drain((int)TotalDamage);
                    }

                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    break;
                }
            }
            gameObject.SetActive(false);
            yield return null;
        }
    }


    public void SetDandelionSkillB(bool boolean)
    {
        isDandelion = boolean;
    }

    public void SetBuffLevel3(bool Boolean)
    {
        isLevel3 = Boolean;
    }

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
            if (isDandelion)
            {
                StartCoroutine(SkillB(collision));
            }
            else
            {
                enemyStat.TakeDamage((int)TotalDamage);
                gameObject.SetActive(false);
            }

            if (playerStat.canDrain && !isSkillBullet && !isSubBullet)
            {
                playerStat.Drain((int)TotalDamage);
            }

            if (isIce && !enemyStat.GetIsIce() && !isSkillBullet)
            {
                enemyStat.SetIsIce(true);
                enemyStat.DecreaseSpeed(slowRate);

            }
        }
    }
}
