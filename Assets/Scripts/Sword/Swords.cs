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
    public int skillDamage;
    public float skillCool;
    public int skillCount;

    [Header("����")]
    public int swapCount;

    [Header("ź��")]
    public GameObject bulletPrefab;
    public float bulletSpeed;

    [Header("����")]
    public GameObject swordPrefab;
}
