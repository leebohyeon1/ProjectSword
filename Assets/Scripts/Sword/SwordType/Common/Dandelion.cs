using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dandelion : MagicSword, IListener
{
    private float timer = 0f;
    private DandelionSkill skill;

    [Header("고유 능력 진화")]
    [Header("레벨 1")]
    public int damageUp = 0;
    [Header("레벨 2")]
    public float damageDownRate = 0.5f;
    [Header("레벨 3")]
    public Vector2 areaSize;
    public float duration;
    public int damage;
    public float Interval;
    [Header("레벨 4")]
    public float durationUp;
    public float IntervalDown;
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

        if (Input.GetMouseButtonUp(0))
        {
            buffLevel++;
        }
    }

    public void OnEvent(EVENT_TYPE Event_type, Component Sender, object Param = null)
    {
        SubDanSkill((Vector3)Param);
    }

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

    public void Attack()
    {
        timer += Time.deltaTime;
        if (timer > attackSpeed)
        {
            Fire();
            timer = 0f;
        }
    }

    public override void SetFire()
    {
        base.SetFire();
    }

    protected override void ApplyBuffEffects()
    {
        switch (buffLevel)
        {
            case 0:
                
                break;
            case 1:
                Buff1();
                break;
            case 2:
                Buff2();
                break;
            case 3:
   
                break;
            case 4:
            
                break;
        }
    }

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
            bullets.isDandelion = true;

        }
    }

    private void Buff3()
    {
        foreach (GameObject bullet in bulletPool)
        {
            DandelionBullet bullets = bullet.GetComponent<DandelionBullet>();
            bullets.isLevel3 = true;
        }
    }


    public void SubDanSkill(Vector3 pos)
    {
        GameObject panel = Instantiate(skill.Panel,pos , Quaternion.identity);
        panel.transform.localScale = areaSize;

        DandelionField dandelionField = panel.GetComponent<DandelionField>();
        dandelionField.duration = duration;
        dandelionField.damageAmount = damage;
        dandelionField.damageInterval = Interval;
    }
}