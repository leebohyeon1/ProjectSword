using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;



public class MagicSword : MonoBehaviour
{
    [Header("탄막")]
    public GameObject bulletPrefab;
    public Transform bulletPoolTrans;
    public int poolSize = 20;
  

    [Header("행동")]
    public int attckPower;
    public float attackSpeed;
    public float bulletSpeed;
    
    [Header("추적")]
    public Transform followPos; // 플레이어 오브젝트
 
    public float followDelay = 0.1f; // 따라오는 시간 차
    public int maxPositions = 50; // 최대 저장할 위치 수

    protected Queue<Vector3> positions;
    protected Transform followerTransform;

    [SerializeField]
    protected List<GameObject> bulletPool = new List<GameObject>();

    public GameObject firePos;

    private void Start()
    {
      firePos.AddComponent<SwordFire>();
    }
    void Update()
    {
       
    }

    protected virtual void SetTrans()
    {
        positions = new Queue<Vector3>();
        followerTransform = transform;

        firePos = GameObject.Find("FirePos");
        firePos.AddComponent <SwordFire>();
        firePos.GetComponent<SwordFire>().magicSword = this;
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
                bullet.GetComponent<BulletController>().damage = attckPower;
                bullet.GetComponent<BulletController>().damageRate = 1f;
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        newBullet.GetComponent<BulletController>().damage = attckPower;
        newBullet.GetComponent<BulletController>().damageRate = 1f;
        bulletPool.Add(newBullet);
        return newBullet;
    }

    public virtual void Fire()
    {
        GameObject bullet = GetBullet();
     
        bullet.GetComponent<BulletController>().isSubBullet = true;
        bullet.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * bulletSpeed;


    }

}
