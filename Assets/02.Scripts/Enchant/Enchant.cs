using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Enchant", menuName = "ScriptableObjects/Enchant")]
public class Enchant : ScriptableObject
{
    public bool isMain;
    public bool isSub;
    [Space(20f)]

    public int attackDamage = 0;
    public float attackSpeed = 0f;
    public float bulletSpeed = 0f;
    [Space(20f)]

    public int skillLevel = 0;
    public float skillCoolDown = 0f;
    [Space(20f)]

    public int swapBuff = 0;
    public int petUpgrade = 0;
    [Space(20f)]

    public int hpUp = 0;
    public bool isDrain =false;
    [Space(20f)]

    public float hpRecovery = 0f;
    public float swapRecovery = 0f;
    public float skillRecovery = 0f;
    [Space(20f)]

    public float swapDamage = 0f;
    [Space(20f)]

    public int evolutionRune = 0;

    public bool isLegedary = false;
    public string enchantName;
}
