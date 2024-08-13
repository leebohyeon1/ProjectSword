using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenkaiFire : SwordFire
{
    [SerializeField] private TenkaiBullet bullet;
    [SerializeField] private Tenkai tenkai;

    private Vector3 Pos;

    //==================================================================================
    
    void Start()
    {
        Set();

        Pos = new Vector3(transform.position.x, transform.position.y + (tenkai.GetDetectRadius().y/2), 0);
    }

    //==================================================================================

    public void SetTenkai(TenkaiBullet tenkaiBullet, Tenkai tenkai)
    {
        bullet = tenkaiBullet;
        this.tenkai = tenkai;
    }

    //==================================================================================

    public override void Set()
    {
        base.Set();

    }

    public override void Fire()
    {
      
        Transform closestEnemy = null;

        float closestDistance = Mathf.Infinity;
        EnemyStat lowestHealthEnemy = null;

        Collider2D[] enemies  = Physics2D.OverlapBoxAll(Pos, tenkai.GetDetectRadius(),0,bullet.GetEnemyLayer());

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
                    if (currentEnemyStat.HP < lowestHealthEnemy.HP)
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

    public override void SetMagicSword(MagicSword sword)
    {
        base.SetMagicSword(sword);
    }

    //==================================================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Pos, tenkai.GetDetectRadius());
    }
}
