using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Wind,
    Ice,
    Fire,
    Water,
    Thunder
}

public class PlayerStat : MonoBehaviour, IListener
{
    [Header("체력")]
    public float maxHp = 100f;
    public float curHp;

    [Header("무기")]
    public Swords[] weapon;
    private List<GameObject> weaponList = new List<GameObject>();
    public int weaponIndex;
    public Transform[] swordPos;

    [Header("공격")]
    public int attackDamage = 1;
    public float attackSpeed = 1f;
    public GameObject firePos;
    public float criticalRate = 0f;
    public float criticalDamage = 150f;

    [Header("스킬")]
    public float skillCool;
    public float maxSkillCount = 100f;
    private float skillCount_;
    private float skillCost;
    public float maxExtraSkillCount = 150f;
    private float extraSkillCount;
    private float skillTimer;
    [SerializeField] private Vector2[] skillSize = new Vector2[2];

    public float skillCount
    {
        get => skillCount_;
        set
        {
            skillCount_ = Mathf.Min(value, maxSkillCount);
         
            if (skillCount_ == maxSkillCount)
            {
                if (extraSkillCount > 0) return;
                float excess = value - maxSkillCount;
                extraSkillCount = Mathf.Min(extraSkillCount + excess, maxExtraSkillCount);
            }
        }
    }

    [Header("스왑")]
    public float maxSwapCount = 100f;
    private float swapCount_;
    public int maxKeepSwap = 4;
    private int keepSwap;
    public float swapCool = 5f;
    private float swapTimer;

    public float swapCount
    {
        get => swapCount_;
        set
        {
            if (keepSwap != maxKeepSwap)
            {
                swapCount_ = Mathf.Min(value, maxSwapCount);
                if (swapCount_ >= maxSwapCount)
                {
                    swapCount_ = 0f;
                    keepSwap = Mathf.Min(keepSwap + 1, maxKeepSwap);
                    EventManager.Instance.PostNotification(EVENT_TYPE.KEEP_SWAP, this, keepSwap);
                }
            }
        }
    }


    [Header("탄막")]
    public GameObject bulletPrefab;
    public Transform bulletParent;
    public int poolSize = 20;
    public float bulletSpeed;
    public BulletType bulletType;
    private List<GameObject> bulletPool = new List<GameObject>();
    private List<GameObject> TrashPool = new List<GameObject>();

    public List<GameObject> bulletPool_
    {
        get { return bulletPool; }
        set { bulletPool = value; }
    }

    [Header("업그레이드")]
    public int[] upAttackDamage = new int[2];
    public float[] upAttackSpeed = new float[2];
    public float[] upBulletSpeed = new float[2];
    public float[] upSkillDamage = new float[2];
    public int[] skillBuff = new int[2];
    public int[] swapBuff = new int[2];
    public float skillCoolDown = 0f;
    public float swapDamage = 0f;

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

