using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    private EnemyUI enemyUI;

    [SerializeField] protected int hp = 1;
    [SerializeField] protected float speed;
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
    protected bool canBite = false;
    protected bool isFIreStigma = false;
    protected bool isIceStigma;
  
    public bool IceStigma
    {
        get { return isIceStigma; }
        private set
        {
            isIceStigma = value;
            StartCoroutine(IceStigmaOff());
        }
    }

    protected bool canStigma = true;

    protected float IceTime = 5f;
    protected float IceReturnTime = 15f;

    protected int biteDamage = 0;
    protected float biteTimer = 0f;

    protected float defaultSkillGage = 0;
    //==================================================================================

    private void Awake()
    {
        isBless = Random.value * 100f < blessRate;
    }

    void Start()
    {
        enemyUI = GetComponent<EnemyUI>();
        enemyUI.UpdateHPText(hp);

        defaultSkillGage = skillGage;

        ApplySpeed();

        if (isBless )
        {
            GetComponent<SpriteRenderer>().color =  Color.white;
        }
    
    }

    private void Update()
    {
        if(biteDamage > 0)
        {
            biteTimer += Time.deltaTime;
            if (biteTimer > 3)
            {
                StartCoroutine(Bite());
               
            }
        }    
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    //==================================================================================

    public virtual void TakeDamage(int damage, bool Count = true)
    {
        hp -= damage;
        enemyUI.UpdateHPText(hp);
        
        if(canBite)
        {
            biteDamage += damage;
        }

        if (hp <= 0)
        {
            Destroy(gameObject);
            if (Count)
            {
                float finalSkillGage = isBless ? skillGage * multiplyGage : skillGage;
                float finalSwapGage = isBless ? swapGage * multiplyGage : swapGage;

                EventManager.Instance.PostNotification(EVENT_TYPE.FLOWER, this);
                EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT, this, finalSkillGage);
                EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, finalSwapGage);
            }
        }
    }

    public virtual void SetIsIce(bool boolean)
    {
        isIce = boolean;
    }

    public virtual void SetIsMolar(bool boolean)
    {
        isMolar = boolean;
    }

    public virtual void SetIsFIreStigma(bool boolean)
    {
        isFIreStigma = boolean;
    }

    public virtual void SetIsIceStigma(bool boolean, float time = 5f)
    {
        if(canStigma)
        {
            IceStigma = boolean;
            IceTime = time;
            canStigma = false;
        }
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

    public virtual bool GetFireStigma() => isFIreStigma;
    public virtual bool GetIceStigma() => IceStigma;

    public virtual void SetBite() { canBite = true; }

    public virtual int HP => hp;   

    protected virtual void ApplySpeed()
    {
        // ��� �ӵ� �������� �����Ͽ� ���� �ӵ��� ���
        float totalRate = 0f;
        foreach (float modifier in speedModifiers)
        {
            totalRate += modifier;
        }

        float finalSpeed = speed * (1 - totalRate / 100f);
        GetComponent<Rigidbody2D>().velocity = Vector2.down * (finalSpeed + SpawnManager.Instance.PlusAcceleration());
    }
    protected virtual void CalSpeed()
    {
        float totalRate = 0f;
        foreach (float modifier in speedModifiers)
        {
            totalRate += modifier;
        }

        if(totalRate >= 75f)
        {
            IceTime += 5;
        }
        
    }
    protected virtual IEnumerator Bite()
    {
        canBite = false; 
        TakeDamage(biteDamage / 2);
        biteDamage = 0;
        biteTimer = 0;
        yield return new WaitForSeconds(10f);
        canBite = true;
    }

    public virtual IEnumerator Ten1()
    {
        for (int i = 0; i < 2; i++)
        {
            TakeDamage(1);
            yield return new WaitForSeconds(1f);
        }
    }

    public virtual IEnumerator IceStigmaOff()
    {
        yield return new WaitForSeconds(IceTime);
        IceStigma = false;

        yield return new WaitForSeconds(IceReturnTime);
        canStigma = true;
    }
    //==================================================================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStat>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if(collision.GetComponent<DandelionBullet>() && collision.GetComponent<DandelionBullet>().GetSubBullet())
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.DAN3, this, transform.position);

        }

        if (collision.GetComponent<TenkaiBullet>() && collision.GetComponent<TenkaiBullet>().GetDiffusion()){
            skillGage += collision.GetComponent<TenkaiBullet>().GetSkillGage();
        }
        else
        {
            skillGage = defaultSkillGage;
        }
    }
}
