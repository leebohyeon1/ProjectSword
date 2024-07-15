using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TenkaiBullet : BulletController
{
    private int diffusionCount_;
    public int diffusionCount = 1;
    public float detectionRadius;
    public LayerMask enemyLayer;

    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        diffusionCount_ = diffusionCount;
    }
    // Update is called once per frame
    void Update()
    {

    }

    void FindNextTarget(Collider2D collider)
    {
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
                    if (currentEnemyStat.hp < lowestHealthEnemy.hp)
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
            transform.position = closestEnemy.position;
        }
        //target = closestEnemy.gameObject;
        //target.GetComponent<EnemyStat>().TakeDamage((int)TotalDamage);

     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        TotalDamage = damage * damageRate;
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyStat>().TakeDamage((int)TotalDamage);
            if (diffusionCount_ > 0)
            {
                FindNextTarget(collision);
            }
            else
            {
                gameObject.SetActive(false);
                diffusionCount_ = 0;
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
