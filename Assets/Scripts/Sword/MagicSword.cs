using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MagicSword : MonoBehaviour
{

    [Header("ź��")]
    public GameObject bullet;
    public Transform bulletPoolTrans;
    public int poolSize = 20;
  

    [Header("�ൿ")]
    public int attckPower;
    public float attackSpeed;
    public float bulletSpeed;
    
    [Header("����")]
    public Transform followPos; // �÷��̾� ������Ʈ
 
    public float followDelay = 0.1f; // ������� �ð� ��
    public int maxPositions = 50; // �ִ� ������ ��ġ ��

    protected Queue<Vector3> positions;
    protected Transform followerTransform;

    [SerializeField]
    protected List<GameObject> bulletPool = new List<GameObject>();

    void Update()
    {
       
    }

    protected virtual void SetTrans()
    {
        positions = new Queue<Vector3>();
        followerTransform = transform;
    }

    protected virtual void Follow()
    {

        if (positions.Count > maxPositions)
        {
            positions.Dequeue(); // ť�� ���� �� ��� ���� ������ ��ġ ����
        }

        positions.Enqueue(followPos.position);

        // ��ġ�� ���� �ð� �̻� ť�� ����Ǿ��� �� ���󰡵��� ó��
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
            GameObject bullet = Instantiate(this.bullet);
            bullet.SetActive(false);
            bullet.transform.SetParent(bulletPoolTrans, true);
            bulletPool.Add(bullet);
        }
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

        GameObject newBullet = Instantiate(bullet);
        newBullet.SetActive(false);
        newBullet.GetComponent<BulletController>().damage = attckPower;
        newBullet.GetComponent<BulletController>().damageRate = 1f;
        bulletPool.Add(newBullet);
        return newBullet;
    }

    public virtual void Fire()
    {
        GameObject bullet = GetBullet();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.GetComponent<BulletController>().isSubBullet = true;
        bullet.SetActive(true);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = Vector2.up * bulletSpeed;

    }

}
