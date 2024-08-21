using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TwinFlipBullet : BulletController
{
    [SerializeField] private Transform player;

    [SerializeField] private bool isBSkill = false;

    [SerializeField] private int[] upDamage;
    [SerializeField] private float[] distance;

    private bool canPenetration = false;
    //=============================================================================

    void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();
        player = playerStat.transform;
        damageRate = 1;
    }

    //=============================================================================

    private int CalculateDamage(float dis)
    {
        if(dis >= distance[0])
        {
            TotalDamage += upDamage[0];
            Debug.Log("거리"  + 0);
        }
        else if (dis >= distance[1])
        {
            TotalDamage += upDamage[1];
            Debug.Log("거리" + 1);
        }
        else if (dis >= distance[2])
        {
            TotalDamage += upDamage[2];
            Debug.Log("거리" + 2);
        }
        else if (dis >= distance[3])
        {
            TotalDamage += upDamage[3];
            Debug.Log("거리" + 3);
        }
        else
        {
            TotalDamage *= upDamage[4];
        }
        Debug.Log("데미지: " + TotalDamage);
        return (int)TotalDamage;
    }

    public void SetBSkill(bool b) { isBSkill = b; }

    public float[] GetDistance() { return distance; }

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

    public void Spear()
    { 
        canPenetration = true;
    }
    public override void SetTwinSwap(bool bo)
    {
        base.SetTwinSwap(bo);
    }

    public void DamageUp(LengthDamage damage)
    {
        upDamage[0] = damage.firstLenDamage;
        upDamage[1] = damage.SecondLentDamage;
        upDamage[2] = damage.thirdLenDamage;
        upDamage[3] = damage.fourthLenDamage;
        upDamage[4] = damage.lastMultiplyDamage;
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

            if(GameManager.Instance.GetTwinflip4())
            {
                if (!enemyStat.GetFireStigma() && bulletType == BulletType.Fire)
                {
                    enemyStat.SetIsFIreStigma(true);
                }
                else if (!enemyStat.GetIceStigma() && bulletType == BulletType.Ice)
                {
                    enemyStat.SetIsIceStigma(true);
                }
            }
   

            if (isBSkill)
            {
                float dis = Vector2.Distance(player.position, collision.transform.position);
                Debug.Log("거리: " + dis);
                CalculateDamage(dis);
                
                if (isTwinflip3)
                {
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(collision.transform.position);

                    // 화면의 절반 이하에 있는지 확인
                    if (screenPosition.y < Screen.height / 2)
                    {
                        float dis1 = Vector2.Distance(playerStat.transform.position, collision.transform.position);
                        TotalDamage = CalculateTwinDamage(dis);
                        if (enemyStat.GetFireStigma() && bulletType == BulletType.Fire && !isCritical)
                        {
                            TotalDamage += (1 + criticalDamage / 100);
                            enemyStat.SetIsFIreStigma(false);
                        } 
                        else if (isCritical)
                        {
                            TotalDamage *= (1 + criticalDamage / 100);
                        }

                        if (enemyStat.GetIceStigma() && bulletType == BulletType.Ice)
                        {
                            TotalDamage *= 1.5f;
                        }

                        enemyStat.TakeDamage((int)TotalDamage);
                    }
                }
                else
                {
                    if (enemyStat.GetFireStigma() && bulletType == BulletType.Fire && !isCritical)
                    {
                        TotalDamage += (1 + criticalDamage / 100);
                        enemyStat.SetIsFIreStigma(false);
                    }
                    else if(isCritical)
                    {
                        TotalDamage *= (1 + criticalDamage / 100);
                    }

                    if (enemyStat.GetIceStigma() && bulletType == BulletType.Ice)
                    {
                        TotalDamage *= 1.5f;
                    }
                    enemyStat.TakeDamage((int)TotalDamage);
                }

            }
            else
            {
                if (isTwinflip3)
                {
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(collision.transform.position);

                    // 화면의 절반 이하에 있는지 확인
                    if (screenPosition.y < Screen.height / 2)
                    {
                        float dis1 = Vector2.Distance(playerStat.transform.position, collision.transform.position);
                        TotalDamage = CalculateTwinDamage(dis1);
                        
                        if (enemyStat.GetFireStigma() && bulletType == BulletType.Fire && !isCritical)
                        {
                            TotalDamage += (1 + criticalDamage / 100);
                            enemyStat.SetIsFIreStigma(false);
                        }
                        else if (isCritical)
                        {
                            TotalDamage *= (1 + criticalDamage / 100);
                        }

                        if (enemyStat.GetIceStigma() && bulletType == BulletType.Ice)
                        {
                            TotalDamage *= 1.5f;
                        }
                        enemyStat.TakeDamage((int)TotalDamage);
                    }
                }
                else
                {
                    if (enemyStat.GetFireStigma() && bulletType == BulletType.Fire && !isCritical)
                    {
                        TotalDamage += (1 + criticalDamage / 100);
                        enemyStat.SetIsFIreStigma(false);
                    }
                    else if(isCritical)
                    {
                        TotalDamage *= (1 + criticalDamage / 100);
                    }

                    if (enemyStat.GetIceStigma() && bulletType == BulletType.Ice)
                    {
                        TotalDamage *= 1.5f;
                    }
                    enemyStat.TakeDamage((int)TotalDamage);
                }
            }


            if (playerStat.canDrain && !isSkillBullet && !isSubBullet)
            {
                playerStat.Drain((int)TotalDamage);
            }


            if (isIce && !enemyStat.GetIsMolar() && !isSkillBullet)
            {
                enemyStat.SetIsIce(true);
                enemyStat.DecreaseSpeed(slowRate);
            }

            if(!canPenetration)
            {
                gameObject.SetActive(false);
            }
            else
            {
                canPenetration = false;
            }
    
        }
    }
}
