using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Swords")]
public class Swords : ScriptableObject
{
    [Header("����")]
    public int attackdamage;
    public float attackSpeed;

    [Header("��ų")]
    public float skillCost;
    public float skillCool;

    [Header("ź��")]
    public float bulletSpeed;
    public BulletType bulletType;

    [Header("����")]
    public GameObject swordPrefab;
    public int swordAttackPower;
    public float swordAttackSpeed;
    public float swordBulletSpeed;

    [Header("�̹���")]
    public Sprite skillImage;
}
