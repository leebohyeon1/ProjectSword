using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class SwordSkill : MonoBehaviour
{
    public Vector2[] skillSize;
    public Transform skillPoint; // Inspector에서 설정

    public LayerMask enemyLayer;

    public virtual void Skill(int Index) { }

    public virtual Vector2 ChangeSkill(int Index) { return Vector2.zero; }

    public virtual void SetTrans(Transform trans) { }

    public virtual void SkillA() { }
    public virtual void SkillB() { }
}
