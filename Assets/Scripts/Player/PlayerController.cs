using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IListener
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

        EventManager.Instance.AddListener(EVENT_TYPE.FIRE, this);
    }
    void Update()
    {
        if (!GameManager.Instance.GetSkillBool())     
        {
            Drag();
        }

        HadleAttack();
    }

    public void OnEvent(EVENT_TYPE Event_type, Component Sender, object Param = null)
    {
        attackTimer = 0;
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
        if(attackTimer > (playerStat.GetAttackSpeed() - playerStat.upAttackSpeed[playerStat.GetWeaponIndex()])) 
        {
            playerStat.GetFirePos().GetComponents<SwordFire>()[playerStat.GetWeaponIndex()].Fire();
        }
    }
 
}
