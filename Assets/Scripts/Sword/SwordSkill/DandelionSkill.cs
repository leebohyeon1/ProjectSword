using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionSkill : SwordSkill
{
    PlayerStat playerStat;

    [Header("스킬")]
    public float[] power;
    public float[] duration;
    private float[] skillTimer = new float[2];
    private bool[] skillActive = new bool[2];

    [Header("A")]
    public float damageInterval = 1f;
    private float damageTimer;
    public GameObject Panel;
    private List<GameObject> ppp;



    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();

        ppp = new List<GameObject>();

    }

    void Update()
    {
        if (skillActive[0])
        {
            skillTimer[0] += Time.deltaTime;
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageInterval)
            {
                Vector2 boxSize = ChangeSkill(0);
                Collider2D[] enemies = Physics2D.OverlapBoxAll(skillPoint.position, boxSize, 0, enemyLayer);

                foreach (Collider2D enemy in enemies)
                {
                    if (enemy != null)
                    {
                        EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
                        if (enemyStat != null)
                        {
                            enemyStat.TakeDamage((int)power[0]);
                        }
                    }
                }
                damageTimer = 0f;
            }

            if (skillTimer[0] >= duration[0])
            {
                skillActive[0] = false;
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

    public override void SkillA()
    {
        if (skillPoint == null)
        {
            Debug.LogError("SkillPoint가 설정되지 않았습니다. SkillA를 실행할 수 없습니다.");
            return;
        }

        skillTimer[0] = 0f;
        damageTimer = 0f;
        skillActive[0] = true;

        ppp.Clear();
        GameObject panel =Instantiate(Panel, skillPoint.position, Quaternion.identity);
        panel.transform.localScale = ChangeSkill(0);
        ppp.Add(panel);
    }
    public override void SkillB()
    {

        foreach(GameObject bullet in playerStat.bulletPool_)
        {
            DandelionBullet bullets = bullet.GetComponent<DandelionBullet>();
            bullets.SetDamagebuff(power[1]);
            bullets.isDandelion = true;

        }
        skillTimer[1] = 0f;
        skillActive[1] = true;  
    }

    void SkillBOff()
    {
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            DandelionBullet bullets = bullet.GetComponent<DandelionBullet>();
            bullets.SetDamagebuff(1);
            bullets.isDandelion = false;
        }
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

}



