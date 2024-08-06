using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private EnemyUI enemyUI;

    public int hp = 1;
    [SerializeField]protected float speed;
    [SerializeField] protected int damage;

    [Header("게이지")]
    [SerializeField] private float skillGage;
    [SerializeField] protected float swapGage;

    [Header("축복")]
    private float blessRate = 5f;
    private  bool isBless;
    private float multiplyGage = 2f;

    protected List<float> speedModifiers = new List<float>();

    [Header("상태")]
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
        // 속도 감소율 추가
        speedModifiers.Add(rate);
        ApplySpeed();
    }

    public virtual void IncreaseSpeed(float rate)
    {
        // 속도 증가율 추가 (음수 값을 속도 감소 리스트에 추가)
        speedModifiers.Add(-rate);
        ApplySpeed();
    }

    public virtual bool GetIsIce() => isIce;

    public virtual bool GetIsMolar() => isMolar;

    protected virtual void ApplySpeed()
    {
        // 모든 속도 감소율을 적용하여 최종 속도를 계산
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
