using System.Collections;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    public float targetWidth = 6f; // 게임 내에서 가로 길이의 월드 단위
    public float maxHeight = 10f;  // 게임 내에서 허용하는 최대 높이
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 현재 화면 비율을 계산
        float screenAspect = screenWidth / screenHeight;

        // 가로 길이를 고정하고 세로 길이를 조정
        float orthographicSize = targetWidth / (2f * screenAspect);

        // 현재 세로 길이가 maxHeight를 초과하는지 확인
        if (orthographicSize * 2f > maxHeight)
        {
            // 세로 길이를 maxHeight에 맞추고 레터박스 처리
            orthographicSize = maxHeight / 2f;

            float letterboxRatio = orthographicSize * screenAspect / (targetWidth / 2f);
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = letterboxRatio;
            rect.x = 0;
            rect.y = (1.0f - letterboxRatio) / 2.0f;

            cam.rect = rect;
        }
        else
        {
            // 전체 화면 사용
            cam.rect = new Rect(0, 0, 1, 1);
        }

        // 조정된 orthographicSize를 카메라에 설정
        cam.orthographicSize = orthographicSize;
    }

    void OnValidate()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
        AdjustCameraSize();
    }

    void Update()
    {
        if (Screen.width != cam.pixelWidth || Screen.height != cam.pixelHeight)
        {
            AdjustCameraSize();
        }
    }
}
