
using UnityEngine;

public class Tidebite : MagicSword
{
    private float timer = 0f;

    [Header("고유 능력 진화")]

    [Header("레벨 1")]
    [SerializeField] private int damageUp = 10;

    [Header("레벨 2")]
    [SerializeField] private int molarDamage = 5;


    [Header("레벨 4")]
    [SerializeField] private int molarDamageUp = 2;

    //================================================================================================

    void Start()
    {
        SetTrans();

        SetFire();
        InitializePool();
    }

    void Update()
    {
        Follow();
        Attack();

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
           TidebiteBullet bullets = bullet.GetComponent<TidebiteBullet>();
            //bullets.SetDamagebuff(damageDownRate);
            bullets.SetMolar(true);
            bullets.SetMolar(1);
            bullets.SetMolarDamage(molarDamage);

        }
    }

    private void Buff3()
    {
        foreach (GameObject bullet in bulletPool)
        {
            TidebiteBullet bullets = bullet.GetComponent<TidebiteBullet>();
            bullets.SetMolar(2);
        }
    }

    private void Buff4()
    {
        molarDamage += molarDamageUp;
    }


    public void Evoltion1() { }
    public void Evoltion2() { }
    public void Evoltion3() { }
    public void Evoltion4() { }
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

    protected override void ApplyEvolutionEffects()
    {
        switch (evolutionLevel)
        {
            case 1:
                Evoltion1();
                break;
            case 2:
                Evoltion2();
                break;
            case 3:
                Evoltion3();
                break;
            case 4:
                Evoltion4();
                break;
        }
    }

}