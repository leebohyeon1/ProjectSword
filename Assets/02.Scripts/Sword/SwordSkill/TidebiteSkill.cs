using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidebiteSkill : SwordSkill
{
    PlayerStat playerStat;

    [Header("½ºÅ³")]
    [SerializeField] private float[] power;

    //==================================================================================

    void Start()
    {
        playerStat = FindObjectOfType<PlayerStat>();
    }

    //==================================================================================

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

    public override void SkillA()
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
    }

    public override void SkillB()
    {
        GameObject Bullet = GetComponent<MagicSword>().GetBulletPrefab();

        GameObject BigBullet =  Instantiate(Bullet,new Vector2(skillPoint.position.x,playerStat.transform.position.y - 5f),Quaternion.identity);
        BigBullet.transform.localScale *= 2;
        BigBullet.GetComponent<BulletController>().SetDamage((int)(power[1] + skillDamageUp));
        //BigBullet.GetComponent<BulletController>().damageRate = 1f;
        BigBullet.GetComponent<BulletController>().SetSubBullet();
       BigBullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * playerStat.bulletSpeed* 1.2f;
       
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
