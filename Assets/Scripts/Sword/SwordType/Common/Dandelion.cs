using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dandelion : MagicSword, IListener
{
    private float timer = 0f;
    private DandelionSkill skill;

    [Header("고유 능력 진화")]

    [Header("레벨 1")]
    [SerializeField] private int damageUp = 0;

    [Header("레벨 2")]
    [SerializeField] private float damageDownRate = 0.5f;

    [Header("레벨 3")]
    private bool isLevel3 = false;
    [SerializeField] private Vector2 areaSize;
    [SerializeField] private float duration;
    [SerializeField] private int damage;
    [SerializeField] private float Interval;

    [Header("레벨 4")]
    [SerializeField] private float durationUp;
    [SerializeField] private float IntervalDown;

    //================================================================================================

    void Start()
    {
        SetTrans();

        SetFire();
        InitializePool();
        skill = GetComponent<DandelionSkill>();
        
        EventManager.Instance.AddListener(EVENT_TYPE.DAN3,this);
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
        Attack();
    }

    public void OnEvent(EVENT_TYPE Event_type, Component Sender, object Param = null)
    {
        if (isLevel3)
        {
            SubDanSkill((Vector3)Param);
        }
    }

    //================================================================================================


    private void Buff1()
    {
        plusAttackPower += damageUp;
    }

    private void Buff2()
    {
        foreach (GameObject bullet in bulletPool)
        {
            DandelionBullet bullets = bullet.GetComponent<DandelionBullet>();
            bullets.SetDamagebuff(damageDownRate);
            bullets.SetDandelionSkillB (true);

        }
    }

    private void Buff3()
    {
        isLevel3 = true;
        foreach (GameObject bullet in bulletPool)
        {
            DandelionBullet bullets = bullet.GetComponent<DandelionBullet>();
            bullets.SetBuffLevel3(true);
        }
    }

    private void Buff4()
    {
        duration += durationUp;
        Interval -= IntervalDown;
    }

    public void SubDanSkill(Vector3 pos)
    {
        GameObject panel = Instantiate(skill.panel,pos , Quaternion.identity);
        panel.transform.localScale = areaSize;

        DandelionField dandelionField = panel.GetComponent<DandelionField>();
        dandelionField.SetField(duration, Interval, damage);

    }

    public void Attack()
    {
        timer += Time.deltaTime;
        if (timer > attackSpeed)
        {
            Fire();
            timer = 0f;
        }
    }

    //================================================================================================

    public override void SetTrans()
    {
        base.SetTrans();
    }

    protected override void Follow()
    {
        base.Follow();
    }

    protected override void InitializePool()
    {
        base.InitializePool();
    }

    public override void Fire()
    {
        base.Fire();
    }

    public override void SetFire()
    {
        base.SetFire();
    }
    public override GameObject GetBulletPrefab() => base.GetBulletPrefab();

    public override void SetSword(Transform Trans, int AttackPower, float AttackSpeed, float BulletSpeed)
    {
        base.SetSword(Trans, AttackPower, AttackSpeed, BulletSpeed);
    }

    protected override void ApplyBuffEffects()
    {
        switch (buffLevel)
        {
            case 1:
                Buff1();
                break;
            case 2:
                Buff2();
                break;
            case 3:
                Buff3();
                break;
            case 4:
                Buff4();
                break;
        }
    }

}