using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStat : MonoBehaviour,IListener
{
    [Header("체력")]
    public int curHp = 50;

    [Header("무기")]
    public Swords[] weapon;
    public int weaponIndex;
    public Transform[] swordPos;

    [Header("공격")]
    public int attackdamage = 1;
    public float attackSpeed = 1f;

    [Header("스킬")]
    public int skillDamage;
    public float skillCool;
    public float maxSkillCount = 100f;
    public float skillCount
    {
        get { return skillCount_; }
        set 
        {
            skillCount_ = value; 
            if(skillCount_ > maxSkillCount)
            {
                skillCount_ = maxSkillCount;
                extraSkillCount += skillCount_ - maxSkillCount;
                if (extraSkillCount > maxExtraSkillCount)
                {
                    extraSkillCount = maxExtraSkillCount;
                }               
            }         
        }
    }
    private float skillCount_ = 0f;
    private float skillCost;
    public float maxExtraSkillCount = 150f;
    private float extraSkillCount = 0f;

    [Header("스왑")]
    public float swapCount = 0f;

    [Header("탄막")]
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
                skillCount += (float)Param;
                EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_ON, this, skillCount/maxSkillCount);
                break;
            case EVENT_TYPE.SWAP_COUNT:
                swapCount += (float)Param;
                EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_ON, this,swapCount/100f);
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

        // 기존 풀의 총알을 모두 삭제하고 새 총알 프리팹으로 풀을 다시 초기화
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
        swapCount = 0f;
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
        skillCost = weapon[weaponIndex].skillCost;
        bulletPrefab = weapon[weaponIndex].bulletPrefab;
        bulletSpeed = weapon[weaponIndex].bulletSpeed;
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        Debug.Log("현재 체력: " + curHp);
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
            magicSword.GetComponent<MagicSword>().ActPower = weapon[i].swordActPower;
            magicSword.GetComponent<MagicSword>().ActSpeed = weapon[i].swordActSpeed;
        }
    }

    public void UseSkill()
    {
        Debug.Log("스킬 사용");
        skillCount -= skillCost;
        if (extraSkillCount > 0)
        {
            skillCount += extraSkillCount;
        }
        //weapon[weaponIndex].skill.Skill();
    }

}
