using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinFlipSkill : SwordSkill
{
    PlayerStat playerStat;

    [Header("½ºÅ³")]
    [SerializeField] private float[] duration;
    private float[] skillTimer = new float[2];
    private bool[] skillActive = new bool[2];
    [SerializeField] private bool isBuff = false;

    [Header("A")]
    [SerializeField] private float slowRate;
    private float slowCheckTimer;

    [Header("B")]
    [SerializeField] private float bulletSpeedSlow;

    [Space(10f)]
    [SerializeField] private GameObject Panel;
    private List<GameObject> enemyList = new List<GameObject>();
    private List<GameObject> ppp = new List<GameObject>();
    private int index;
    private BulletType bull;
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

    //==================================================================================

    public void SetBuff(bool boolean) { isBuff = boolean; }

    public void SkillBOff()
    {

        playerStat.upAttackSpeed[index] += bulletSpeedSlow;
        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            if (bullet.GetComponent<TwinFlipBullet>() != null)
            {
                TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
                bullets.SetBSkill(false);
            }
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
        if (skillPoint == null)
        {
            return;
        }

        skillTimer[0] = 0f;
        skillActive[0] = true;

        ppp.Clear();
        GameObject panel = Instantiate(Panel, skillPoint.position, Quaternion.identity);
        panel.transform.localScale = ChangeSkill(0);
        ppp.Add(panel);

        if (bull != BulletType.Ice)
        GetComponent<Twinflip>().ChangeIceBullet();


        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
            bullets.SetBulletType( BulletType.Ice);
            bull = BulletType.Ice;
        }
 
    }
    public override void SkillB()
    {
        index = playerStat.GetWeaponIndex();
        playerStat.upAttackSpeed[index] -= bulletSpeedSlow;

        if (bull != BulletType.Fire)
        GetComponent<Twinflip>().ChangeFireBullet();


        foreach (GameObject bullet in playerStat.bulletPool_)
        {
            TwinFlipBullet bullets = bullet.GetComponent<TwinFlipBullet>();
            bullets.SetBSkill(true);
            bullets.SetBulletType(BulletType.Fire);
            bull = BulletType.Fire;
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

    //==================================================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0.5f);
        TwinFlipBullet bullet = playerStat.bulletPool_[0].GetComponent<TwinFlipBullet>();
        for (int i = 0; i < bullet.GetDistance().Length; i++)
        {
            Gizmos.DrawLine(new Vector3(100, playerStat.transform.position.y + bullet.GetDistance()[i], 0), new Vector3(-100, transform.position.y + bullet.GetDistance()[i], 0));
        }
    }
}




