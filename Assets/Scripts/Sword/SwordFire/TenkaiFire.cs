using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenkaiFire : SwordFire
{
    TenkaiBullet bullet;

    Tenkai tenkai;
    // Start is called before the first frame update
    void Start()
    {
        bullet = magicSword.bulletPrefab.GetComponent<TenkaiBullet>();
        tenkai = magicSword.GetComponent<Tenkai>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Fire()
    {
      
        Transform closestEnemy = null;
        while (closestEnemy == null)
        {
            float closestDistance = Mathf.Infinity;
            EnemyStat lowestHealthEnemy = null;

            Collider2D[] enemies  = Physics2D.OverlapBoxAll(transform.position,tenkai.detectRadius,bullet.enemyLayer);

            foreach (Collider2D enemyCollider in enemies)
            {

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

            if (closestEnemy != null)
            {
                GameObject bullet = playerStat.GetBullet();
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.SetActive(true);

                //Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bullet.transform.position = closestEnemy.position;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, tenkai.detectRadius);
    }
}
