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
    public float skillCost;
    public float skillCool;

    [Header("탄막")]
    public float bulletSpeed;

    [Header("마검")]
    public GameObject swordPrefab;
    public int swordActPower;
    public float swordActSpeed;

    [Header("이미지")]
    public Sprite skillImage;
    

}
