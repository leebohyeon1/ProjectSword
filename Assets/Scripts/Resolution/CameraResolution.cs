using System.Collections;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    public float targetWidth = 6f; // 게임 내에서 가로 길이의 월드 단위
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
        float targetAspect = screenWidth / screenHeight;

        // 카메라의 orthographicSize를 가로 길이에 맞춰 설정
        cam.orthographicSize = targetWidth / (2f * targetAspect);
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