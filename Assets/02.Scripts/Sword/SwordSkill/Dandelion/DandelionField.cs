using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionField : MonoBehaviour
{
    [SerializeField] private float duration = 5f; // 장판의 지속 시간
    [SerializeField] private float damageInterval = 1f; // 피해를 주는 간격
    [SerializeField] private int damageAmount = 1; // 피해량
    private float damageTimer = 0f;

    [SerializeField] private LayerMask enemyLayer;

    //==================================================================================

    private void Start()
    {
        // 일정 시간이 지나면 장판을 파괴
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        damageTimer += Time.deltaTime;
        if (damageTimer >= damageInterval)
        {
            damageTimer = 0f;
            ApplyDamage();
        }
    }

    //==================================================================================

    private void ApplyDamage()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, transform.localScale,0, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyStat enemyStat = hit.GetComponent<EnemyStat>();
                if (enemyStat != null)
                {
                    enemyStat.TakeDamage(damageAmount);
                }
            }
        }
    }

    public void SetField(float Duration, float Interval, int Damage)
    {
        duration = Duration;
        damageInterval = Interval;
        damageAmount = Damage;
    }
}
