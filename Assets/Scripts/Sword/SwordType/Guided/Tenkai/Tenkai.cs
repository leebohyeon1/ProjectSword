using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tenkai : MagicSword
{
    public Vector3 detectRadius;

    float timer = 0f;
    Vector3 Pos;
    PlayerStat playerStat;
    // Start is called before the first frame update
    void Start()
    {
        SetTrans();

        SetFire();

        InitializePool();

        Pos = new Vector3(transform.position.x, transform.position.y + (detectRadius.y / 2), 0);
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
        Attack();
    }

    public override void SetTrans()
    {
        base.SetTrans();
        playerStat = FindFirstObjectByType<PlayerStat>();
    }

    public override void SetFire()
    {

        firePos.AddComponent<TenkaiFire>();
        //firePos.GetComponent<TenkaiFire>().enabled = false;
        firePos.GetComponent<TenkaiFire>().magicSword = this;

        firePos.GetComponent<TenkaiFire>().bullet = bulletPrefab.GetComponent<TenkaiBullet>();
        firePos.GetComponent<TenkaiFire>().tenkai = this;
    }
    protected override void Follow()
    {
        base.Follow();
    }

    protected override void InitializePool()
    {
        base.InitializePool();
    }

    public override void SetBullet()
    {
        firePos.GetComponent<TenkaiFire>().enabled = !firePos.GetComponent<TenkaiFire>().enabled;
    }

    protected override GameObject GetBullet()
    {
        return base.GetBullet();
    }

    public void Attack()
    {
        timer += Time.deltaTime;
        if (timer > attackSpeed)
        {
            Fire();
            timer = 0f;
        }
    }

    public override void Fire()
    {
        Transform closestEnemy = null;

        float closestDistance = Mathf.Infinity;
        EnemyStat lowestHealthEnemy = null;

        Collider2D[] enemies = Physics2D.OverlapBoxAll(Pos, detectRadius, 0, bulletPrefab.GetComponent<TenkaiBullet>().enemyLayer);

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
            GameObject bullet = GetBullet();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.GetComponent<BulletController>().isSubBullet = true;
            bullet.SetActive(true);

            //Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bullet.transform.position = closestEnemy.position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Pos, detectRadius);
    }
}
