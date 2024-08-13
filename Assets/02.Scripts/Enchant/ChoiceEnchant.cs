using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceEnchant : MonoBehaviour
{
    [SerializeField] private Enchant curEnchant;
    public Enchant Enchant
    {
        get { return curEnchant; }
        set { curEnchant = value; }
    }

    [SerializeField] private TMP_Text text;

    //==================================================================================

    void OnBecameInvisible()
    {
        Destroy(transform.parent.gameObject);
    }

    //==================================================================================

    public void TextSet()
    {
        text.text = curEnchant.name;
    }

 
}