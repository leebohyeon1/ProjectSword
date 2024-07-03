using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IListener
{
    [Header("스킬 버튼")]
    public Image skillBtn;
    public Button skillDragBtn;
    private float fillAmount;
    void Awake()
    {
        skillDragBtn.gameObject.SetActive(false);
        skillBtn.fillAmount = 0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SKILL_ON, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEvent(EVENT_TYPE Event_type, Component Sender, object Param = null)
    {
        float fillAmount = (float)GameManager.Instance.monCount / (int)Param;

        if (fillAmount >= 1)
        {
            skillDragBtn.gameObject.SetActive(true);
        }
        else
        {
         
            skillDragBtn.gameObject.SetActive(false);
        }
        skillBtn.fillAmount = fillAmount;
    }

    public void ChangeWeapon()
    {
        EventManager.Instance.PostNotification(EVENT_TYPE.CHANGE_WEAPON, this);
    }
}
