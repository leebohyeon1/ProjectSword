using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoint : MonoBehaviour
{
    public Transform skillMask;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(skillMask.gameObject.activeSelf)
        {
            transform.position = skillMask.position;
        }
    }
       
}
