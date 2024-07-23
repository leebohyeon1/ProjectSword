using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("일시 정지")]
    public bool isPause = false;

    [Header("스킬")]
    [SerializeField]
    private bool isSkill;
    public float skillTimeScale;
    //==================================================================================


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
    }
    //==================================================================================

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

    public void SetPause(bool pause)
    {
        isPause = pause;
        if(isPause)
        {
            Time.timeScale = 0f;
            Debug.Log("일시정지");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("게임재개");
        }
    }

    private void OnApplicationPause(bool pause) //플레이어가 다른 창으로 넘어갔을 때와 같은 플레이 중이 아닌 경우
    {
        SetPause(pause);
    }
}
