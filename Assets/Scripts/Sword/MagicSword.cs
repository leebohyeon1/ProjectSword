using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MagicSword : MonoBehaviour
{
    [Header("탄막")]
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform bulletPoolTrans;
    [SerializeField] private int poolSize = 20;
  
    [Header("행동")]
    [SerializeField] protected int attackPower;
    protected int plusAttackPower;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float bulletSpeed;

    [Header("추적")]
    [SerializeField] protected Transform followPos; // 플레이어 오브젝트
    [SerializeField] protected float followDelay = 0.1f; // 따라오는 시간 차
    [SerializeField] protected int maxPositions = 50; // 최대 저장할 위치 수

    protected Queue<Vector3> positions;
    protected Transform followerTransform;

    [SerializeField]
    protected List<GameObject> bulletPool = new List<GameObject>();

    [SerializeField] protected GameObject firePos;

    [SerializeField]
    private int buffLevel_;
    public int buffLevel
    {
        get => buffLevel_;
        set
        {
            if (buffLevel >= maxBuffLevel)
            {
                return;
            }

            buffLevel_ = Mathf.Min(value, maxBuffLevel);

          
            ApplyBuffEffects();
        }
    }

    private const int maxBuffLevel = 4;

    //================================================================================================

    private void Start()
    {
        SetTrans();
        SetFire();
    }
    void Update()
    {
        Follow();
    }

    //================================================================================================

    private void InitializeBullet(GameObject bullet)
    {
        var bulletController = bullet.GetComponent<BulletController>();
        bulletController.SetDamage(attackPower + plusAttackPower);
    }

    //================================================================================================

    public virtual void SetTrans()
    {
        positions = new Queue<Vector3>();
        followerTransform = transform;

        firePos = GameObject.Find("FirePos");
             
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

    protected virtual void InitializePool()
    {
        bulletPool.Clear();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullet.transform.SetParent(bulletPoolTrans, true);
            bulletPool.Add(bullet);
        }

        bulletPoolTrans.SetParent(null);
    }

    protected virtual GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                InitializeBullet(bullet);
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab, bulletPoolTrans);
        newBullet.SetActive(false);
        InitializeBullet(newBullet);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    public virtual void Fire()
    {
        GameObject bullet = GetBullet();
        bullet.GetComponent<BulletController>().GetSubBullet();
        bullet.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * bulletSpeed;
    }
    
    public virtual void SetBullet()
    {
        var swordFire = firePos.GetComponent<SwordFire>();
        swordFire.enabled = !swordFire.enabled;
    }
    
    public virtual void SetFire()
    {
        firePos.AddComponent<SwordFire>().SetMagicSword(this);
    }

    protected virtual void ApplyBuffEffects()
    {
        // buffLevel에 따라 추가 효과를 여기에 적용
        // 예를 들어, 공격력, 발사 속도, 탄환 속도 등을 증가시킴
        // 이 메서드는 buffLevel이 변경될 때마다 호출됨
    }

    public virtual GameObject GetBulletPrefab() => bulletPrefab;

    public virtual void SetSword(Transform Trans, int AttackPower, float AttackSpeed, float BulletSpeed)
    {
        followPos = Trans;
        attackPower = AttackPower;
        attackSpeed = AttackSpeed;
        bulletSpeed = BulletSpeed;

    }
}
