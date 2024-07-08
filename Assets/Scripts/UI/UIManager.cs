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
    public GameObject[] keepSwapUI;
    private int keepSwap = 0;
    private bool canSwap = false;
    //==================================================================================

    void Awake()
    {
        skillBtn.fillAmount = 0f;
        swapBtn.fillAmount = 0f;

        skillDragBtn.gameObject.SetActive(false);
        
        for (int i = 0; i < keepSwapUI.Length; i++)
        {
            keepSwapUI[i].gameObject.SetActive(false);
        }
    }

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.SKILL_COUNT, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SWAP_COUNT, this);

        EventManager.Instance.AddListener(EVENT_TYPE.SKILL_ON, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SWAP_ON, this);

        EventManager.Instance.AddListener(EVENT_TYPE.KEEP_SWAP, this);
    }

    public void OnEvent(EVENT_TYPE Event_type, Component Sender, object Param = null)
    {
        switch (Event_type)
        {
            case EVENT_TYPE.SKILL_COUNT:
                if(Sender.GetComponent<PlayerStat>() != null)
                {
                    float skillFillAmount = (float)Param;

                    skillBtn.fillAmount = skillFillAmount;
                }        
                break;

            case EVENT_TYPE.SKILL_ON:
                skillDragBtn.gameObject.SetActive((bool)Param);
                break;

            case EVENT_TYPE.SWAP_COUNT:
                if (Sender.GetComponent<PlayerStat>() != null)
                {
                    float swapFillAmount = (float)Param;

                    swapBtn.fillAmount = swapFillAmount;
                }
                break;
            case EVENT_TYPE.SWAP_ON:
                    canSwap = (bool)Param;      
                break;
            case EVENT_TYPE.KEEP_SWAP:
                keepSwap = (int)Param;
                
                for(int i = 0; i < keepSwapUI.Length; i ++ )
                {
                    if(i < keepSwap)
                    {
                        keepSwapUI[i].SetActive(true);
                    }
                    else
                    {
                        keepSwapUI[i].SetActive(false);
                    }                   
                }
                break;
        }
    }
    //==================================================================================

    public void ChangeWeapon()
    {
        if(canSwap)
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.CHANGE_WEAPON, this);
        }
        
    }
}
