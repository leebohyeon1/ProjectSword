using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("체력")]
    public int curHp = 50;

    [Header("공격")]
    public int attackdamage = 1;
    public float attackSpeed = 1f;

    [Header("스킬")]
    public int skillDamage;
    public float skillCool;

    [Header("탄막")]
    public GameObject bulletPrefab;
    public int poolSize = 20;
    private List<GameObject> bulletPool;
    public float bulletSpeed;


    private void Start()
    {
        bulletPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.GetComponent<BulletController>().damage = attackdamage;
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        newBullet.GetComponent<BulletController>().damage = attackdamage;
        bulletPool.Add(newBullet);
        return newBullet;
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        Debug.Log("현재 체력: " +  curHp);    
        if(curHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
