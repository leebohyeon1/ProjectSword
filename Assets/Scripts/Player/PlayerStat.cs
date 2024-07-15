using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerStat : MonoBehaviour,IListener
{
    [Header("체력")]
    public float maxHp = 100f;
    public float curHp ;

    [Header("무기")]
    public Swords[] weapon;
    private List<GameObject> weaponList = new List<GameObject>();
    public int weaponIndex;
    public Transform[] swordPos;

    [Header("공격")]
    public int attackdamage = 1;
    public float attackSpeed = 1f;

    [Header("스킬")]
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
    [SerializeField]
    private Vector2[] skillSize = new Vector2[2];

    [Header("스왑")]
    [SerializeField]
    public float maxSwapCount = 100f;
    private float swapCount_ = 0f;
    public float swapCount
    {
        get => swapCount_;
        set
        {
            if (keepSwap != maxkeepSwap)
            {
                swapCount_ = Mathf.Min(value, maxSwapCount);

               

                if (swapCount_ >= maxSwapCount)
                {
                    swapCount = 0f;
                    keepSwap += keepSwap < maxkeepSwap ? 1 : 0;
                    EventManager.Instance.PostNotification(EVENT_TYPE.KEEP_SWAP, this, keepSwap);
                }
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

    public List<GameObject> bulletPool_
    { 
        get { return bulletPool; }
        set { bulletPool = value; }
    }
    private List<GameObject> bulletPool = new List<GameObject>();
    private List<GameObject> TrashPool = new List<GameObject>();
    //==================================================================================

    void Awake()
    {
        curHp = maxHp;
    }

    private void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.CHANGE_WEAPON, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SKILL_COUNT, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SWAP_COUNT, this);

        SpawnSwords();
        SetWeapon();
        InitializePool();


        //GetComponent<PlayerUI>().UpdateHp(curHp, maxHp);
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
                bullet.GetComponent<BulletController>().damageRate = 1f;
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        newBullet.GetComponent<BulletController>().damage = attackdamage;
        newBullet.GetComponent<BulletController>().damageRate = 1f;
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
        skillCool = currentWeapon.skillCool;
        skillCost = currentWeapon.skillCost;
        bulletPrefab = currentWeapon.swordPrefab.GetComponent<MagicSword>().bullet;
        bulletSpeed = currentWeapon.bulletSpeed;

        for (int i = 0; i < skillSize.Length; i++)
        {
            skillSize[i] = weapon[weaponIndex].swordPrefab.GetComponent<SwordSkill>().skillSize[i];
        }

        GameUIManager.Instance.skillProfile.sprite = weapon[weaponIndex].skillImage;
        GameUIManager.Instance.swapProfile[0].sprite = weapon[weaponIndex].skillImage;
        GameUIManager.Instance.swapProfile[1].sprite = weapon[(weaponIndex + 1) % weapon.Length].skillImage;
    }

    public Vector2 SetWeaponSize(int i)
    {
        return skillSize[i];
    }

    public void TakeDamage(float damage)
    {
        curHp -= damage;

        GetComponent<PlayerUI>().UpdateHp(curHp, maxHp);
        if (curHp <= 0f)
        {
            curHp = 0f;
            Destroy(gameObject);
        }
    }

    public void HealHp(float heal)
    {
        curHp += heal;
        if (curHp >= maxHp)
        {
            maxHp = curHp;
        }

        GetComponent<PlayerUI>().UpdateHp(curHp, maxHp);
    }

    public void SpawnSwords()
    {
        for (int i = 0; i < weapon.Length; i++)
        {
            GameObject swordInstance = Instantiate(weapon[i].swordPrefab, swordPos[i].position, Quaternion.identity);
            MagicSword magicSword = swordInstance.GetComponent<MagicSword>();
            magicSword.followPos = swordPos[i];
            magicSword.ActPower = weapon[i].swordActPower;
            magicSword.ActSpeed = weapon[i].swordActSpeed;
            weaponList.Add(swordInstance);
        }
    }

    public void UseSkill(int skillIndex)
    {
        skillCount -= skillCost;
        skillTimer = 0f;
        if (extraSkillCount > 0f)
        {
            CalculationCount();
        }
        weaponList[weaponIndex].GetComponent<SwordSkill>().Skill(skillIndex);
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
            extraSkillCount = 0f;
        }
    }
    
    public void SetSKillTrans(Transform skillTransform)
    {
        weaponList[weaponIndex].GetComponent<SwordSkill>().SetTrans(skillTransform);
    }

    public Swords GetSwords()
    {
        return weapon[weaponIndex];
    }

    public List<GameObject> GetBulletPool()
    {
        return bulletPool;
    }

}


