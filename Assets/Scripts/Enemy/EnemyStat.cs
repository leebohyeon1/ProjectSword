using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private EnemyUI enemyUI;

    public int hp = 1;
    public float speed;
    public int damage;

    [Header("게이지")]
    public float skillGage;
    public float swapGage;

    [Header("축복")]
    public float blessRate = 5f;
    public bool isBless;
    public float multiplyGage;

    private void Awake()
    {
        isBless = Random.value * 100f < blessRate;
    }

    void Start()
    {
        enemyUI = GetComponent<EnemyUI>();
        enemyUI.UpdateHPText(hp);

        GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;

        if (isBless )
        {
            GetComponent<SpriteRenderer>().color =  Color.white;
        }
    
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        enemyUI.UpdateHPText(hp);

        if (hp <= 0)
        {
            Destroy(gameObject);
            float finalSkillGage = isBless ? skillGage * multiplyGage : skillGage;
            float finalSwapGage = isBless ? swapGage * multiplyGage : swapGage;

            EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT, this, finalSkillGage);
            EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, finalSwapGage);
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
