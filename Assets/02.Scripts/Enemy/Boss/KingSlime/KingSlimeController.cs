using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeController : BossController
{
    private int attackCount = 0;
    [SerializeField] private int skillCount = 4;

    [Space(10f)]
    private bool isSkill = false;
    [SerializeField] private float waitTime = 4f;
    [SerializeField] private int bulletCount = 3;
    [SerializeField] private float angleRange = 45f;
    [SerializeField] private float fireInterval = 0.8f;
    [SerializeField] private int fireRepeatCount = 4;

    [Space(10f)]
    [SerializeField] private int skillDamage = 10;
    [SerializeField] private int explosionDamage = 40;
    [SerializeField] private float skillSpeed = 1;

    [Space(10f)]
    [SerializeField] private GameObject[] SkillPrefab;


    //==================================================================================

    void Start()
    {
        bossStat = GetComponent<BossStat>();
    }

    void Update()
    {
        if (!isSkill)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= bossStat.GetAttackSpeed())
            {
                Attack();
            }
        }

    }

    //==================================================================================

    public override void Attack()
    {
        GameObject bullet = bossStat.GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        bulletRb.velocity = bossStat.GetAttackVector() * (bossStat.GetBulletSpeed());

        attackTimer = 0f;
        attackCount++;

        if (attackCount >= skillCount - 1)
        {
            SKill();
        }

    }

    public override void SKill()
    {
        isSkill = true;
        attackCount = 0;
        StartCoroutine(FireBullets());
    }

    private IEnumerator FireBullets()
    {
        yield return new WaitForSeconds(waitTime);

        for (int i = 0; i < fireRepeatCount; i++)
        {
            FireBulletWave();
            yield return new WaitForSeconds(fireInterval);
        }

        isSkill = false;
    }

    private void FireBulletWave()
    {
        float angleStep = angleRange / (bulletCount - 1);
        float startAngle = -angleRange / 2;

        // 무작위로 하나의 탄환을 선택
        int randomIndex = Random.Range(0, bulletCount);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // 무작위로 선택된 탄환은 SkillPrefab[1], 나머지는 SkillPrefab[0] 사용
            GameObject bulletPrefab = (i == randomIndex) ? SkillPrefab[1] : SkillPrefab[0];

            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);

            if (i == randomIndex)
            {
                bullet.GetComponent<KingSlimeSkill>().explosionDamage = explosionDamage; // 폭발 데미지 설정
            }
            else
            {
                bullet.GetComponent<BossSkillController>().InitializeSkill(skillDamage, BulletType.Wind);
            }

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = rotation * Vector2.down * skillSpeed;
        }
    }
}
