using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionField : MonoBehaviour
{
    public float duration = 5f; // ������ ���� �ð�
    public float damageInterval = 1f; // ���ظ� �ִ� ����
    public int damageAmount = 1; // ���ط�
    private float damageTimer = 0f;

    public LayerMask enemyLayer;

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
}
