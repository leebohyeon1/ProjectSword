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

        // �巡�װ� ������ �� ��ų ���
        Panel.SetActive(false);
        UseSkill();
        GameManager.Instance.SkillOnOff();
    }

    void UseSkill()
    {
        // ���⼭ ��ų ��� ������ �����մϴ�.
        // ���� ���, �巡�� �� ��� ��ġ�� ��ų�� ����մϴ�.
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0; // z�� ��ġ�� 0���� �����Ͽ� 2D ���� ��ġ�ϰ� �մϴ�.

        // ��ų �������� �����ϰų�, �ش� ��ġ�� ��ų ȿ���� �����մϴ�.
        Debug.Log("��ų ��� ��ġ: " + worldPosition);
    }
}

