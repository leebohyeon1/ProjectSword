using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillBtn : MonoBehaviour,
 IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private CanvasGroup canvasGroup;
    public GameObject Panel;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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

        // 드래그가 끝났을 때 스킬 사용
        Panel.SetActive(false);
        UseSkill();
        GameManager.Instance.SkillOnOff();
    }

    void UseSkill()
    {
        // 여기서 스킬 사용 로직을 구현합니다.
        // 예를 들어, 드래그 앤 드롭 위치에 스킬을 사용합니다.
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0; // z축 위치를 0으로 설정하여 2D 평면상에 위치하게 합니다.

        // 스킬 프리팹을 스폰하거나, 해당 위치에 스킬 효과를 적용합니다.
        Debug.Log("스킬 사용 위치: " + worldPosition);
    }
}

