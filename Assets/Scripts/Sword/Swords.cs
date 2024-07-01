using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Swords")]
public class Swords : ScriptableObject
{
    [Header("공격")]
    public int attackdamage;
    public float attackSpeed;

    [Header("스킬")]
    public int skillDamage;
    public float skillCool;
}
