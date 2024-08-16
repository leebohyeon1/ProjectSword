using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("일시 정지")]
    [SerializeField] private bool isPause = false;

    [Header("스킬")]
    [SerializeField] private bool isSkill;
    [SerializeField] private float skillTimeScale;

    [Header("상태")]
    [SerializeField] private bool isTwinflipLevel3;
    [SerializeField] private bool isTidebiteLevel3;
    [SerializeField] private bool isTidebiteLevel4;
    [SerializeField] private bool isLegendaryQuest;
    public int flowerLevel = 0;

    private int[] TwinflipDamage;
    public int[] TwinDam { get { return TwinflipDamage; } }

    private float[] TwinflipDistance;
    public float[] TwinDis { get { return TwinflipDistance; } }

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

    public void SetTwinflip3(bool twinflip, ref int[] dam, ref float[] dis)
    {
        isTwinflipLevel3 = twinflip;
        TwinflipDamage = new int[dam.Length];
        TwinflipDamage = dam;

        TwinflipDistance = new float[dis.Length];
        TwinflipDistance = dis;
    }

    public void SetTidebiteLevel3(bool boolean)
    {
        isTidebiteLevel3 = boolean;
    }
    public void SetTidebiteLevel4(bool boolean)
    {
        isTidebiteLevel4 = boolean;
    }
    public void SetLegendaryQuest(bool boolean)
    {
        isLegendaryQuest = boolean;
    }

    public bool GetTwinflip3() => isTwinflipLevel3;
    public bool GetLegendaryQuest() => isLegendaryQuest;
    public bool GetTidebite3() => isTidebiteLevel3;
    public bool GetTidebite4() => isTidebiteLevel4;
    //==================================================================================

    private void OnApplicationPause(bool pause) //플레이어가 다른 창으로 넘어갔을 때와 같은 플레이 중이 아닌 경우
    {
        SetPause(pause);
    }
}
