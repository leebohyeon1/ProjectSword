using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour, IListener
{
    public static GameUIManager Instance {  get; private set; }

    private PlayerStat playerStat;

    //====================================================================
    
    [Header("��ų ��ư")]
    [SerializeField] private Image skillBtn;
    [SerializeField] private Button skillDragBtn;

    [Header("��ų")]
    [SerializeField] private GameObject skillImageBar;
    [SerializeField] private Transform skillEndPoint;

    [Header("���� ��ư")]
    [SerializeField] private Image swapBtn;
    [SerializeField] private GameObject[] keepSwapUI;
    private int keepSwap = 0;
    private bool canSwap = false;

    [Header("����")]
    [SerializeField] private GameObject[] swapImageBar;
    [SerializeField] private Transform swapEndPoint;

    [Header("�Ͻ����� ��ư")]
    [SerializeField] private GameObject pausePanel;
    private bool isPause =false;

    [Header("��ų/���� �Ƕ���")]
    [SerializeField] private Image skillProfile;
    [SerializeField] private Image[] swapProfile;

    [Header("��ô��")]
    [SerializeField] private Slider gameProgress;

    [Header("����UI")]
    [SerializeField] private Slider bossHpBar;
    [SerializeField] private TMP_Text bossName;

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
        gameProgress.value = 0f;

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

    void Update()
    {
        DisplayProgress();
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

    #region Btn
    public void ChangeWeaponBtn()
    {
        if(canSwap)
        {
            StartCoroutine(MoveSwapBar());
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
    #endregion

    #region Skill/Swap UI
    public void MoveSkillBar()
    {
        skillImageBar.transform.DOMoveX(skillEndPoint.position.x, 0.7f)
            .SetEase(Ease.OutCirc).OnComplete(() => skillImageBar.transform.position = skillImageBar.transform.parent.transform.position);
    }

    public IEnumerator MoveSwapBar()
    {
        Time.timeScale = 0.3f;

        int index = playerStat.GetWeaponIndex();
        swapImageBar[index].transform.DOMoveX(swapEndPoint.position.x, 0.2f).SetEase(Ease.OutCirc)
            .OnComplete(() => swapImageBar[index].transform.position = swapImageBar[index].transform.parent.transform.position);

        yield return new WaitForSeconds(0.6f);

        EventManager.Instance.PostNotification(EVENT_TYPE.CHANGE_WEAPON, this);

        Time.timeScale = 1f;
        swapImageBar[index].transform.DOMoveX(swapEndPoint.position.x, 0.6f).SetEase(Ease.OutCirc)
          .OnComplete(() => swapImageBar[index].transform.position = swapImageBar[index].transform.parent.transform.position);
    }

    public void SetSwapUIImage(Sprite mainWeapon, Sprite subWeapon)
    {
        swapProfile[0].sprite = mainWeapon;
        swapProfile[1].sprite = subWeapon;
    }

    public void SetSkillImage(Sprite Weapon)
    {
        skillProfile.sprite = Weapon;
    }

    #endregion

    #region GameProgress

    public void DisplayProgress()
    {
        if (SpawnManager.Instance.BossCount() != 0)
        {
            float progress = (float)SpawnManager.Instance.BossCount() /SpawnManager.Instance.BossSpawnCount()[SpawnManager.Instance.BossLength() - 1] ;
            gameProgress.value = progress;
        }
  
    }

    public void BossUIOn(string name)
    {
        gameProgress.gameObject.SetActive(false);
        bossHpBar.gameObject.SetActive(true);
        bossName.gameObject.SetActive(true);
        bossName.text = name;
    }

    public void BossUIOff()
    {
        gameProgress.gameObject.SetActive(true);
        bossHpBar.gameObject.SetActive(false);
        bossName.gameObject.SetActive(false);
    }

    public void UpdateBossUI(int maxHp, int hp)
    {
        bossHpBar.value = (float)hp/maxHp;
    }
    #endregion

}
