using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonSword : MagicSword
{
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        SetTrans();
        InitializePool();
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
        Attack();
    }

    protected override void SetTrans()
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
   
}
