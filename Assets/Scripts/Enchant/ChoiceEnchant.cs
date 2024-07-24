using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceEnchant : MonoBehaviour
{
    public Enchant curEnchant;
    // Start is called before the first frame update
    
    public TMP_Text text;
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