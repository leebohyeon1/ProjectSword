using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStat playerStat;
    private Rigidbody2D rb;
    //=====================================

    private Vector3 touchStartPos;
    private Vector3 characterStartPos;
    private bool isDragging = false;
    private float attackTimer;

    private void Start()
    {
        playerStat = GetComponent<PlayerStat>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Drag();
        HadleAttack();
    }

    public void Drag()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
                characterStartPos = transform.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector3 currentTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
                Vector3 offset = currentTouchPos - touchStartPos;
                rb.MovePosition(new Vector3(characterStartPos.x + offset.x, transform.position.y, transform.position.z));
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

    public void HadleAttack()
    {
        attackTimer += Time.deltaTime;
        if(attackTimer > playerStat.attackSpeed) 
        {
            Fire();
            attackTimer = 0f;
        }
    }

    public void Fire()
    {
        GameObject bullet = playerStat.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = Vector2.up * playerStat.bulletSpeed;

    }

  
}
