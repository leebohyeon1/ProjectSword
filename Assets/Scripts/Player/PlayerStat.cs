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
    public int attackDamage = 1;
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

    [Header("업그레이드")]
    public int[] upAttackDamage = new int[2];
    public float[] upAttackSpeed = new float[2];
    public float[] upBulletSpeed = new float[2];
    public float[] upSkillDamage = new float[2];
    public int[] skillBuff = new int[2];
    public int[] swapBuff = new int[2];
    public float skillCoolDown;

    [Header("흡혈")]
    public bool canDrain = false;
    public int[] damageQuarter;
    public int[] drainAmount;

    [Header("자동 회복")]
    public float hpReTimer = 0f;
    public float hpRecoveryAmount = 0f;

    public float skillReTimer = 0f;
    public float skillRecoveryAmount = 0f; 

    public float swapReTimer = 0f;
    public float swapRecoveryAmount = 0f;
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


        GetComponent<PlayerUI>().UpdateHp(curHp, maxHp);
    }

    void Update()
    {
        skillTimer += Time.deltaTime;
        bool canUseSkill = skillCount_ >= skillCost && skillTimer >= (skillCool - skillCoolDown);
        EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_ON, this, canUseSkill);

        swapTimer += Time.deltaTime;
        bool canUseSwap = keepSwap > 0 && swapTimer >= swapCool;
        EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_ON, this, canUseSwap);

        AutoRecovery();


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
                bullet.GetComponent<BulletController>().damage = (attackDamage + upAttackDamage[weaponIndex]);
                bullet.GetComponent<BulletController>().damageRate = 1f;
                return bullet;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        newBullet.GetComponent<BulletController>().damage = (attackDamage + upAttackDamage[weaponIndex]);
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
        attackDamage = currentWeapon.attackdamage;
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
        GameUIManager.Instance.swapProfile[0].sprite = weapon[0].skillImage;
        GameUIManager.Instance.swapProfile[1].sprite = weapon[ 1].skillImage;
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

    public void Upgrade(Enchant enchant)
    {
        if (enchant.isMain)
        {
            upAttackDamage[0] += enchant.attackDamage;
            upAttackSpeed[0] += enchant.attackSpeed;
            upBulletSpeed[0] += enchant.bulletSpeed;
            upSkillDamage[0] += enchant.skillDamage;
            skillBuff[0] += enchant.skillBuff;
            swapBuff[0] += enchant.swapBuff;
        }

        if (enchant.isSub)
        {
            upAttackDamage[1] += enchant.attackDamage;
            upAttackSpeed[1] += enchant.attackSpeed;
            upBulletSpeed[1] += enchant.bulletSpeed;
            upSkillDamage[1] += enchant.skillDamage;
            skillBuff[1] += enchant.skillBuff;
            swapBuff[1] += enchant.swapBuff;
        }

        canDrain = enchant.isDrain;
        hpRecoveryAmount += enchant.hpRecovery;

        for (int i = 0; i < weapon.Length; i++)
        {
            weapon[weaponIndex].swordPrefab.GetComponent<SwordSkill>().skillDamageUp += upSkillDamage[i];
        }

        HealHp(enchant.hpUp);
    }

    public void Drain(int Damage)
    {
        int heal = 0;
        if(Damage <= damageQuarter[0])
        {
            heal = drainAmount[0];
        }
        else if(Damage <= damageQuarter[1]) 
        {
            heal = drainAmount[1];
        }
        else
        {
            heal = drainAmount[2];
        }

        curHp += heal;
        if (curHp >= maxHp)
        {
            maxHp = curHp;
        }

        GetComponent<PlayerUI>().UpdateHp(curHp, maxHp);
    }

    public void AutoRecovery()
    {
        hpReTimer += Time.deltaTime;
        skillReTimer += Time.deltaTime;
        swapReTimer += Time.deltaTime;

        if (hpReTimer > 0.1f)
        {
            hpReTimer = 0;

            curHp += hpRecoveryAmount;
            if(curHp >= maxHp)
            {
                curHp =  maxHp; 
            }

            GetComponent<PlayerUI>().UpdateHp(curHp, maxHp);
        }

        if(skillReTimer > 0.1f)
        {
            skillReTimer = 0;

            skillCount += skillRecoveryAmount;
            EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT, this, skillCount / maxSkillCount);
        }

        if(swapReTimer > 0.1f)
        {
            swapReTimer = 0;

            swapCount += swapRecoveryAmount;
            EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, swapCount / maxSwapCount);
        }    

    }
}


