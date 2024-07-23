using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : MonoBehaviour
{
    protected PlayerStat playerStat;


    public int damage;
    public float damageRate;
    public float TotalDamage;

    public BulletType bulletType;

    public bool isSkillBullet;
    public bool isSubBullet;
    public bool isIce = false;
    public float slowRate = 0f;
    private void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    private void Update()
    {

    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    public virtual void SetDamagebuff(float rate)
    {
        damageRate = rate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyStat enemyStat = collision.GetComponent<EnemyStat>();
        TotalDamage = damage * damageRate;
        if (collision.CompareTag("Enemy"))
        {
            bool canCount = true;
            if (playerStat.canDrain && !isSkillBullet && !isSubBullet)
            {
                playerStat.Drain((int)TotalDamage);              
            }

            if(isIce&& !enemyStat.isIce)
            {
                enemyStat.DecreaseSpeed(slowRate);
            }

            if(isSubBullet) canCount = false;
            enemyStat.TakeDamage((int)TotalDamage, canCount);
            gameObject.SetActive(false);


        }
    }
}
