using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    [SerializeField]private TMP_Text text;

    public void UpdateHPText(int hp)
    {
        text.text = hp.ToString();
    }
}
