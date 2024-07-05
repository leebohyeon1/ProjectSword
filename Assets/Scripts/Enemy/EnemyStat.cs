using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    EnemyUI enemyUI;

    public int hp = 1;
    public float speed;
    public int damage;

    [Header("∞‘¿Ã¡ˆ")]
    public float skillGage;
    public float swapGage;

    void Start()
    {
        enemyUI = GetComponent<EnemyUI>();
        enemyUI.UpdateHPText(hp);

        GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        enemyUI.UpdateHPText(hp);

        if (hp <= 0)
        {
            Destroy(gameObject);
            EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT, this, skillGage);
            EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT,this,swapGage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
