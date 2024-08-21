using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class SwordSkill : MonoBehaviour
{
    [SerializeField] protected Vector2[] skillSize;
    [SerializeField] protected Transform skillPoint; // Inspector에서 설정

    [SerializeField] protected LayerMask enemyLayer;

    [SerializeField] protected int skillLevel;

    //================================================================================================

    public virtual void Skill(int Index) { }

    public virtual Vector2 ChangeSkill(int Index) { return Vector2.zero; }

    public virtual void SetTrans(Transform trans) { }

    public virtual void SkillA() { }
    public virtual void SkillB() { }

    public virtual Vector2[] GetSkillSize() { return skillSize; }

    public virtual void SetSkillLevel(int level)
    {
        skillLevel += level;
        if (skillLevel > 15)
        {
            skillLevel = 15;
        }
        else
        {
            SkillBuff();
        }
        
    }

    public virtual void SkillBuff()
    {

    }
}
