using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionBullet : BulletController
{
    public bool isDandelion = false;

    public bool isLevel3 = false;

    void Start()
    {
        damageRate = 1f;
        playerStat = FindFirstObjectByType<PlayerStat>();
    }


    public override void SetDamagebuff(float rate)
    {
        base.SetDamagebuff(rate);
    }

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

        //if(collision.CompareTag("Boss"))
        //{
        //    BossStat bossStat = collision.GetComponent<BossStat>();
        //    if (isDandelion)
        //    {
        //        StartCoroutine(SkillB(collision));
        //    }
        //    else
        //    {
        //        bossStat.TakeDamage((int)TotalDamage);
        //        gameObject.SetActive(false);
        //    }

        //    if (playerStat.canDrain && !isSkillBullet && !isSubBullet)
        //    {
        //        playerStat.Drain((int)TotalDamage);
        //    }

        //    if (isIce && !bossStat.GetIsIce() && !isSkillBullet)
        //    {
        //        bossStat.SetIsIce(true);
        //        bossStat.DecreaseSpeed(slowRate);

        //    }
        //}
    }
       
    

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

}
