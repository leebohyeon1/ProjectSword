using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionBullet : BulletController
{
    public bool isDandelion = false;

    void Start()
    {
        damageRate = 1f;
    }


    public override void SetDamagebuff(float rate)
    {
        base.SetDamagebuff(rate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TotalDamage = damage * damageRate;
        if (isDandelion)
        {
            StartCoroutine(SkillB(collision));
        }
        else
        {        
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyStat>().TakeDamage((int)TotalDamage);
                gameObject.SetActive(false);
            }
        }
     
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
