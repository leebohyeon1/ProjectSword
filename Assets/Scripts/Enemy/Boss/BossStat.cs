using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    PlayerStat playerStat;

    [SerializeField] private int hp;
    [SerializeField] private float speed;
    [SerializeField] private float swapGage;

    [Header("����")]
    [SerializeField] private int damage;
    [SerializeField] private float criticalRate;
    [SerializeField] private float criticalDamage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float bulletSpeed;

    [Header("��ų")]
    [SerializeField] private int AttackCount;

    [Header("ź��")]
    [SerializeField] private GameObject bossBulletPrefab;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private BulletType bulletType;
    private List<GameObject> bulletPool = new List<GameObject>();

    [Header("����")]
    private List<float> speedModifiers = new List<float>();
    public bool isIce = false;
    public bool isMolar = false;

    // Start is called before the first frame update
    void Start()
    {
        playerStat = FindFirstObjectByType<PlayerStat>();

        InitializePool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
            SpawnManager.Instance.KillBoss();

            playerStat.SetMaxSkillGage();
            EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, swapGage);
        }
    }

    private void InitializePool()
    {
        bulletPool.Clear();
        for (int i = 0; i < 10; i++)
        {
            GameObject bullet = Instantiate(bossBulletPrefab);
            bullet.SetActive(false);
            bullet.transform.SetParent(bulletParent, true);
            bulletPool.Add(bullet);
        }
    }
    private void InitializeBullet(GameObject bullet)
    {
        var controller = bullet.GetComponent<BossBulletController>();
        controller.InitializeBullet(CalculateDamage(damage), bulletType);
    }
  
    private int CalculateDamage(int baseDamage)
    {
        bool isCritical = Random.Range(0f, 100f) < criticalRate;
        return isCritical ? Mathf.RoundToInt(baseDamage * (1 + criticalDamage / 100f)) : baseDamage;
    }

    public void DecreaseSpeed(float rate)
    {
        // �ӵ� ������ �߰�
        speedModifiers.Add(rate);
        ApplySpeed();
    }
    public void IncreaseSpeed(float rate)
    {
        // �ӵ� ������ �߰� (���� ���� �ӵ� ���� ����Ʈ�� �߰�)
        speedModifiers.Add(-rate);
        ApplySpeed();
    }

    private void ApplySpeed()
    {
        // ��� �ӵ� �������� �����Ͽ� ���� �ӵ��� ���
        float totalRate = 0f;
        foreach (float modifier in speedModifiers)
        {
            totalRate += modifier;
        }

        float finalSpeed = speed * (1 - totalRate / 100f);
        GetComponent<Rigidbody2D>().velocity = Vector2.down * (finalSpeed + SpawnManager.Instance.plusAcceleration);
    }

    public GameObject GetBullet()
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                InitializeBullet(bullet);
                return bullet;
            }
        }

        var newBullet = Instantiate(bossBulletPrefab);
        newBullet.SetActive(false);
        InitializeBullet(newBullet);
        bulletPool.Add(newBullet);
        return newBullet;
    }
    public Vector2 GetAttackVector()
    {
        Vector2 attackVector = playerStat.transform.position - transform.position;
        return attackVector.normalized;
    }

    public float GetBulletSpeed() => bulletSpeed;
    public float GetAttackSpeed() => attackSpeed;

}
