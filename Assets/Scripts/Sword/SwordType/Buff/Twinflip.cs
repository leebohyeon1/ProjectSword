using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinflip : MagicSword
{
    //private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        SetTrans();
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    public override void SetTrans()
    {
        base.SetTrans();
    }

    protected override void Follow()
    {
        base.Follow();
    }

}
