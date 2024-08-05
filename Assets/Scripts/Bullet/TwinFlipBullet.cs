using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinFlipBullet : BulletController
{
    public Transform player;

    public bool isBSkill = false;

    public int[] upDamage;
    public float[] distance;

    // Start is called before the first frame update
    void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();
        player = playerStat.transform;
        damageRate = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TotalDamage = damage * damageRate;
        if (collision.CompareTag("Enemy"))
        {
            EnemyStat enemyStat = collision.GetComponent<EnemyStat>();
         

            if (isBSkill)
            {
                float dis = Vector2.Distance(player.position, collision.transform.position);
                Debug.Log("거리: " +dis);
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


            if (isIce && !enemyStat.isIce && !isSkillBullet)
            {
                enemyStat.isIce = true;
                enemyStat.DecreaseSpeed(slowRate);
               
            }

            gameObject.SetActive(false);
        }

        if(collision.CompareTag("Boss"))
        {
            BossStat bossStat = collision.GetComponent<BossStat>();
            if (isBSkill)
            {
                float dis = Vector2.Distance(player.position, collision.transform.position);
                Debug.Log("거리: " + dis);
                bossStat.TakeDamage(CalculateDamage(dis));
            }
            else
            {
                bossStat.TakeDamage((int)TotalDamage);
            }

            if (playerStat.canDrain && !isSkillBullet && !isSubBullet)
            {
                playerStat.Drain((int)TotalDamage);
            }


            if (isIce && !bossStat.isIce && !isSkillBullet)
            {
                bossStat.isIce = true;
                bossStat.DecreaseSpeed(slowRate);

            }

            gameObject.SetActive(false);
        }
    }

    int CalculateDamage(float dis)
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
}
