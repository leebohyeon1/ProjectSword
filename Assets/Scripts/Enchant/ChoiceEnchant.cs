using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceEnchant : MonoBehaviour
{
    public Enchant[] enchants;
    private Enchant curEnchant;
    // Start is called before the first frame update
    
    public TMP_Text text;

    private void OnEnable()
    {
        int num = Random.Range(0, enchants.Length);
        curEnchant = enchants[num];
        text.text = curEnchant.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerStat playerStat = collision.GetComponent<PlayerStat>();
            playerStat.Upgrade(curEnchant);
            Destroy(transform.parent.gameObject);
        }
       
    }

    void OnBecameInvisible()
    {
        Destroy(transform.parent.gameObject);
    }
}
