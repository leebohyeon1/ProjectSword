using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    private int curSwordIndex = 0;
    // ���� ����
    public Swords[] swords;
    public Button[] characterSelectBtn;  
    private Vector2 defaultButtonSize, selectedButtonSize;

    // �Ϸ���Ʈ ����
    public Button illustrationChageBtn;
    public Image[] Illustration;

    //��ȭ
    public Button[] upgradeBtn;
    public Image[] skillIcon;
    void Start()
    {
        foreach (var button in characterSelectBtn)
        {
            button.onClick.AddListener(() => SelectCharacter(button));
        }
        illustrationChageBtn.onClick.AddListener(() => ChangeIllustration());
            // ��ư ũ�⸦ �ʱ�ȭ�մϴ�.
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
                buttonRectTransform.sizeDelta = selectedButtonSize; // ��ư ũ�� Ȯ��
                button.transform.GetChild(2).gameObject.SetActive(true); // Ȱ��ȭ ������ ǥ��
                curSwordIndex = i;
            }
            else
            {
                buttonRectTransform.sizeDelta = defaultButtonSize; // ��ư ũ�� �⺻������
                button.transform.GetChild(2).gameObject.SetActive(false); // ��Ȱ��ȭ ������ �����
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
