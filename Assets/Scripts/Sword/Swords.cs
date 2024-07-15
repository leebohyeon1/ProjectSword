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

    [Header("����")]
    public GameObject swordPrefab;
    public int swordActPower;
    public float swordActSpeed;

    [Header("�̹���")]
    public Sprite skillImage;
    

}
