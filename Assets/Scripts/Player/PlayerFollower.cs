using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private GameObject leftFollower; // 왼쪽 대각선 뒤 오브젝트
    [SerializeField] private GameObject rightFollower; // 오른쪽 대각선 뒤 오브젝트
    [SerializeField] private LayerMask wallLayer; // 벽 레이어 마스크
    [SerializeField] private float wallDetectionRadius = 0.5f; // 벽 감지 반경

    RaycastHit2D hit;

    void Update()
    {
        // 벽에 가까운지 감지
        if (IsNearWall(Vector2.right))
        {
            RightMoveFollowersBehindPlayer(hit);
        }
        else if(IsNearWall(Vector2.left))
        {
            LeftMoveFollowersBehindPlayer(hit);
        }
        else
        {
            leftFollower.transform.position = Vector2.MoveTowards(leftFollower.transform.position, new Vector2(transform.position.x - 1, transform.position.y - 1), 2f * Time.deltaTime);

            rightFollower.transform.position = Vector2.MoveTowards(rightFollower.transform.position, new Vector2(transform.position.x + 1, transform.position.y - 1), 2f * Time.deltaTime);
        }
    }

    private bool IsNearWall(Vector2 direction)
    {
        // 플레이어 주변에 특정 방향의 벽이 있는지 감지
        hit = Physics2D.Raycast(transform.position, direction, wallDetectionRadius, wallLayer);
        return hit.collider != null;
    }

    private void RightMoveFollowersBehindPlayer(RaycastHit2D hit)
    {

        Vector3 playerBackward = Vector2.down ; // 플레이어의 뒤쪽 방향

        //rightFollower.transform.position = transform.position + playerBackward; // 오른쪽 뒤
        float distance = Vector2.Distance(hit.point,transform.position);
        rightFollower.transform.position = Vector2.MoveTowards(rightFollower.transform.position, playerBackward + new Vector3(transform.position.x + distance - 0.5f, transform.position.y), 2f * Time.deltaTime);
    }

    private void LeftMoveFollowersBehindPlayer(RaycastHit2D hit)
    { 

        Vector3 playerBackward = Vector2.down; // 플레이어의 뒤쪽 방향

        //leftFollower.transform.position = transform.position + playerBackward; // 왼쪽 뒤
        float distance = Vector2.Distance(transform.position, hit.point);
        leftFollower.transform.position = Vector2.MoveTowards(leftFollower.transform.position, playerBackward + new Vector3(transform.position.x - distance + 0.5f, transform.position.y), 2f * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        // 벽 감지 반경을 시각적으로 표시
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, transform.right * wallDetectionRadius);
        Gizmos.DrawRay(transform.position, -transform.right * wallDetectionRadius);

    }
}