        if (keepSwap >= maxKeepSwap)
            EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, 1f);
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
                    if (keepSwap < maxKeepSwap)
                    {
                        swapCount += (float)Param;
                        EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, swapCount / maxSwapCount);
                    }
                }
                break;
        }
    }

    //==================================================================================

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
        EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, swapCount / maxSwapCount);
        EventManager.Instance.PostNotification(EVENT_TYPE.KEEP_SWAP, this, keepSwap);
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
        curHp = Mathf.Min(curHp + heal, maxHp);

        GetComponent<PlayerUI>().UpdateHp(curHp, maxHp);
    }

    public void UseSkill(int skillIndex)
    {
        skillCount -= skillCost;
        skillTimer = 0f;
        if (extraSkillCount > 0f)
        {
            CalculationSkillCount();
        }
        weaponList[weaponIndex].GetComponent<SwordSkill>().Skill(skillIndex);
        EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT, this, skillCount / maxSkillCount);
    }

    public void Drain(int damage)
    {
        int heal = damage <= damageQuarter[0] ? drainAmount[0] :
                    damage <= damageQuarter[1] ? drainAmount[1] : drainAmount[2];

        curHp = Mathf.Min(curHp + heal, maxHp);
        GetComponent<PlayerUI>().UpdateHp(curHp, maxHp);

    }

    public void AutoRecovery()
    {
        if (hpReTimer > 0.1f)
        {
            hpReTimer = 0;

            curHp += hpRecoveryAmount;
            if (curHp >= maxHp)
            {
                curHp = maxHp;
            }

            GetComponent<PlayerUI>().UpdateHp(curHp, maxHp);
        }

        if (skillReTimer > 0.1f)
        {
            skillReTimer = 0;

            skillCount += skillRecoveryAmount;
            EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT, this, skillCount / maxSkillCount);
        }

        if (swapReTimer > 0.1f)
        {
            swapReTimer = 0;

            swapCount += swapRecoveryAmount;
            EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this, swapCount / maxSwapCount);
        }

        hpReTimer += Time.deltaTime;
        skillReTimer += Time.deltaTime;
        swapReTimer += Time.deltaTime;
    }

    private int CalculateDamage(int baseDamage)
    {
        bool isCritical = Random.Range(0f, 100f) < criticalRate;
        return isCritical ? Mathf.RoundToInt(baseDamage * (1 + criticalDamage / 100f)) : baseDamage;
    }

    private void CalculationSkillCount()
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

    #region GetFunc

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

        var newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        InitializeBullet(newBullet);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    public Swords GetSwords() => weapon[weaponIndex];

    public List<GameObject> GetBulletPool() => bulletPool;
    #endregion

    #region SetFunc

    void SetWeapon()
    {
        var currentWeapon = weapon[weaponIndex];
        attackDamage = currentWeapon.attackdamage;
        attackSpeed = currentWeapon.attackSpeed;
        skillCool = currentWeapon.skillCool;
        skillCost = currentWeapon.skillCost;
        bulletPrefab = currentWeapon.swordPrefab.GetComponent<MagicSword>().bulletPrefab;
        bulletSpeed = currentWeapon.bulletSpeed;
        bulletType = currentWeapon.bulletType;

        for (int i = 0; i < skillSize.Length; i++)
        {
            skillSize[i] = weapon[weaponIndex].swordPrefab.GetComponent<SwordSkill>().skillSize[i];
        }

        GameUIManager.Instance.skillProfile.sprite = weapon[weaponIndex].skillImage;
        GameUIManager.Instance.swapProfile[0].sprite = weapon[0].skillImage;
        GameUIManager.Instance.swapProfile[1].sprite = weapon[1].skillImage;
    }

    public Vector2 SetWeaponSize(int i) => skillSize[i];

    public void SetSKillTrans(Transform skillTransform) => weaponList[weaponIndex].GetComponent<SwordSkill>().SetTrans(skillTransform);

    public void SetBulletIce(float rate)
    {
        foreach (var bullet in bulletPool)
        {
            var controller = bullet.GetComponent<BulletController>();
            controller.isIce = !controller.isIce;
            controller.slowRate = rate;
        }
    }

    private void InitializeBullet(GameObject bullet)
    {
        var controller = bullet.GetComponent<BulletController>();
        controller.damage = CalculateDamage(attackDamage + upAttackDamage[weaponIndex]);
        controller.bulletType = bulletType;
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

    public void SpawnSwords()
    {
        for (int i = 0; i < weapon.Length; i++)
        {
            var swordInstance = Instantiate(weapon[i].swordPrefab, swordPos[i].position, Quaternion.identity);
            var magicSword = swordInstance.GetComponent<MagicSword>();
            magicSword.followPos = swordPos[i];
            magicSword.attackPower = weapon[i].swordAttackPower;
            magicSword.attackSpeed = weapon[i].swordAttackSpeed;
            magicSword.bulletSpeed = weapon[i].swordBulletSpeed;
            weaponList.Add(swordInstance);
        }
    }

    #endregion

    #region Upgrade

    public void Upgrade(Enchant enchant)
    {
        ApplyUpgrade(enchant, enchant.isMain ? 0 : 1);

        canDrain = enchant.isDrain;
        hpRecoveryAmount += enchant.hpRecovery;
        skillCoolDown += enchant.skillCoolDown;
        swapDamage += enchant.swapDamage;

        foreach (var weapon in weapon)
        {
            weapon.swordPrefab.GetComponent<SwordSkill>().skillDamageUp += upSkillDamage[weaponIndex];
        }

        HealHp(enchant.hpUp);
    }

    private void ApplyUpgrade(Enchant enchant, int index)
    {
        upAttackDamage[index] += enchant.attackDamage;
        upAttackSpeed[index] += enchant.attackSpeed;
        upBulletSpeed[index] += enchant.bulletSpeed;
        upSkillDamage[index] += enchant.skillDamage;
        skillBuff[index] += enchant.skillBuff;
        swapBuff[index] += enchant.swapBuff;
        weaponList[index].GetComponent<MagicSword>().buffLevel += enchant.petUpgrade;
    }
    #endregion
}