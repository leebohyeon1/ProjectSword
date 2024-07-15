using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour, IListener
{
    public static GameUIManager Instance {  get; private set; }
    private PlayerStat playerStat;
    //====================================================================
    
    [Header("스킬 버튼")]
    public Image skillBtn;
    public Button skillDragBtn;

    [Header("스킬")]
    public GameObject skillImageBar;
    public Transform skillEndPoint;

    [Header("스왑 버튼")]
    public Image swapBtn;
    public GameObject[] keepSwapUI;
    private int keepSwap = 0;
    private bool canSwap = false;

    [Header("스왑")]
    public GameObject[] swapImageBar;
    public Transform swapEndPoint;

    [Header("일시정지 버튼")]
    public GameObject pausePanel;
    private bool isPause =false;

    [Header("이미지")]
    public Image skillProfile;
    public Image[] swapProfile;

    
    //==================================================================================

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            Instance = this;

        }
        skillBtn.fillAmount = 0f;
        swapBtn.fillAmount = 0f;

        skillDragBtn.gameObject.SetActive(false);
        pausePanel.SetActive(false);

        for (int i = 0; i < keepSwapUI.Length; i++)
        {
            keepSwapUI[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();

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

    public void ChangeWeaponBtn()
    {
        if(canSwap)
        {
            MoveSwapBar();

            EventManager.Instance.PostNotification(EVENT_TYPE.CHANGE_WEAPON, this);

        }

    }

    public void PauseBtn()
    {
        isPause = !isPause;
        GameManager.Instance.SetPause(isPause);
        pausePanel.SetActive(isPause);
    }

    public void ExitGameBtn()
    {
        PauseBtn();
        SceneManager.LoadScene(0);
    }

    public void MoveSkillBar()
    {
        skillImageBar.transform.DOMoveX(skillEndPoint.position.x, 0.7f)
            .SetEase(Ease.OutCirc).OnComplete(() => skillImageBar.transform.position = skillImageBar.transform.parent.transform.position);
    }
    public void MoveSwapBar()
    {
        int index = playerStat.weaponIndex;
        swapImageBar[index].transform.DOMoveX(swapEndPoint.position.x, 0.7f)
              .SetEase(Ease.OutCirc);
    }

}
