using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private GameObject leftFollower; // ���� �밢�� �� ������Ʈ
    [SerializeField] private GameObject rightFollower; // ������ �밢�� �� ������Ʈ
    [SerializeField] private LayerMask wallLayer; // �� ���̾� ����ũ
    [SerializeField] private float wallDetectionRadius = 0.5f; // �� ���� �ݰ�

    RaycastHit2D hit;

    void Update()
    {
        // ���� ������� ����
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
        // �÷��̾� �ֺ��� Ư�� ������ ���� �ִ��� ����
        hit = Physics2D.Raycast(transform.position, direction, wallDetectionRadius, wallLayer);
        return hit.collider != null;
    }

    private void RightMoveFollowersBehindPlayer(RaycastHit2D hit)
    {

        Vector3 playerBackward = Vector2.down ; // �÷��̾��� ���� ����

        //rightFollower.transform.position = transform.position + playerBackward; // ������ ��
        float distance = Vector2.Distance(hit.point,transform.position);
        rightFollower.transform.position = Vector2.MoveTowards(rightFollower.transform.position, playerBackward + new Vector3(transform.position.x + distance - 0.5f, transform.position.y), 2f * Time.deltaTime);
    }

    private void LeftMoveFollowersBehindPlayer(RaycastHit2D hit)
    { 

        Vector3 playerBackward = Vector2.down; // �÷��̾��� ���� ����

        //leftFollower.transform.position = transform.position + playerBackward; // ���� ��
        float distance = Vector2.Distance(transform.position, hit.point);
        leftFollower.transform.position = Vector2.MoveTowards(leftFollower.transform.position, playerBackward + new Vector3(transform.position.x - distance + 0.5f, transform.position.y), 2f * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        // �� ���� �ݰ��� �ð������� ǥ��
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, transform.right * wallDetectionRadius);
        Gizmos.DrawRay(transform.position, -transform.right * wallDetectionRadius);

    }
}