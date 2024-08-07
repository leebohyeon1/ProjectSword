using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionSkill : SwordSkill
{
    PlayerStat playerStat;

    [Header("스킬")]
    [SerializeField] private float[] power;
    [SerializeField] private float[] duration;
    private float[] skillTimer = new float[2];
    private bool[] skillActive = new bool[2];

    [Header("A")]
    [SerializeField] private float damageInterval = 1f;
    private float damageTimer;
    [SerializeField] private GameObject Panel;
    public GameObject panel { get { return Panel; } }
    private List<GameObject> ppp;

    //==================================================================================

    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();

        ppp = new List<GameObject>();

    }

    void Update()
    {
        /*if (skillActive[0])
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
                            enemyStat.TakeDamage((int)(power[0] + skillDamageUp));
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
                damageTimer = 0f;
            }
        }*/

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

    void SkillBOff()
    {
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            DandelionBullet bullets = bullet.GetComponent<DandelionBullet>();
            bullets.SetDamagebuff(1);
            bullets.SetDandelionSkillB( false);
        }
        skillActive[1] = false;
       
    }

    //==================================================================================

    public override void SkillA()
    {
        if (skillPoint == null)
        {
            Debug.LogError("SkillPoint가 설정되지 않았습니다. SkillA를 실행할 수 없습니다.");
            return;
        }

        GameObject panel = Instantiate(Panel, skillPoint.position, Quaternion.identity);
        panel.transform.localScale = ChangeSkill(0);

        DandelionField dandelionField = panel.GetComponent<DandelionField>();
        dandelionField.SetField(duration[0], damageInterval, (int)power[0]);

    }

    public override void SkillB()
    {

        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            DandelionBullet bullets = bullet.GetComponent<DandelionBullet>();
            bullets.SetDamagebuff(power[1]);
            bullets.SetDandelionSkillB(true);

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

    public override void SetSkillDamage(float Damage)
    {
        base.SetSkillDamage(Damage);
    }



}



