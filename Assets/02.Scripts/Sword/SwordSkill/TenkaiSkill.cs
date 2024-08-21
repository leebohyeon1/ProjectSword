using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenkaiSkill : SwordSkill
{
    PlayerStat playerStat;

    [Header("��ų")]
    [SerializeField] private float[] aPower;
    [SerializeField] private float[] bPower;
    [SerializeField] private float[] duration;
    private float[] skillTimer = new float[2];
    private bool[] skillActive = new bool[2];

    [Header("A")]
    [SerializeField] private float slowRate;
    [SerializeField] private float damageInterval = 1f;
    private float damageTimer;
    [SerializeField] private GameObject Panel;
    private List<GameObject> enemyList= new List<GameObject>();
    private List<GameObject> ppp = new List<GameObject>();

    //==================================================================================

    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();

    }

    void Update()
    {
        if (skillActive[0])
        {
            skillTimer[0] += Time.deltaTime;
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
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
                            enemyStat.TakeDamage((int)aPower[skillLevel]);
                     
                            enemyList.Add(Enemy);
                            if (enemyStat != null)
                            {
                                enemyStat.DecreaseSpeed(slowRate);
                            }
                        }
                
                    }
                }
                damageTimer = 0f;
            }

            if (skillTimer[0] >= duration[0])
            {
                skillActive[0] = false;
                RestoreAllEnemySpeeds();
                Destroy(ppp[0]);
                ppp.Clear();
            }
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

    //==================================================================================

    public void SkillBOff()
    {
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            TenkaiBullet bullets = bullet.GetComponent<TenkaiBullet>();
            bullets.SetDiffusionCount(1);
        }
        skillTimer[1] = 0f;
        skillActive[1] = false;
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

    //==================================================================================

    public override void SkillA()
    {

        skillTimer[0] = 0f;
        damageTimer = 0f;
        skillActive[0] = true;

        ppp.Clear();
        GameObject panel = Instantiate(Panel, skillPoint.position, Quaternion.identity);
        panel.transform.localScale = ChangeSkill(0);
        ppp.Add(panel);
    }
    public override void SkillB()
    {
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            TenkaiBullet bullets = bullet.GetComponent<TenkaiBullet>();
            bullets.IncreaseDiffusionCount((int)bPower[skillLevel]);
        }
        skillTimer[1] = 0f;
        skillActive[1] = true;
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
    public override Vector2[] GetSkillSize()
    {
        return base.GetSkillSize();
    }

    public override void SetSkillLevel(int Damage)
    {
        base.SetSkillLevel(Damage);
    }


}
