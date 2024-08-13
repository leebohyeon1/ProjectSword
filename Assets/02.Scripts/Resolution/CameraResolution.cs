using System.Collections;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    public float targetWidth = 6f; // ���� ������ ���� ������ ���� ����
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

        // ���� ȭ�� ������ ���
        float targetAspect = screenWidth / screenHeight;

        // ī�޶��� orthographicSize�� ���� ���̿� ���� ����
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