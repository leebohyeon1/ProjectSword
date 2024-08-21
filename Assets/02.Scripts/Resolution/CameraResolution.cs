using System.Collections;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    public float targetWidth = 6f; // ���� ������ ���� ������ ���� ����
    public float maxHeight = 10f;  // ���� ������ ����ϴ� �ִ� ����
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
        float screenAspect = screenWidth / screenHeight;

        // ���� ���̸� �����ϰ� ���� ���̸� ����
        float orthographicSize = targetWidth / (2f * screenAspect);

        // ���� ���� ���̰� maxHeight�� �ʰ��ϴ��� Ȯ��
        if (orthographicSize * 2f > maxHeight)
        {
            // ���� ���̸� maxHeight�� ���߰� ���͹ڽ� ó��
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
            // ��ü ȭ�� ���
            cam.rect = new Rect(0, 0, 1, 1);
        }

        // ������ orthographicSize�� ī�޶� ����
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
