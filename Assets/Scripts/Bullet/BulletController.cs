using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletController : MonoBehaviour
{
    protected PlayerStat playerStat;
    public enum Type
    {
        None,
        Ice,
        Fire
    }

    public int damage;
    public float damageRate;
    public float TotalDamage;

    public Type bulletType;

    public bool isSkillBullet;
    public bool isSubBullet;
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

        TotalDamage = damage * damageRate;
        if (collision.CompareTag("Enemy"))
        {
            bool canCount = true;
            if (playerStat.canDrain && !isSkillBullet && !isSubBullet)
            {
                playerStat.Drain((int)TotalDamage);
                canCount = false;   
            }

            collision.GetComponent<EnemyStat>().TakeDamage((int)TotalDamage, canCount);
            gameObject.SetActive(false);


        }
    }
}
