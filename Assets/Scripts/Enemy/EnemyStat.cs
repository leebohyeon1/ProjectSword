using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private EnemyUI enemyUI;

    public int hp = 1;
    [SerializeField]protected float speed;
    [SerializeField] protected int damage;

    [Header("������")]
    [SerializeField] private float skillGage;
    [SerializeField] protected float swapGage;

    [Header("�ູ")]
    private float blessRate = 5f;
    private  bool isBless;
    private float multiplyGage = 2f;

    protected List<float> speedModifiers = new List<float>();

    [Header("����")]
    protected bool isIce = false;
    protected bool isMolar = false;

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

    public virtual void TakeDamage(int damage, bool Count = true)
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

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public virtual void SetIsIce(bool boolean)
    {
        isIce = boolean;
    }

    public virtual void SetIsMolar(bool boolean)
    {
        isMolar = boolean;
    }

    public virtual void DecreaseSpeed(float rate)
    {
        // �ӵ� ������ �߰�
        speedModifiers.Add(rate);
        ApplySpeed();
    }

    public virtual void IncreaseSpeed(float rate)
    {
        // �ӵ� ������ �߰� (���� ���� �ӵ� ���� ����Ʈ�� �߰�)
        speedModifiers.Add(-rate);
        ApplySpeed();
    }

    public virtual bool GetIsIce() => isIce;

    public virtual bool GetIsMolar() => isMolar;

    protected virtual void ApplySpeed()
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if(collision.GetComponent<DandelionBullet>() && collision.GetComponent<DandelionBullet>().isSubBullet)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.DAN3, this, transform.position);

        }
    }
}
