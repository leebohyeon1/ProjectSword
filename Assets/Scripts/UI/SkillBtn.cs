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
        // �巡�װ� ������ �� ��ų ���
        if (Vector2.Distance(mousePos, startPosition) > 10f) // 10f�� �Ӱ谪, ���� ��ġ�� �����ٸ� ���
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
        Debug.Log("��ų ��� ��ҵ�");
        // ��ų ��� ��� ���� �߰�
        Panel.SetActive(false);
        GameManager.Instance.SkillOnOff();
    }

    void UseSkill()
    {
        // ���⼭ ��ų ��� ������ �����մϴ�.
        // ���� ���, �巡�� �� ��� ��ġ�� ��ų�� ����մϴ�.
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);    
        worldPosition.z = 0;
        Debug.Log("��ų ��� ��ġ: " + worldPosition);

        // ��ų �������� �����ϰų�, �ش� ��ġ�� ��ų ȿ���� �����մϴ�.
        GameManager.Instance.SkillOnOff();
        playerStat.UseSkill();

        Panel.SetActive(false);
    }
}

