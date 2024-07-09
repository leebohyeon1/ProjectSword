using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : MonoBehaviour
{
    public Vector2[] skillSize;

    public virtual void Skill(int Index) { }

    public virtual Vector2 ChangeSkill(int Index) { return Vector2.zero; }
}
