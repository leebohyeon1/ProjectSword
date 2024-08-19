using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionBullet : BulletController
{
    [SerializeField] private bool isDandelion = false;

    [SerializeField] private bool isFlower = false;

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
                    if (isTwinflip3)
                    {
                        Vector3 screenPosition = Camera.main.WorldToScreenPoint(collision.transform.position);

                        // 화면의 절반 이하에 있는지 확인
                        if (screenPosition.y < Screen.height / 2)
                        {
                            float dis = Vector2.Distance(playerStat.transform.position, collision.transform.position);
                            Debug.Log("거리: " + dis);
                            TotalDamage = CalculateTwinDamage(dis);
                            if (isCritical)
                            {
                                TotalDamage *= (1 + criticalDamage / 100);
                            }
                            collision.GetComponent<EnemyStat>().TakeDamage((int)TotalDamage);
                        }
                    }
                    else
                    {
                        if (isCritical)
                        {
                            TotalDamage *= (1 + criticalDamage / 100);
                        }
                        collision.GetComponent<EnemyStat>().TakeDamage((int)TotalDamage);
                    }

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

    public void SetFlower(bool Boolean)
    {
        isFlower = Boolean;
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

    public override void SetDamage(int Damage, float Cri = 0, bool f = false)
    {
        base.SetDamage(Damage, Cri, f);
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

    public override void SetTwinflip3(bool twinflip3)
    {
        base.SetTwinflip3(twinflip3);
    }
    public override BulletType GetBulletType()
    {
        return base.GetBulletType();
    }
    public override bool GetSubBullet()
    {
        return base.GetSubBullet();
    }
    public override void SetSubBullet()
    {
        base.SetSubBullet();
    }
    public override bool GetIce()
    {
        return base.GetIce();
    }

    protected override int CalculateTwinDamage(float distance)
    {
        return base.CalculateTwinDamage(distance);
    }
    //=============================================================================

    private void OnTriggerEnter2D(Collider2D collision)
    {

      
        if (collision.CompareTag("Enemy"))
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

            if (!isSubBullet && isFlower)
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.COUNT_FLOWER, this);
            }

            if (isDandelion)
            {
                StartCoroutine(SkillB(collision));
            }
            else
            {
                if (isTwinflip3)
                {
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(collision.transform.position);

                    // 화면의 절반 이하에 있는지 확인
                    if (screenPosition.y < Screen.height / 2)
                    {
                        float dis = Vector2.Distance(playerStat.transform.position, collision.transform.position);

                        enemyStat.TakeDamage(CalculateTwinDamage(dis));
                    }
                }
                else
                {
                    if (isCritical)
                    {
                        TotalDamage *= (1 + criticalDamage / 100);
                    }
                    enemyStat.TakeDamage((int)TotalDamage);
                }

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
