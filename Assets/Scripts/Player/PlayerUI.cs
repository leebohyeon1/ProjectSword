using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TMP_Text hpText;

    private void Start()
    {
        UpdateHp(GetComponent<PlayerStat>().curHp);
    }

    public void UpdateHp(int hp)
    {
        hpText.text = hp.ToString();
    }
}
