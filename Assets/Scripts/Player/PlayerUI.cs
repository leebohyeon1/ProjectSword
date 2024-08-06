using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private HealthBar healthBar;

    private float Hp;
    private float MaxHp;

    private void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();

        Hp = GetComponent<PlayerStat>().GetCurHP();
        MaxHp = GetComponent<PlayerStat>().GetMaxHP();
    }

    private void Update()
    {
        healthBar.UpdateHp(Hp, MaxHp, 0);
    }

    public void UpdateHp(float hp, float maxHp)
    {
        Hp = hp;
        MaxHp = maxHp;
    }

}
