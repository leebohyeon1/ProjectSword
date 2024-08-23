using UnityEngine;
using UnityEngine.UI;

public class CategoryUI : MonoBehaviour
{
    [SerializeField] private Button[] categoryButtons; // ����, ����, �̱� ���� ��ư
    [SerializeField] private Image[] categoryIcons; // ��ư �����ܵ�
    private Vector2 defaultButtonSize, selectedButtonSize;

    //==================================================================================

    void Start()
    {
        foreach (var button in categoryButtons)
        {
            button.onClick.AddListener(() => OnCategoryClick(button));
        }

        // ��ư ũ�⸦ �ʱ�ȭ�մϴ�.
        var firstButtonRectTransform = categoryButtons[0].GetComponent<RectTransform>();
        defaultButtonSize = firstButtonRectTransform.sizeDelta;
        selectedButtonSize = new Vector2(defaultButtonSize.x * 1.7f, defaultButtonSize.y);

        // �ʱ� ���·� ù ��° ��ư�� �����մϴ�.
        OnCategoryClick(categoryButtons[2]);
    }

    //==================================================================================

    void OnCategoryClick(Button clickedButton)
    {
        // Ŭ���� ��ư�� ������� UI�� ������Ʈ�մϴ�.
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
                buttonRectTransform.sizeDelta = selectedButtonSize; // ��ư ũ�� Ȯ��
                button.transform.GetChild(0).gameObject.SetActive(true); // Ȱ��ȭ ������ ǥ��
                iconTransform.localPosition = new Vector2(0, 50); // ������ ��ġ ����
                MainUIManager.Instance.SetUI(i); // UI �Ŵ������� �ش� �ε��� UI ����
            }
            else
            {
                buttonRectTransform.sizeDelta = defaultButtonSize; // ��ư ũ�� �⺻������
                button.transform.GetChild(0).gameObject.SetActive(false); // ��Ȱ��ȭ ������ �����
                iconTransform.localPosition = Vector3.zero; // ������ ��ġ �⺻������
            }
        }
    }
}
