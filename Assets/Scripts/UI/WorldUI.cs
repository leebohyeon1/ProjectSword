using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour
{
    [SerializeField] private Button[] worldButtons;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var button in worldButtons)
        {
            button.onClick.AddListener(() => OnWorldClick(button));
        }
    }

    void OnWorldClick(Button clickedButton)
    {
        UpdateWorldUI(clickedButton);
    }

    void UpdateWorldUI(Button clickedButton)
    {
        foreach (var button in worldButtons)
        {
            bool isSelected = button == clickedButton;
            //button.transform.localPosition = isSelected ? new Vector3(button.transform.localPosition.x, button.transform.localPosition.y + 10, 0) : button.transform.localPosition; // 아이콘이 위로 이동

            if (isSelected)
            {
                button.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                button.transform.GetChild(0).gameObject.SetActive(false);
            }
        }


    }

}
