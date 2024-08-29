using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "ScriptableObjects/Swords")]
public class Swords : ScriptableObject
{
    [Header("����")]
    public int attackdamage;
    public float attackSpeed;

    [Header("����")]
    public float swapDamage;

    [Header("��ų")]
    public float skillCost;
    public float skillCool;
    public int AskillLevel;
    public float[] AskillDamage;
    public int BskillLevel;
    public float[] BskillDamage;
    public float SwapDamage;

    [Header("ź��")]
    public float bulletSpeed;
    public BulletType bulletType;

    [Header("����")]
    public GameObject swordPrefab;
    public int swordAttackPower;
    public float swordAttackSpeed;
    public float swordBulletSpeed;

    [Header("�̹���")]
    public Sprite skillBarImage;
    public Sprite characterProfile;
    public Sprite swordProfile;
    public Sprite skillAImage;
    public Sprite skillBImage;

    [Header("����")]
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
