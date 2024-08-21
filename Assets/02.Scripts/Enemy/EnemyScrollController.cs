using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScrollController : MonoBehaviour
{
    [SerializeField] private int[] nextFirstPattern;
    [SerializeField] private int[] nextSecondPattern;
    [SerializeField] private int[] nextThirdPattern;

    //==================================================================================

    private void Start()
    {
        IncreaseHp(SpawnManager.Instance.totalHp);    
    }

    void Update()
    {
        //transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        CheckIfOutOfScreen();
    }

    //==================================================================================

    void CheckIfOutOfScreen()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

    public int[] NextPattern(int num)
    {
        switch (num)
        {
            case 0:
                return nextFirstPattern;
            case 1:
                return nextSecondPattern;
            case 2:
                return nextThirdPattern;
            default:
                return nextFirstPattern;
        }
    }

    public void IncreaseHp(int hp)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<EnemyStat>().IncreaseHp(hp);
        }
    }
}
