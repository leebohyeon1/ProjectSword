using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("½ºÅ³")]
    [SerializeField]
    private bool isSkill;
    public float skillTimeScale;

    [SerializeField]
    private int monsterCount;
    public int monCount 
    {
        get { return monsterCount; } 
        set 
        {
            monsterCount = value;
            EventManager.Instance.PostNotification(EVENT_TYPE.KILL_MON,this,monsterCount);
        } 
    }

    //==================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        monsterCount = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void SkillOnOff()
    {
        isSkill = !isSkill;

        if (isSkill)
        {
            Time.timeScale = skillTimeScale;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public bool GetSkillBool()
    {
        return isSkill;
    }
}
