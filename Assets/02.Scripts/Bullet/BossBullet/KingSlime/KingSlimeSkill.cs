using UnityEngine;

public class KingSlimeSkill : MonoBehaviour
{
    public float explosionRadius = 3f;  // ���� �ݰ�
    public int explosionDamage = 20;    // ���� ������
    public LayerMask playerLayer;       // �÷��̾� ���̾� ����ũ

    private void Update()
    {
        CheckExplosion();
    }

    void CheckExplosion()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayer);

        if (player != null)
        {
            // �÷��̾�� ���� ������ ����
            player.GetComponent<PlayerStat>().TakeDamage(explosionDamage);

            // ���� ����Ʈ �߰� ����
            Explode();

            // źȯ ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }

    void Explode()
    {
        // ���⿡ ���� ����Ʈ �ڵ� �߰� ����
        Debug.Log("���� �߻�!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
