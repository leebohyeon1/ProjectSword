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
           
            if (isBSkill)
            {
                float dis = Vector2.Distance(player.position, collision.transform.position);
                Debug.Log("�Ÿ�: " +dis);
                collision.GetComponent<EnemyStat>().TakeDamage(CalculateDamage(dis));
            }
            else
            {
                collision.GetComponent<EnemyStat>().TakeDamage((int)TotalDamage);
            }

            if (playerStat.canDrain)
            {
                playerStat.Drain((int)TotalDamage);
            }
            gameObject.SetActive(false);
        }
    }

    int CalculateDamage(float dis)
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
}
