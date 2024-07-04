using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSword : MonoBehaviour
{
    public Transform followPos; // �÷��̾� ������Ʈ
 
    public float followDelay = 0.5f; // ������� �ð� ��
    public int maxPositions = 50; // �ִ� ������ ��ġ ��
    
    public bool isNearWall = false;

    private Queue<Vector3> positions;
    private Transform followerTransform;

    void Start()
    {
        positions = new Queue<Vector3>();
        followerTransform = transform;
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {

            if (positions.Count > maxPositions)
            {
                positions.Dequeue(); // ť�� ���� �� ��� ���� ������ ��ġ ����
            }

            positions.Enqueue(followPos.position);

            // ��ġ�� ���� �ð� �̻� ť�� ����Ǿ��� �� ���󰡵��� ó��
            if (positions.Count > (followDelay / Time.deltaTime))
            {
                followerTransform.position = positions.Dequeue();
            }
     
    }
}
