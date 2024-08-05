using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScrollController : MonoBehaviour
{
    //public float moveSpeed = 2.0f;
    public float baseSpawnRate;
    public float curSpawnRate;

    public int[] nextFirstPattern;
    public int[] nextSecondPattern;
    public int[] nextThirdPattern;

    void Update()
    {
        //transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        CheckIfOutOfScreen();
    }

    void CheckIfOutOfScreen()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }
        //Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        //if (screenPosition.y < -2f)
        //{
        //    Destroy(gameObject);
        //}
    }
}
