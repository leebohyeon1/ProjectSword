using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "ScriptableObjects/Swords")]
public class Swords : ScriptableObject
{
  

    [Header("공격")]
    public int basicAttackLevel = 0;
    public int attackLevel = 0;
    public int attackSpeedLevel = 0;
    public float[] BasicAttackDamageByLevel;

    public int[] AttackDamageByLevel;
    public int attackdamage;

    public float[] AttackSpeedByLevel;
    public float attackSpeed;

    [Header("스왑")]
    public float swapDamage;

    [Header("스킬")]
    public float skillCost;
    public float skillCool;
    
    public int AskillLevel;
    public float[] AskillDamageByLevel;
    public int AskillDamage;
    
    public int BskillLevel;
    public float[] BskillDamageByLevel;
    public float BskillDamage;

    public float SwapDamage;

    [Header("탄막")]
    public float bulletSpeed;
    public BulletType bulletType;

    [Header("마검")]
    public int swordLevel;
    public float[] SwordAttackDamageByLevel;
    public float[] SwordAttackSpeedByLevel;
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

    [Header("기본 공격")]
    public string BasicAtkName;
    public string BasicAtkType;
    public string BasicAtkInformation;

    [Header("스킬 A")]
    public string ASkillName;
    public string ASkillType;
    public string ASkillArea;
    public string ASkillInformation;

    [Header("스킬 B")]
    public string BSkillName;
    public string BSkillType;
    public string BSkillArea;
    public string BSkillInformation;

    [Header("정령체")]
    public string petName;
    public string petType;
    public string petArea;
    public string petInformation;
}
