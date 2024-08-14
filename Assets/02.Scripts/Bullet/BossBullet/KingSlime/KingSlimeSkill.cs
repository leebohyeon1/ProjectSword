using UnityEngine;

public class KingSlimeSkill : MonoBehaviour
{
    public float explosionRadius = 3f;  // 폭발 반경
    public int explosionDamage = 20;    // 폭발 데미지
    public LayerMask playerLayer;       // 플레이어 레이어 마스크

    private void Update()
    {
        CheckExplosion();
    }

    void CheckExplosion()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayer);

        if (player != null)
        {
            // 플레이어에게 폭발 데미지 입힘
            player.GetComponent<PlayerStat>().TakeDamage(explosionDamage);

            // 폭발 이펙트 추가 가능
            Explode();

            // 탄환 비활성화
            gameObject.SetActive(false);
        }
    }

    void Explode()
    {
        // 여기에 폭발 이펙트 코드 추가 가능
        Debug.Log("폭발 발생!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
