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
    public float skillCost;
    public float skillCool;
    public SwordSkill skill;

    [Header("탄막")]
    public GameObject bulletPrefab;
    public float bulletSpeed;

    [Header("마검")]
    public GameObject swordPrefab;
    public int swordActPower;
    public float swordActSpeed;
}
