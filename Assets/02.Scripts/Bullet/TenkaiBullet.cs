using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TenkaiBullet : BulletController
{
    private int diffusionCount_;
    [SerializeField] private bool isDiffusionBullet = false;
    [SerializeField] private int diffusionCount = 1;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private GameObject target;

    private bool buff2 = false;
    private float buff2Slow = 0f;

    private bool buff4 = false; 
    private int buff4Damage;

    private float skillGageUp = 0f;

    public GameObject lightningPrefab; // 라이트닝 오브젝트 프리팹

    //=============================================================================

    void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();

        damageRate = 1f;


    }

    private void OnEnable()
    {
        diffusionCount_ = diffusionCount;
        Invoke("NoCollision", 1f);
        //SpawnLightning(playerStat.transform.position, transform.position);
    }

    //=============================================================================

    void FindNextTarget(Collider2D collider)
    {
        if(GameManager.Instance.GetTenkai(3) && !isSubBullet)
        {
            isDiffusionBullet = true;
        }
            
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        EnemyStat lowestHealthEnemy = null;


        foreach (Collider2D enemyCollider in enemies)
        {
            if ( enemyCollider == collider)
            {
                continue;
            }
            Transform enemyTransform = enemyCollider.transform;
            float distance = Vector3.Distance(transform.position, enemyTransform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyTransform;
                lowestHealthEnemy = enemyCollider.GetComponent<EnemyStat>();
            }
            else if (distance == closestDistance)
            {
                EnemyStat currentEnemyStat = enemyCollider.GetComponent<EnemyStat>();
                if (currentEnemyStat != null && lowestHealthEnemy != null)
                {
                    if (currentEnemyStat.HP < lowestHealthEnemy.HP)
                    {
                        closestEnemy = enemyTransform;
                        lowestHealthEnemy = currentEnemyStat;
                    }
                }
            }
        }

        if (closestEnemy == null)
        {
            gameObject.SetActive(false);            
        }
        else
        {
            target = closestEnemy.gameObject;
            transform.position = closestEnemy.position;
        }     
    }

    public void SetBuff2(bool boolean, float slow)
    {
        buff2 = boolean;
        buff2Slow = slow;   
    }

    public void SetBuff3(int count)
    {
        diffusionCount = count;
    }

    public void SetBuff4(int Damage)
    {
        buff4Damage = Damage;
    }

    public void SetDiffusionCount(int count) { diffusionCount = count; }
    public void SetSkillGage(float Gage)
    {
        skillGageUp = Gage; 
    }
    public void IncreaseDiffusionCount(int count)
    {
        diffusionCount += count;
    }

    public override void SetTwinflip3(bool twinflip3)
    {
        base.SetTwinflip3(twinflip3);
    }

    public LayerMask GetEnemyLayer() => enemyLayer;
    public float GetSkillGage() => skillGageUp;

    private void NoCollision()
    {
        gameObject.SetActive(false);
    }

    void SpawnLightning(Vector3 startPosition, Vector3 targetPosition)
    {
        // 라이트닝 프리팹 생성
        GameObject lightning = Instantiate(lightningPrefab, startPosition, Quaternion.identity);

        // 라인 렌더러 설정
        LineRenderer lineRenderer = lightning.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, targetPosition);

        Destroy(lightning, 0.1f);
        // 애니메이션 또는 추가 효과를 위한 다른 설정 가능
    }

    //=============================================================================

    public override void SetBulletType(BulletType bulletType)
    {
        base.SetBulletType(bulletType);
    }
    public override void SetDamagebuff(float rate)
    {
        base.SetDamagebuff(rate);
    }
    public override void SetDamage(int Damage, float Cri = 0, bool f = false)
    {
        base.SetDamage(Damage,Cri, f);
    }
    public override void SetSlowRate(float slowRate)
    {
        base.SetSlowRate(slowRate);
    }
    public override void SetIce(bool ice)
    {
        base.SetIce(ice);
    }
    public override void SetSubBullet()
    {
        base.SetSubBullet();
    }
    public override BulletType GetBulletType()
    {
        return base.GetBulletType();
    }
    public override void IncreaseDamage(float damage)
    {
        base.IncreaseDamage(damage);
    }

    public override bool GetSubBullet()
    {
        return base.GetSubBullet();
    }

    public override bool GetIce()
    {
        return base.GetIce();
    }
    public override void SetTwinSwap(bool bo)
    {
        base.SetTwinSwap(bo);
    }
    public bool GetDiffusion()
    {
        return isDiffusionBullet;
    }

    protected override int CalculateTwinDamage(float distance)
    {
        return base.CalculateTwinDamage(distance);
    }

    //=============================================================================

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {

            EnemyStat enemyStat = collision.GetComponent<EnemyStat>();
            
            if (!isCritical && isSubBullet && isTwinSwap)
            {
                if (Random.value < 0.2f)
                {
                    isCritical = true;
                }
            }

            if (enemyStat.GetIsIce())
            {
                TotalDamage = (damage + damageUp) * damageRate;
            }
            else
            {
                TotalDamage = damage * damageRate;
            }

            if (diffusionCount_ == 0 && buff4)
            {
                if (enemyStat.GetIsIce())
                {
                    TotalDamage = (buff4Damage + damage + damageUp) * damageRate;
                }
             
                if (isTwinflip3)
                {
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(collision.transform.position);

                    // 화면의 절반 이하에 있는지 확인
                    if (screenPosition.y < Screen.height / 2)
                    {
                        float dis = Vector2.Distance(playerStat.transform.position, collision.transform.position);
     
                        TotalDamage = CalculateTwinDamage(dis);
                        if (isCritical)
                        {
                            TotalDamage *= (1 + criticalDamage / 100);
                        }
                        enemyStat.TakeDamage((int)TotalDamage);
                    }
                }
                else
                {
                    if (isCritical)
                    {
                        TotalDamage *= (1 + criticalDamage / 100);
                    }

                    enemyStat.TakeDamage((int)TotalDamage);
                }
            }
            else
            {
                if (isTwinflip3)
                {
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(collision.transform.position);

                    // 화면의 절반 이하에 있는지 확인
                    if (screenPosition.y < Screen.height / 2)
                    {
                        float dis = Vector2.Distance(playerStat.transform.position, collision.transform.position);

                        TotalDamage = CalculateTwinDamage(dis);
                        if (isCritical)
                        {
                            TotalDamage *= (1 + criticalDamage / 100);
                        }
                        enemyStat.TakeDamage((int)TotalDamage);
                    }
                }
                else
                {
                    if (isCritical)
                    {
                        TotalDamage *= (1 + criticalDamage / 100);
                    }
                    enemyStat.TakeDamage((int)TotalDamage);
                }
            }

            if (diffusionCount_ > 0)
            {
                FindNextTarget(collision);
                if(target != null)
                {
                    SpawnLightning(collision.transform.position, target.transform.position);
                    target = null;
                }
            }
            else
            {
                gameObject.SetActive(false);
                diffusionCount_ = 0;
            }

            if (buff2)
            {
                enemyStat.DecreaseSpeed(buff2Slow);
            }

            if (isIce && !enemyStat.GetIsMolar() && !isSkillBullet)
            {
                enemyStat.SetIsIce(true);
                enemyStat.DecreaseSpeed(slowRate);
            }

            if (playerStat.canDrain && !isSkillBullet && !isSubBullet)
            {
                playerStat.Drain((int)TotalDamage);
            }

            if(GameManager.Instance.GetTenkai(1) && !isSubBullet)
            {
                StartCoroutine(enemyStat.Ten1());
            }
            
            if (GameManager.Instance.GetTenkai(2) && !isSubBullet && bulletType == BulletType.Thunder)
            {
                enemyStat.TakeDamage(1);
            }

            diffusionCount_--;



        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
