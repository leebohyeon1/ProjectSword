using UnityEngine;
using UnityEngine.UI;

public class CategoryUI : MonoBehaviour
{
    [SerializeField] private Button[] categoryButtons; // ����, ����, �̱� ���� ��ư
    [SerializeField] private Image[] categoryIcons; // ��ư �����ܵ�
    private Vector2 defualtButtonSize, selectButtonSize;

    //==================================================================================

    void Start()
    {
        foreach (var button in categoryButtons)
        {
            button.onClick.AddListener(() => OnCategoryClick(button));
        }
        defualtButtonSize = categoryButtons[0].GetComponent<RectTransform>().sizeDelta;
        selectButtonSize = new Vector2(categoryButtons[0].GetComponent<RectTransform>().sizeDelta.x * 1.7f, categoryButtons[0].GetComponent<RectTransform>().sizeDelta.y);

        OnCategoryClick(categoryButtons[2]);
    }

    //==================================================================================

    void OnCategoryClick(Button clickedButton)
    {
        // �ش� ī�װ��� Ŭ���Ǿ��� �� UI ��ȭ ó��
        UpdateCategoryUI(clickedButton); 

    }

    void UpdateCategoryUI(Button clickedButton)
    {
        int index = 0;
        foreach (var button in categoryButtons)
        {
            bool isSelected = button == clickedButton;
            //button.transform.localPosition = isSelected ? new Vector3(button.transform.localPosition.x, button.transform.localPosition.y + 10, 0) : button.transform.localPosition; // �������� ���� �̵�
          
            if(isSelected)
            {
                button.GetComponent<RectTransform>().sizeDelta = selectButtonSize ; // ���÷� ���� ũ�� Ȯ��
                button.transform.GetChild(0).gameObject.SetActive(true);
                button.transform.GetChild(1).gameObject.transform.localPosition = new Vector2(0, 50);
                MainUIManager.Instance.SetUI(index);
            }
            else
            {
                button.GetComponent<RectTransform>().sizeDelta = defualtButtonSize; // ���÷� ���� ũ�� Ȯ��
                button.transform.GetChild(0).gameObject.SetActive(false);
                button.transform.GetChild(1).gameObject.transform.localPosition = Vector3.zero;
            }
            index++;
        }

 
    }
}
