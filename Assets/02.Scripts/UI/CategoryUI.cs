using UnityEngine;
using UnityEngine.UI;

public class CategoryUI : MonoBehaviour
{
    [SerializeField] private Button[] categoryButtons; // 육성, 월드, 뽑기 등의 버튼
    [SerializeField] private Image[] categoryIcons; // 버튼 아이콘들
    private Vector2 defaultButtonSize, selectedButtonSize;

    //==================================================================================

    void Start()
    {
        foreach (var button in categoryButtons)
        {
            button.onClick.AddListener(() => OnCategoryClick(button));
        }

        // 버튼 크기를 초기화합니다.
        var firstButtonRectTransform = categoryButtons[0].GetComponent<RectTransform>();
        defaultButtonSize = firstButtonRectTransform.sizeDelta;
        selectedButtonSize = new Vector2(defaultButtonSize.x * 1.7f, defaultButtonSize.y);

        // 초기 상태로 첫 번째 버튼을 선택합니다.
        OnCategoryClick(categoryButtons[2]);
    }

    //==================================================================================

    void OnCategoryClick(Button clickedButton)
    {
        // 클릭된 버튼을 기반으로 UI를 업데이트합니다.
        UpdateCategoryUI(clickedButton);
    }

    void UpdateCategoryUI(Button clickedButton)
    {
        for (int i = 0; i < categoryButtons.Length; i++)
        {
            Button button = categoryButtons[i];
            bool isSelected = button == clickedButton;
            RectTransform buttonRectTransform = button.GetComponent<RectTransform>();
            Transform iconTransform = button.transform.GetChild(1);

            if (isSelected)
            {
                buttonRectTransform.sizeDelta = selectedButtonSize; // 버튼 크기 확대
                button.transform.GetChild(0).gameObject.SetActive(true); // 활성화 아이콘 표시
                iconTransform.localPosition = new Vector2(0, 50); // 아이콘 위치 조정
                MainUIManager.Instance.SetUI(i); // UI 매니저에서 해당 인덱스 UI 설정
            }
            else
            {
                buttonRectTransform.sizeDelta = defaultButtonSize; // 버튼 크기 기본값으로
                button.transform.GetChild(0).gameObject.SetActive(false); // 비활성화 아이콘 숨기기
                iconTransform.localPosition = Vector3.zero; // 아이콘 위치 기본값으로
            }
        }
    }
}
