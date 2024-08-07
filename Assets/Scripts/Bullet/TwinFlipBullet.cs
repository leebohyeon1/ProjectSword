using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinFlipBullet : BulletController
{
    [SerializeField] private Transform player;

    [SerializeField] private bool isBSkill = false;

    [SerializeField] private int[] upDamage;
    [SerializeField] private float[] distance;

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
            TotalDamage *= 2;
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


            if (isBSkill)
            {
                float dis = Vector2.Distance(player.position, collision.transform.position);
                Debug.Log("거리: " + dis);
                enemyStat.TakeDamage(CalculateDamage(dis));
            }
            else
            {
                enemyStat.TakeDamage((int)TotalDamage);
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

            gameObject.SetActive(false);
        }
    }
}
