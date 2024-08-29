using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    private int curSwordIndex = 0;
    // 마검 선택
    public Swords[] swords;
    public Button[] characterSelectBtn;  
    private Vector2 defaultButtonSize, selectedButtonSize;

    // 일러스트 변경
    public Button illustrationChageBtn;
    public Image[] Illustration;

    //강화
    public Button[] SetInformationBtn;
    public Button[] upgradeOnBtn;
    public Image[] skillIcon;

    public GameObject UpgradePanel;

    //설명 창
    public Image informationIcon;
    public TMP_Text[] informationText;

    void OnEnable()
    {
        foreach (var button in characterSelectBtn)
        {
            button.onClick.AddListener(() => SelectCharacter(button));
        }


        for (int i =0; i < swords.Length; i++)
        {
            characterSelectBtn[i].transform.GetChild(0).GetComponent<Image>().sprite = swords[i].skillBarImage;
        }


        illustrationChageBtn.onClick.AddListener(() => ChangeIllustration());
            // 버튼 크기를 초기화합니다.
            var firstButtonRectTransform = characterSelectBtn[0].GetComponent<RectTransform>();
        defaultButtonSize = firstButtonRectTransform.sizeDelta;
        selectedButtonSize = new Vector2(defaultButtonSize.x, 210);
        SetUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //==================================================================================
    public void SetUI()
    {
        Illustration[0].sprite = swords[curSwordIndex].characterProfile;
        Illustration[0].SetNativeSize();
        Illustration[0].transform.localScale = new Vector3(0.6f,0.6f,1);
        Illustration[1].sprite = swords[curSwordIndex].swordProfile;
        Illustration[1].SetNativeSize();
        Illustration[1].transform.localScale = new Vector3(0.9f, 0.9f, 1);
        skillIcon[1].sprite = swords[curSwordIndex].skillAImage;
        skillIcon[2].sprite = swords[curSwordIndex].skillBImage;
        
    }

    public void SelectCharacter(Button clickedButton)
    {
        UpgradePanel.SetActive(false);

        for (int i = 0; i < characterSelectBtn.Length; i++)
        {
            Button button = characterSelectBtn[i];
            bool isSelected = button == clickedButton;
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();

            if (isSelected)
            {
                buttonRectTransform.sizeDelta = selectedButtonSize; // 버튼 크기 확대
                button.transform.GetChild(2).gameObject.SetActive(true); // 활성화 아이콘 표시
                curSwordIndex = i;
                SetUI();
            }
            else
            {
                buttonRectTransform.sizeDelta = defaultButtonSize; // 버튼 크기 기본값으로
                button.transform.GetChild(2).gameObject.SetActive(false); // 비활성화 아이콘 숨기기
            }
        }
    }

    public void ChangeIllustration()
    {
        for (int i = 0; i < Illustration.Length; i++)
        {
            if (Illustration[i].gameObject.activeSelf)
            {
                Illustration[i].gameObject.SetActive(false);
            }
            else
            {
                Illustration[i].gameObject.SetActive(true);
            }
        }
    }

    public void SetInformation(int index)
    {
        UpgradePanel.SetActive(false);
        informationIcon.sprite = skillIcon[index].sprite;

        switch (index)
        {
            case 0:
                informationText[0].text  = swords[curSwordIndex].BasicAtkInformation;
                informationText[1].text  = swords[curSwordIndex].BasicAtkName;
                informationText[2].text  = swords[curSwordIndex].BasicAtkType;
                informationText[3].text  = " ";
                break;
            case 1:
                informationText[0].text = swords[curSwordIndex].ASkillInformation;
                informationText[1].text = swords[curSwordIndex].ASkillName;
                informationText[2].text = swords[curSwordIndex].ASkillType;
                informationText[3].text = swords[curSwordIndex].ASkillArea;
                break;
            case 2:
                informationText[0].text = swords[curSwordIndex].BSkillInformation;
                informationText[1].text = swords[curSwordIndex].BSkillName;
                informationText[2].text = swords[curSwordIndex].BSkillName;
                informationText[3].text = swords[curSwordIndex].BSkillArea;
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                informationText[0].text = swords[curSwordIndex].petInformation;
                informationText[1].text = swords[curSwordIndex].petName;
                informationText[2].text = swords[curSwordIndex].petType;
                informationText[3].text = swords[curSwordIndex].petArea;
                break;


        }
    }

    public void UpgradeOn()
    {
        if (UpgradePanel.activeSelf)
        {
            UpgradePanel.SetActive(false);
        }
        else
        {
            UpgradePanel.SetActive(true);
        }

    }

    public void Upgrade()
    {

    }
}
