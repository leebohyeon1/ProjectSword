using UnityEngine;
using UnityEngine.UI;

public class CategoryUI : MonoBehaviour
{
    [SerializeField] private Button[] categoryButtons; // 육성, 월드, 뽑기 등의 버튼
    [SerializeField] private Image[] categoryIcons; // 버튼 아이콘들
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
        // 해당 카테고리가 클릭되었을 때 UI 변화 처리
        UpdateCategoryUI(clickedButton); 

    }

    void UpdateCategoryUI(Button clickedButton)
    {
        int index = 0;
        foreach (var button in categoryButtons)
        {
            bool isSelected = button == clickedButton;
            //button.transform.localPosition = isSelected ? new Vector3(button.transform.localPosition.x, button.transform.localPosition.y + 10, 0) : button.transform.localPosition; // 아이콘이 위로 이동
          
            if(isSelected)
            {
                button.GetComponent<RectTransform>().sizeDelta = selectButtonSize ; // 예시로 가로 크기 확장
                button.transform.GetChild(0).gameObject.SetActive(true);
                button.transform.GetChild(1).gameObject.transform.localPosition = new Vector2(0, 50);
                MainUIManager.Instance.SetUI(index);
            }
            else
            {
                button.GetComponent<RectTransform>().sizeDelta = defualtButtonSize; // 예시로 가로 크기 확장
                button.transform.GetChild(0).gameObject.SetActive(false);
                button.transform.GetChild(1).gameObject.transform.localPosition = Vector3.zero;
            }
            index++;
        }

 
    }
}
