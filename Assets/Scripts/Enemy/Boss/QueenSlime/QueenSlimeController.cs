using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class QueenSlimeController : BossController
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
    [SerializeField] private float skillSpeed = 1;

    [Space(10f)]
    [SerializeField] private GameObject SkillPrefab;
    
    
    void Start()
    {
        bossStat = GetComponent<BossStat>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSkill)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= bossStat.GetAttackSpeed())
            {
                Attack();
            }
        }
       
    }

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

        if (attackCount >= skillCount -1 )
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

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            GameObject bullet = Instantiate(SkillPrefab, transform.position, rotation);
            bullet.GetComponent<BossSkillController>().InitializeSkill(skillDamage, BulletType.Water);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = rotation * Vector2.down * skillSpeed;
        }
    }
}
