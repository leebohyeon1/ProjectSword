using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScrollController : MonoBehaviour
{
    public float moveSpeed = 2.0f;

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        CheckIfOutOfScreen();
    }

    void CheckIfOutOfScreen()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y < -2f)
        {
            Destroy(gameObject);
        }
    }
}
