using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillBtn : MonoBehaviour,
 IBeginDragHandler, IDragHandler, IEndDragHandler, IListener
{
    private PlayerStat playerStat;
    [SerializeField] private RectTransform canvasRectTransform;
    //==========================================

    private Vector2 startPosition;
    private CanvasGroup canvasGroup;
    //==========================================
    
    [SerializeField]private GameObject darkOverlay;
    [SerializeField] private GameObject skillMask;

    [SerializeField] private float cancelSize = 7f;
    [SerializeField] private Transform skillSpawnPoint;
    [SerializeField] private int skill_Index = 0;
    private bool isDragging = false;

    //================================================================================================

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        playerStat = FindObjectOfType<PlayerStat>();

        EventManager.Instance.AddListener(EVENT_TYPE.SKILL_OFF, this);
    }

    public void OnEvent(EVENT_TYPE Event_type, Component Sender, object Param = null)
    {
        CancelSkill();
        
    }

    //================================================================================================

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.Instance.SkillOnOff();
        darkOverlay.SetActive(true);
        startPosition = transform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        skillMask.transform.localScale = Vector3.zero;

        skill_Index = 0;
        ChangeSkillSize(skill_Index);
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = Input.mousePosition;
            Vector2 DragPosition = eventData.position;

            Vector3 worldPoint = transform.position;
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, worldPoint);

            Vector3 targetPosition;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRectTransform, new Vector2(screenPoint.x , screenPoint.y ), Camera.main, out targetPosition))
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
            else if (DragPosition.y < screenHeight / 2 && skill_Index != 1)
            {
                // 2번 스킬
                skill_Index = 1;
                ChangeSkillSize(skill_Index);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            // 드래그가 끝났을 때 스킬 사용

            if (Vector2.Distance(startPosition, mousePos) > cancelSize) // 10f는 임계값, 원래 위치에 가깝다면 취소
            {
                CancelSkill();
            }
            else
            {
                UseSkill();
            }
        }

    }

    //================================================================================================

    public void CancelSkill()
    {
        Debug.Log("스킬 취소");
        playerStat.SetSKillTrans(skillMask.transform);

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        isDragging = false;

        GameManager.Instance.SkillOnOff();
        transform.position = startPosition;
        darkOverlay.SetActive(false);
    }

    void UseSkill()
    {
        Debug.Log("스킬 사용");
        playerStat.SetSKillTrans(skillMask.transform);

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        isDragging = false;

        GameManager.Instance.SkillOnOff();

        Tidebite tidebite = playerStat.GetSwords()[playerStat.GetWeaponIndex()].GetComponent<Tidebite>();
        if (tidebite != null)
        {
            tidebite.SetLevel3();
        }
        playerStat.UseSkill(skill_Index);
        transform.position = startPosition;
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

