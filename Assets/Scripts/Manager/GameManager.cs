using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("�Ͻ� ����")]
    public bool isPause = false;

    [Header("��ų")]
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
            Debug.Log("�Ͻ�����");
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("�����簳");
        }
    }

    private void OnApplicationPause(bool pause) //�÷��̾ �ٸ� â���� �Ѿ�� ���� ���� �÷��� ���� �ƴ� ���
    {
        SetPause(pause);
    }
}
