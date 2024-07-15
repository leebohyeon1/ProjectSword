using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage;
    public float damageRate;
    public float TotalDamage;

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
            collision.GetComponent<EnemyStat>().TakeDamage((int)TotalDamage);
            gameObject.SetActive(false);
        }
    }
}
