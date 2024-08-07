using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScrollController : MonoBehaviour
{
    [SerializeField] private int[] nextFirstPattern;
    [SerializeField] private int[] nextSecondPattern;
    [SerializeField] private int[] nextThirdPattern;

    //==================================================================================

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
}
