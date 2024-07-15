using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillBtn : MonoBehaviour,
 IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private PlayerStat playerStat;
    public RectTransform canvasRectTransform;
    //==========================================

    private Vector2 startPosition;
    private CanvasGroup canvasGroup;
    //==========================================
    
    public GameObject darkOverlay;
    public GameObject skillMask;

    public float cancelSize = 7f;
    public Transform skillSpawnPoint;
    public int skill_Index = 0;

    
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        playerStat = FindObjectOfType<PlayerStat>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.Instance.SkillOnOff();
        darkOverlay.SetActive(true);
        startPosition = transform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        Vector2 DragPosition = eventData.position;

        Vector3 worldPoint = transform.position;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPoint);

        Vector3 targetPosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, new Vector2(screenPoint.x + 75, screenPoint.y + 75), Camera.main, out targetPosition))
        {
            skillMask.transform.position = targetPosition;          
        }

        float screenHeight = Screen.height;
        Vector2 dragVector = DragPosition - startPosition;

        if (DragPosition.y > screenHeight / 2 && skill_Index != 0)
        {
            // 1번 스킬 
            skill_Index = 0;
            ChangeSkillSize(skill_Index);
        }
        else if(DragPosition.y < screenHeight / 2 && skill_Index != 1)
        {
            // 2번 스킬
            skill_Index = 1;
            ChangeSkillSize(skill_Index);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        playerStat.SetSKillTrans(skillMask.transform);

        transform.position = startPosition;

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

     
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        // 드래그가 끝났을 때 스킬 사용

        if (Vector2.Distance(startPosition,mousePos) > cancelSize) // 10f는 임계값, 원래 위치에 가깝다면 취소
        {
            CancelSkill();
        }
        else
        {
            UseSkill();
        }

    }

    private void CancelSkill()
    {
        GameManager.Instance.SkillOnOff();

        darkOverlay.SetActive(false);
    }

    void UseSkill()
    {
        GameManager.Instance.SkillOnOff();
        playerStat.UseSkill(skill_Index);
        GameUIManager.Instance.MoveSkillBar();
        darkOverlay.SetActive(false);
    }

    void ChangeSkillSize(int index)
    {
        Transform Parent = skillMask.transform.parent;

        skillMask.transform.SetParent(null);  
        skillMask.transform.localScale = playerStat.SetWeaponSize(skill_Index);
        skillMask.transform.SetParent(Parent);

    }

}

