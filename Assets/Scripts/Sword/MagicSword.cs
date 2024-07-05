using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MagicSword : MonoBehaviour
{
    [Header("행동")]
    public int ActPower;
    public float ActSpeed;
    
    [Header("추적")]
    public Transform followPos; // 플레이어 오브젝트
 
    public float followDelay = 0.1f; // 따라오는 시간 차
    public int maxPositions = 50; // 최대 저장할 위치 수

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
            positions.Dequeue(); // 큐가 가득 찬 경우 가장 오래된 위치 제거
        }

        positions.Enqueue(followPos.position);

        // 위치가 일정 시간 이상 큐에 저장되었을 때 따라가도록 처리
        if (positions.Count > (followDelay / Time.deltaTime))
        {
            followerTransform.position = positions.Dequeue();
        }
    }
}
