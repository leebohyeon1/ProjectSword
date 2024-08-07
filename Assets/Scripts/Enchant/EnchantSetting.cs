using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantSetting : MonoBehaviour
{
    PlayerStat playerStat;

    [SerializeField] private float[] firstPercentage;
    [SerializeField] private Upgrade[] upgrades;

    [SerializeField] private ChoiceEnchant[] choices;

    //==================================================================================

    void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();

        for (int i = 0; i < choices.Length; i++)
        {
            int randomNum = Random.Range(0, 100);
            choices[i].Enchant = RandomEnchant(randomNum);
            choices[i].TextSet();
        } 
    }

    //==================================================================================

    public Enchant RandomEnchant(int randomRate)
    {
        if (randomRate <= firstPercentage[0])
        {
            int num = Random.Range(0, 100);
            return upgrades[0].RandomUpgrade(num);

        }
        else if (randomRate <= firstPercentage[0] + firstPercentage[1])
        {
            int num = Random.Range(0, 100);
            return upgrades[1].RandomUpgrade(num);
        }
        else
        {
            int num = Random.Range(0, 100);
            return upgrades[2].RandomUpgrade(num);
        }
    }

    //==================================================================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStat playerStat = collision.GetComponent<PlayerStat>();

            int num = 0;
            float minLength = Mathf.Infinity;
            for (int i = 0; i < choices.Length; i++)
            {

                float distance = Vector2.Distance(collision.transform.position, choices[i].transform.position);


                if (distance < minLength)
                {
                    minLength = distance;
                    num = i;
                }
            }

            playerStat.Upgrade(choices[num].Enchant);

            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public class Upgrade
{
    public float[] secondPercentage;

    public Enchant[] enchantsFirst;
    public Enchant[] enchantsSecond;
    public Enchant[] enchantsThird;

    public Enchant RandomUpgrade(int randomRate)
    {
        if (randomRate <= secondPercentage[0])
        {
            int num = Random.Range(0, enchantsFirst.Length);
            return enchantsFirst[num];
        }
        else if (randomRate <= secondPercentage[0] + secondPercentage[1])
        {
            int num = Random.Range(0, enchantsSecond.Length);
            return enchantsSecond[num];
        }
        else
        {
            int num = Random.Range(0, enchantsThird.Length);
            return enchantsThird[num];
        }
    }
}
