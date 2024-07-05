using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideSword : MagicSword
{
    // Start is called before the first frame update
    void Start()
    {
        Set();
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    protected override void Set()
    {
        base.Set();
    }

    protected override void Follow()
    {
        base.Follow();  
    }
}
