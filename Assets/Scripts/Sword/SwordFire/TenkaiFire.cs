using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenkaiFire : SwordFire
{
    public TenkaiBullet bullet;
    public Tenkai tenkai;

    Vector3 Pos;
    void Start()
    {
        Set();

        Pos = new Vector3(transform.position.x, transform.position.y + (tenkai.detectRadius.y/2), 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Set()
    {
        base.Set();

    }
    public override void Fire()
    {
      
        Transform closestEnemy = null;

        float closestDistance = Mathf.Infinity;
        EnemyStat lowestHealthEnemy = null;

        Collider2D[] enemies  = Physics2D.OverlapBoxAll(Pos, tenkai.detectRadius,0,bullet.enemyLayer);

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
        EventManager.Instance.PostNotification(EVENT_TYPE.FIRE, this);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Pos, tenkai.detectRadius);
    }
}
