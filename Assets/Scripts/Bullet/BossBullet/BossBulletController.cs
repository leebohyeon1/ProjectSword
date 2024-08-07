using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    private float damage = 0;

    private BulletType bulletType;

    //=============================================================================

    public void InitializeBullet(float _damage, BulletType type)
    {
        damage = _damage;
        bulletType = type;
    }

    //=============================================================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);       
    }
}
