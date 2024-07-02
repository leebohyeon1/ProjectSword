using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    public TMP_Text text;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHPText(int hp)
    {
        text.text = hp.ToString();
    }
}
