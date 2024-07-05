using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IListener
{
    [Header("스킬 버튼")]
    public Image skillBtn;
    public Button skillDragBtn;

    [Header("스왑 버튼")]
    public Image swapBtn;
    private bool canSwap = false;

    void Awake()
    {
        skillDragBtn.gameObject.SetActive(false);
        skillBtn.fillAmount = 0f;
        swapBtn.fillAmount= 0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SKILL_ON, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SWAP_ON, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEvent(EVENT_TYPE Event_type, Component Sender, object Param = null)
    {
        switch (Event_type)
        {
            case EVENT_TYPE.SKILL_ON:
                float skillFillAmount = GameManager.Instance.skillCount / (float)Param;

                if (skillFillAmount >= 1f)
                {
                    skillDragBtn.gameObject.SetActive(true);
                }
                else
                {
                    skillDragBtn.gameObject.SetActive(false);
                }

                skillBtn.fillAmount = skillFillAmount;
                break;

            case EVENT_TYPE.SWAP_ON:
                float swapFillAmount = GameManager.Instance.swapCount / (float)Param;
                
                if (swapFillAmount >= 1f)
                {
                    canSwap =true;  
                }

                swapBtn.fillAmount = swapFillAmount;
                break;
        }
    }

    public void ChangeWeapon()
    {
        if(canSwap)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.CHANGE_WEAPON, this);
            GameManager.Instance.swapCount = 0f;
        }
        
    }
}
