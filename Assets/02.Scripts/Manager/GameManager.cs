using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("�Ͻ� ����")]
    [SerializeField] private bool isPause = false;

    [Header("��ų")]
    [SerializeField] private bool isSkill;
    [SerializeField] private float skillTimeScale;

    [Header("����")]
    [SerializeField] private bool isTwinflipLevel3;
    [SerializeField] private bool isTwinflipLevel4;

    [SerializeField] private bool isTidebiteLevel3;
    [SerializeField] private bool isTidebiteLevel4;


    [SerializeField] private bool isTenkaiLevel1;
    [SerializeField] private bool isTenkaiLevel2;
    [SerializeField] private bool isTenkaiLevel3;


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
            Debug.Log("�Ͻ�����");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("�����簳");
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
    public void SetTwinflip4(bool twinflip)
    {
        isTidebiteLevel4 = twinflip;
    }

    public void SetTidebiteLevel3(bool boolean)
    {
        isTidebiteLevel3 = boolean;
    }
    public void SetTidebiteLevel4(bool boolean)
    {
        isTidebiteLevel4 = boolean;
    }

    public void SetTenkaiLevel1(bool boolean)
    {
        isTenkaiLevel1 = boolean;
    }
    public void SetTenkaiLevel2(bool boolean)
    {
        isTenkaiLevel2 = boolean;
    }
    public void SetTenkaiLevel3(bool boolean)
    {
        isTenkaiLevel3 = boolean;
    }

    public void SetLegendaryQuest(bool boolean)
    {
        isLegendaryQuest = boolean;
    }

    public bool GetTwinflip3() => isTwinflipLevel3;
    public bool GetTwinflip4() => isTwinflipLevel4;

    public bool GetLegendaryQuest() => isLegendaryQuest;
    public bool GetTidebite3() => isTidebiteLevel3;
    public bool GetTidebite4() => isTidebiteLevel4;
    public bool GetTenkai(int num)
    {
        switch (num)
        {
            case 1:
                return isTenkaiLevel1;
            case 2:
                return isTenkaiLevel2;
            case 3:
                return isTenkaiLevel3;
            default:
                return false;
        }
    }

    //==================================================================================

    private void OnApplicationPause(bool pause) //�÷��̾ �ٸ� â���� �Ѿ�� ���� ���� �÷��� ���� �ƴ� ���
    {
        SetPause(pause);
    }
}
