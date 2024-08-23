using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "ScriptableObjects/Swords")]
public class Swords : ScriptableObject
{
    [Header("공격")]
    public int attackdamage;
    public float attackSpeed;

    [Header("스왑")]
    public float swapDamage;

    [Header("스킬")]
    public float skillCost;
    public float skillCool;
    public int AskillLevel;
    public float[] AskillDamage;
    public int BskillLevel;
    public float[] BskillDamage;
    public float SwapDamage;

    [Header("탄막")]
    public float bulletSpeed;
    public BulletType bulletType;

    [Header("마검")]
    public GameObject swordPrefab;
    public int swordAttackPower;
    public float swordAttackSpeed;
    public float swordBulletSpeed;

    [Header("이미지")]
    public Sprite skillBarImage;
    public Sprite characterProfile;
    public Sprite swordProfile;
    public Sprite skillAImage;
    public Sprite skillBImage;

    [Header("설명")]
    public string ASkillName;
    public string ASkillType;
    public string ASkillArea;
    public string ASkillInformation;

    [Space(20f)]
    public string BSkillName;
    public string BSkillType;
    public string BSkillArea;
    public string BSkillInformation;
}
