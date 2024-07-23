using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private EnemyUI enemyUI;

    public int hp = 1;
    public float speed;
    public int damage;

    [Header("������")]
    public float skillGage;
    public float swapGage;

    [Header("�ູ")]
    public float blessRate = 5f;
    public bool isBless;
    public float multiplyGage;

    private List<float> speedModifiers = new List<float>();

    public bool isIce;

    private void Awake()
    {
        isBless = Random.value * 100f < blessRate;
    }

    void Start()
    {
        enemyUI = GetComponent<EnemyUI>();
        enemyUI.UpdateHPText(hp);

        ApplySpeed();

        if (isBless )
        {
            GetComponent<SpriteRenderer>().color =  Color.white;
        }
    
    }

    public void TakeDamage(int damage, bool Count = true)
    {
        hp -= damage;
        enemyUI.UpdateHPText(hp);

        if (hp <= 0)
        {
            Destroy(gameObject);
            if (Count)
            {
                float finalSkillGage = isBless ? skillGage * multiplyGage : skillGage;
                float finalSwapGage = isBless ? swapGage * multiplyGage : swapGage;

                EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT, this, finalSkillGage);
                EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, finalSwapGage);
            }
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

     public void DecreaseSpeed(float rate)
    {
        // �ӵ� ������ �߰�
        speedModifiers.Add(rate);
        ApplySpeed();
    }

    public void IncreaseSpeed(float rate)
    {
        // �ӵ� ������ �߰� (���� ���� �ӵ� ���� ����Ʈ�� �߰�)
        speedModifiers.Add(-rate);
        ApplySpeed();
    }

    private void ApplySpeed()
    {
        // ��� �ӵ� �������� �����Ͽ� ���� �ӵ��� ���
        float totalRate = 0f;
        foreach (float modifier in speedModifiers)
        {
            totalRate += modifier;
        }

        float finalSpeed = speed * (1 - totalRate / 100f);
        GetComponent<Rigidbody2D>().velocity = Vector2.down * (finalSpeed + SpawnManager.Instance.plusAcceleration);
    }
}
