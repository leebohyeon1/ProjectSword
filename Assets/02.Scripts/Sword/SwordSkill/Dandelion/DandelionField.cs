using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionField : MonoBehaviour
{
    [SerializeField] private float duration = 5f; // ������ ���� �ð�
    [SerializeField] private float damageInterval = 1f; // ���ظ� �ִ� ����
    [SerializeField] private int damageAmount = 1; // ���ط�
    private float damageTimer = 0f;

    [SerializeField] private LayerMask enemyLayer;

    //==================================================================================

    private void Start()
    {
        // ���� �ð��� ������ ������ �ı�
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
