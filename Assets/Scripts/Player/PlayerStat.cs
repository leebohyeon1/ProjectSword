using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
        get => skillCount_;
        set 
        {
            skillCount_ = Mathf.Min(value, maxSkillCount);
            if (skillCount_ == maxSkillCount)
            {
                extraSkillCount = Mathf.Min(extraSkillCount + (value - maxSkillCount), maxExtraSkillCount);
            }         
        }
    }
    [SerializeField]
    private float skillCount_ = 0f;
    private float skillCost;
    public float maxExtraSkillCount = 150f;
    [SerializeField]
    private float extraSkillCount = 0f;
    private float skillTimer;

    [Header("스왑")]
    [SerializeField]
    public float maxSwapCount = 100f;
    private float swapCount_ = 0f;
    public float swapCount
    {
        get => swapCount_;
        set
        {
            swapCount_ = Mathf.Min(value, maxSwapCount);
            if (keepSwap == maxkeepSwap)
            {
                swapCount = 0f;
            }
            if (swapCount_ >= maxSwapCount)
            {
                swapCount = 0f;
                keepSwap += keepSwap < maxkeepSwap ? 1 : 0;
                EventManager.Instance.PostNotification(EVENT_TYPE.KEEP_SWAP,this,keepSwap);
            }
        }
    }
    public int maxkeepSwap = 4;
    [SerializeField]
    private int keepSwap = 0;
    public float swapCool = 5f;
    private float swapTimer = 0f;

    [Header("탄막")]
    public GameObject bulletPrefab;
    public Transform bulletParent;
    public int poolSize = 20;
    public float bulletSpeed;

    private List<GameObject> bulletPool = new List<GameObject>();
    private List<GameObject> TrashPool = new List<GameObject>();
    //==================================================================================

    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.CHANGE_WEAPON, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SKILL_COUNT, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SWAP_COUNT, this);

        InitializePool();
        SpawnSwords();
        SetWeapon();
    }

    void Update()
    {
        skillTimer += Time.deltaTime;
        bool canUseSkill = skillCount_ >= skillCost && skillTimer >= skillCool;
        EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_ON, this, canUseSkill);

        swapTimer += Time.deltaTime;
        bool canUseSwap = keepSwap > 0 && swapTimer >= swapCool;
        EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_ON, this, canUseSwap);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.CHANGE_WEAPON:
                ChangeWeapon();
                break;
            case EVENT_TYPE.SKILL_COUNT:
                if (Sender != this)
                {
                    skillCount += (float)Param;
                    EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT, this, skillCount / maxSkillCount);
                }
                break;
            case EVENT_TYPE.SWAP_COUNT:
                if (Sender != this)
                {
                    swapCount += (float)Param;

                    if(keepSwap < maxkeepSwap)
                    {
                        EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, swapCount / maxSwapCount);
                    }
                    else
                    {
                        EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, 1f);
                    }
                }
                break;
        }
    }
    //==================================================================================

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
        bulletPool.Clear();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullet.transform.SetParent(bulletParent, true);
            bulletPool.Add(bullet);
        }
    }

    public void ChangeWeapon()
    {
        weaponIndex = (weaponIndex + 1) % weapon.Length;

        foreach (GameObject bullet in TrashPool)
        {
            Destroy(bullet);
        }
        TrashPool.Clear();

        SetWeapon();

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

        keepSwap--;
        swapTimer = 0f;
        InitializePool();

        EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT, this, skillCount / maxSkillCount);
        EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, swapCount/ maxSwapCount);
        EventManager.Instance.PostNotification(EVENT_TYPE.KEEP_SWAP, this, keepSwap);
    }

    void SetWeapon()
    {
        var currentWeapon = weapon[weaponIndex];
        attackdamage = currentWeapon.attackdamage;
        attackSpeed = currentWeapon.attackSpeed;
        skillDamage = currentWeapon.skillDamage;
        skillCool = currentWeapon.skillCool;
        skillCost = currentWeapon.skillCost;
        bulletPrefab = currentWeapon.bulletPrefab;
        bulletSpeed = currentWeapon.bulletSpeed;
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
            var swordInstance = Instantiate(weapon[i].swordPrefab, swordPos[i].position, Quaternion.identity);
            var magicSword = swordInstance.GetComponent<MagicSword>();
            magicSword.followPos = swordPos[i];
            magicSword.ActPower = weapon[i].swordActPower;
            magicSword.ActSpeed = weapon[i].swordActSpeed;
        }
    }

    public void UseSkill()
    {
        Debug.Log("스킬 사용");
        skillCount -= skillCost;
        skillTimer = 0;
        if (extraSkillCount > 0)
        {
            CalculationCount();
        }

        EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT, this, skillCount / maxSkillCount);
    }

    void CalculationCount()
    {
        float countNeeded = maxSkillCount - skillCount;
        if (extraSkillCount > countNeeded)
        {
            skillCount += countNeeded;
            extraSkillCount -= countNeeded;
        }
        else
        {
            skillCount += extraSkillCount;
            extraSkillCount = 0;
        }
    }
}


