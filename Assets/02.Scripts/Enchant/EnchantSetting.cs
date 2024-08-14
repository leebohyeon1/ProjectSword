using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        for (int i = 0; i < 2; i++)
        {
            MagicSword magicSword = playerStat.GetSwords()[i].GetComponent<MagicSword>();
            if (magicSword != null && magicSword.buffLevel == 4)
            {
                List<Enchant> numbersList = new List<Enchant>(upgrades[1].enchantsSecond);
 
                // 특정 값(예: 3)을 제거
                numbersList.Remove(upgrades[1].enchantsSecond[i]);  // 첫 번째로 발견된 3만 제거됩니다.

                // 리스트를 배열로 다시 변환
                upgrades[1].enchantsSecond = numbersList.ToArray();

            }
            if (upgrades[1].enchantsSecond.Length == 0)
            {
                upgrades[1].secondPercentage[0] = 100;
            }

            if (magicSword != null && magicSword.evolutionLevel == 4)
            {
                List<Enchant> numbersList = new List<Enchant>(upgrades[0].enchantsThird);
   
                // 특정 값(예: 3)을 제거
                numbersList.Remove(upgrades[0].enchantsThird[i]);  // 첫 번째로 발견된 3만 제거됩니다.

                // 리스트를 배열로 다시 변환
                upgrades[0].enchantsThird = numbersList.ToArray();

            }
            if (upgrades[0].enchantsThird.Length == 0)
            {
                upgrades[0].secondPercentage[0] += 10;
                upgrades[0].secondPercentage[1] += 10;
            }

            
            if (magicSword != null && magicSword.evolutionLevel == 4)
            {
                upgrades[2].enchantsFourth.Add(upgrades[2].legendaryQuests[i]);
            }
            
     
        }

        if (!GameManager.Instance.GetLegendaryQuest())
        {
            upgrades[2].secondPercentage[0] -= 10;
            upgrades[2].secondPercentage[1] -= 10;
            upgrades[2].secondPercentage[2] -= 10;
        }
        
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
    public List<Enchant> enchantsFourth;

    public Enchant[] legendaryQuests;

    public Enchant RandomUpgrade(int randomRate)
    {
        if (enchantsFourth.Count ==0)
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
        else
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
            else if( randomRate <= secondPercentage[0] + secondPercentage[1] + secondPercentage[3])
            {
                int num = Random.Range(0, enchantsThird.Length);
                return enchantsThird[num];
            }
            else
            {
                int num = Random.Range(0, enchantsFourth.Count);
                return enchantsFourth[num];
            }
        }
      
    }
}
