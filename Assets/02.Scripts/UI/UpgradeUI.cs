using System.Collections;
using System.Collections.Generic;
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
    public Button[] upgradeBtn;
    public Image[] skillIcon;
    void Start()
    {
        foreach (var button in characterSelectBtn)
        {
            button.onClick.AddListener(() => SelectCharacter(button));
        }
        illustrationChageBtn.onClick.AddListener(() => ChangeIllustration());
            // 버튼 크기를 초기화합니다.
            var firstButtonRectTransform = characterSelectBtn[0].GetComponent<RectTransform>();
        defaultButtonSize = firstButtonRectTransform.sizeDelta;
        selectedButtonSize = new Vector2(defaultButtonSize.x, 210);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //==================================================================================
    public void SetUI()
    {
        Illustration[0].sprite = swords[curSwordIndex].characterProfile;
        Illustration[1].sprite = swords[curSwordIndex].swordProfile; 
    }

    public void SelectCharacter(Button clickedButton)
    {
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
}
