using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceEnchant : MonoBehaviour
{
    public float[] percentage;
    public Enchant[] enchantsFirst;
    public Enchant[] enchantsSecond;
    public Enchant[] enchantsThird;
    private Enchant curEnchant;
    // Start is called before the first frame update
    
    public TMP_Text text;

    private void OnEnable()
    {
        int randomNum = Random.Range(0, 100);
        RandomEnchant(randomNum);
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

    public void RandomEnchant(int randomRate)
    {
        if (randomRate <= percentage[0])
        {
            int num = Random.Range(0, enchantsFirst.Length);
            curEnchant = enchantsFirst[num];

        }
        else if( randomRate <= percentage[0] + percentage[1]) 
        {
            int num = Random.Range(0, enchantsSecond.Length);
            curEnchant = enchantsSecond[num];
        }
        else
        {
            int num = Random.Range(0, enchantsThird.Length);
            curEnchant = enchantsThird[num];

        }
    }
}
