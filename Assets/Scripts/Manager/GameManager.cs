using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("��ų")]
    [SerializeField]
    private bool canSkill;
    public float skillTimeScale;

    [SerializeField]
    private int skillCount_;
    public int skillCount 
    {
        get { return skillCount_; } 
        set 
        {
            skillCount_ = value;
            EventManager.Instance.PostNotification(EVENT_TYPE.SKILL_COUNT,this);
        } 
    }

    [Header("����")]

    [SerializeField]
    private int swapCount_;
    public int swapCount
    {
        get { return swapCount_; }
        set
        {
            swapCount_ = value;
            EventManager.Instance.PostNotification(EVENT_TYPE.SWAP_COUNT, this);
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

        skillCount_ = 0;
        swapCount_ = 0;
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
        canSkill = !canSkill;

        if (canSkill)
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
        return canSkill;
    }
}
