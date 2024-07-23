using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinFlipSkill : SwordSkill
{
    PlayerStat playerStat;

    [Header("스킬")]
    public float[] duration;
    private float[] skillTimer = new float[2];
    private bool[] skillActive = new bool[2];

    [Header("A")]
    public float slowRate;
    private float slowCheckTimer;

    [Header("B")]
    public float bulletSpeedSlow;

    [Space(10f)]
    public GameObject Panel;
    private List<GameObject> enemyList = new List<GameObject>();
    private List<GameObject> ppp = new List<GameObject>();
    // Start is called before the first frame update

    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (skillActive[0])
        {
            skillTimer[0] += Time.deltaTime;
            slowCheckTimer += Time.deltaTime;

            if (slowCheckTimer > 0.2f)
            {
                RestoreAllEnemySpeeds();
                Vector2 boxSize = ChangeSkill(0);
                Collider2D[] enemies = Physics2D.OverlapBoxAll(skillPoint.position, boxSize, 0, enemyLayer);

                foreach (Collider2D enemy in enemies)
                {
                    if (enemy != null)
                    {
                        GameObject Enemy = enemy.gameObject;
                        if (Enemy != null)
                        {
                            EnemyStat enemyStat = Enemy.GetComponent<EnemyStat>();

                            enemyList.Add(Enemy);
                            if (enemyStat != null)
                            {
                                enemyStat.DecreaseSpeed(slowRate);
                            }
                        }

                    }

                }
                slowCheckTimer = 0f;
            }

        }

        if (skillTimer[0] >= duration[0])
        {
            skillActive[0] = false;
            RestoreAllEnemySpeeds();
            Destroy(ppp[0]);
        }

        if (skillActive[1])
        {
            skillTimer[1] += Time.deltaTime;


            if (skillTimer[1] >= duration[1])
            {
                SkillBOff();

            }
        }
    }

    public override void SkillA()
    {
        if (skillPoint == null)
        {
            Debug.LogError("SkillPoint가 설정되지 않았습니다. SkillA를 실행할 수 없습니다.");
            return;
        }

        skillTimer[0] = 0f;
        skillActive[0] = true;

        ppp.Clear();
        GameObject panel = Instantiate(Panel, skillPoint.position, Quaternion.identity);
        panel.transform.localScale = ChangeSkill(0);
        ppp.Add(panel);

        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
            bullets.bulletType = BulletType.Ice;
        }

    }
    public override void SkillB()
    {
        playerStat.bulletSpeed -= bulletSpeedSlow;
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
            bullets.isBSkill = true;
            bullets.bulletType = BulletType.Fire;
        }
        skillTimer[1] = 0f;
        skillActive[1] = true;
    }

    public void SkillBOff()
    {
        
        playerStat.bulletSpeed += bulletSpeedSlow;
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            if (bullet.GetComponent<TwinFlipBullet>() != null)
            {
                TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
                bullets.isBSkill = false;
            }
        }
        skillTimer[1] = 0f;
        skillActive[1] = false;
    }

    public override void Skill(int index)
    {
        if (index == 0)
        {
            SkillA();
        }
        else
        {
            SkillB();
        }
    }

    public override Vector2 ChangeSkill(int index)
    {
        return skillSize[index];
    }
    public override void SetTrans(Transform trans)
    {
        skillPoint = trans;
    }
    private void RestoreAllEnemySpeeds()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy != null)
            {
                EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
                if (enemyStat != null)
                {
                    enemyStat.IncreaseSpeed(slowRate);
                }
            }
        }
        enemyList.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0.5f);
        TwinFlipBullet bullet = playerStat.bulletPool_[0].GetComponent<TwinFlipBullet>();
        for (int i = 0; i < bullet.distance.Length; i++)
        {
            Gizmos.DrawLine(new Vector3(100, playerStat.transform.position.y + bullet.distance[i], 0), new Vector3(-100, transform.position.y + bullet.distance[i], 0));
        }
    }
}




