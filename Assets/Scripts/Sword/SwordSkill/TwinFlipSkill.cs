using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinFlipSkill : SwordSkill
{
    PlayerStat playerStat;

    [Header("스킬")]
    public float[] power;
    public float[] duration;
    private float[] skillTimer = new float[2];
    private bool[] skillActive = new bool[2];

    [Header("A")]
    public float slowRate;
    public float damageUpRate;
    public float slowCheckTimer;

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
                            enemyStat.TakeDamage((int)power[0]);

                            enemyList.Add(Enemy);
                            if (enemyStat != null)
                            {
                                enemyStat.SetSpeed(slowRate);
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
    }
    public override void SkillB()
    {
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
    private void RestoreAllEnemySpeeds()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy != null)
            {
                EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
                if (enemyStat != null)
                {
                    enemyStat.SetSpeed(0f);
                }
            }
        }
        enemyList.Clear();
    }


}




