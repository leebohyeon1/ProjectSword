using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MagicSword : MonoBehaviour
{
    [Header("�ൿ")]
    public int ActPower;
    public float ActSpeed;
    
    [Header("����")]
    public Transform followPos; // �÷��̾� ������Ʈ
 
    public float followDelay = 0.1f; // ������� �ð� ��
    public int maxPositions = 50; // �ִ� ������ ��ġ ��

    protected Queue<Vector3> positions;
    protected Transform followerTransform;


    void Update()
    {
       
    }

    protected virtual void Set()
    {
        positions = new Queue<Vector3>();
        followerTransform = transform;
    }

    protected virtual void Follow()
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
