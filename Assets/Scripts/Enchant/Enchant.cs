using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Enchant", menuName = "Enchant")]
public class Enchant : ScriptableObject
{
    public int attackDamage = 0;
    public float attackSpeed = 0f;
    public float bulletSpeed = 0f;
    public float skillDamage = 0f;

    public string name;
}
