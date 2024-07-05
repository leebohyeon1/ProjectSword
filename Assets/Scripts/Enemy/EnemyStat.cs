using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    EnemyUI enemyUI;


    public int hp = 1;
    public float speed;
    public int damage;

    void Start()
    {
        enemyUI = GetComponent<EnemyUI>();
        enemyUI.UpdateHPText(hp);

        GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
    }

    void Update()
    {
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        enemyUI.UpdateHPText(hp);

        if (hp <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.skillCount++;
            GameManager.Instance.swapCount++;
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
