using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStat : MonoBehaviour,IListener
{
    [Header("ü��")]
    public int curHp = 50;

    [Header("����")]
    public Swords[] weapon;
    private List<GameObject> magicSwords;
    public int weaponIndex;
    public Transform[] swordPos;

    [Header("����")]
    public int attackdamage = 1;
    public float attackSpeed = 1f;

    [Header("��ų")]
    public int skillDamage;
    public float skillCool;
    public int skillCount;

    [Header("����")]
    public int swapCount;

    [Header("ź��")]
    public GameObject bulletPrefab;
    public int poolSize = 20;
    public float bulletSpeed;

    private List<GameObject> bulletPool;
    private List<GameObject> TrashPool;

    //====================================================

    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.CHANGE_WEAPON, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SKILL_COUNT,this);
        EventManager.Instance.AddListener(EVENT_TYPE.SWAP_COUNT,this);
       
        TrashPool = new List<GameObject>();

        InitializePool();

        SpawnSwords();

        SetWeapon();            
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.CHANGE_WEAPON:
                ChangeWeapon();
                break;
            case EVENT_TYPE.SKILL_COUNT:
                EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_ON, this,skillCount);
                break;
            case EVENT_TYPE.SWAP_COUNT:
                EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_ON, this,swapCount);
                break;
                
        }

    }
    //====================================================

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

    void InitializePool()
    {
        bulletPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public void ChangeWeapon()
    {
        weaponIndex = (weaponIndex + 1) % 2;

        foreach (GameObject bullet in TrashPool)
        {
            Destroy(bullet);
        }
     
        TrashPool = new List<GameObject>();
        SetWeapon();

        // ���� Ǯ�� �Ѿ��� ��� �����ϰ� �� �Ѿ� ���������� Ǯ�� �ٽ� �ʱ�ȭ
        foreach (GameObject bullet in bulletPool)
        {  
            if (!bullet.activeInHierarchy)
            {            
                Destroy(bullet);
            }
            else
            {
                TrashPool.Add(bullet);
            }
        }

        InitializePool();
        EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_ON, this,  skillCount);
        EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_ON, this, swapCount);
    }

    void SetWeapon()
    {
        attackdamage = weapon[weaponIndex].attackdamage;
        attackSpeed = weapon[weaponIndex].attackSpeed;
        skillDamage = weapon[weaponIndex].skillDamage;
        skillCool = weapon[weaponIndex].skillCool;
        skillCount = weapon[weaponIndex].skillCount;
        swapCount = weapon[weaponIndex].swapCount;
        bulletPrefab = weapon[weaponIndex].bulletPrefab;
        bulletSpeed = weapon[weaponIndex].bulletSpeed;
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        Debug.Log("���� ü��: " + curHp);
        GetComponent<PlayerUI>().UpdateHp(curHp);
        if (curHp <= 0)
        {
            curHp = 0;
            Destroy(gameObject);
        }
    }

    public void SpawnSwords()
    {
        for (int i = 0; i < weapon.Length; i++)
        {         
            GameObject magicSword = Instantiate(weapon[i].swordPrefab, swordPos[i].position, Quaternion.identity);
            magicSword.GetComponent<MagicSword>().followPos = swordPos[i];
        }
    }


}
