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
            Debug.Log("�Ÿ�"  + 0);
        }
        else if (dis >= distance[1])
        {
            TotalDamage += upDamage[1];
            Debug.Log("�Ÿ�" + 1);
        }
        else if (dis >= distance[2])
        {
            TotalDamage += upDamage[2];
            Debug.Log("�Ÿ�" + 2);
        }
        else if (dis >= distance[3])
        {
            TotalDamage += upDamage[3];
            Debug.Log("�Ÿ�" + 3);
        }
        else
        {
            TotalDamage *= 2;
        }
        Debug.Log("������: " + TotalDamage);
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

    public override void SetTwinflip3(bool twinflip3)
    {
        base.SetTwinflip3(twinflip3);
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
        TotalDamage = damage * damageRate;
        if (collision.CompareTag("Enemy"))
        {
            EnemyStat enemyStat = collision.GetComponent<EnemyStat>();


            if (isBSkill)
            {
                float dis = Vector2.Distance(player.position, collision.transform.position);
                Debug.Log("�Ÿ�: " + dis);
                CalculateDamage(dis);
                
                if (isTwinflip3)
                {
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(collision.transform.position);

                    // ȭ���� ���� ���Ͽ� �ִ��� Ȯ��
                    if (screenPosition.y < Screen.height / 2)
                    {
                        float dis1 = Vector2.Distance(playerStat.transform.position, collision.transform.position);
                        Debug.Log("�Ÿ�: " + dis1);
                        enemyStat.TakeDamage(CalculateTwinDamage(dis1));
                    }
                }
                else
                {
                    enemyStat.TakeDamage((int)TotalDamage);
                }

            }
            else
            {
                if (isTwinflip3)
                {
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(collision.transform.position);

                    // ȭ���� ���� ���Ͽ� �ִ��� Ȯ��
                    if (screenPosition.y < Screen.height / 2)
                    {
                        float dis1 = Vector2.Distance(playerStat.transform.position, collision.transform.position);
                        Debug.Log("�Ÿ�: " + dis1);
                        enemyStat.TakeDamage(CalculateTwinDamage(dis1));
                    }
                }
                else
                {
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

            gameObject.SetActive(false);
        }
    }
}
