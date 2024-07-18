using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideSword : MagicSword
{
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

    protected override void SetTrans()
    {
        base.SetTrans();
    }

    protected override void Follow()
    {
        base.Follow();  
    }
}
