using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillBtn : MonoBehaviour,
 IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private PlayerStat playerStat;
    private Vector3 startPosition;
    private CanvasGroup canvasGroup;
    public GameObject Panel;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        playerStat = FindObjectOfType<PlayerStat>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.Instance.SkillOnOff();
        Panel.SetActive(true);
        startPosition = transform.position;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPosition;
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        // 드래그가 끝났을 때 스킬 사용
        if (Vector2.Distance(mousePos, startPosition) > 10f) // 10f는 임계값, 원래 위치에 가깝다면 취소
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
        Debug.Log("스킬 사용 취소됨");
        // 스킬 사용 취소 로직 추가
        Panel.SetActive(false);
        GameManager.Instance.SkillOnOff();
    }

    void UseSkill()
    {
        // 여기서 스킬 사용 로직을 구현합니다.
        // 예를 들어, 드래그 앤 드롭 위치에 스킬을 사용합니다.
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);    
        worldPosition.z = 0;
        Debug.Log("스킬 사용 위치: " + worldPosition);

        // 스킬 프리팹을 스폰하거나, 해당 위치에 스킬 효과를 적용합니다.
        GameManager.Instance.SkillOnOff();
        playerStat.UseSkill();

        Panel.SetActive(false);
    }
}

