using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionSkill : SwordSkill
{
    [Header("½ºÅ³ A")]
    public int damage = 1;
    public float damageCycle = 1f;
    public float totalTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Skill(int index)
    {
        if(index == 0)
        {

        }
        else
        {

        }
    }
    public override Vector2 ChangeSkill(int index)
    {
        return skillSize[index];
    }
}
