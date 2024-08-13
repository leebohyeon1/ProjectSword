using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUIManager : MonoBehaviour
{
    public static MainUIManager Instance { get; private set; }

    [SerializeField] private GameObject[] UI;

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

    void Start()
    {
       

    }

    void Update()
    {
        
    }

    //==================================================================================

    public void StartGameBtn()
    {
        SceneManager.LoadScene(1);
    }

    public void SetUI(int index)
    {
        for(int i = 0; i < UI.Length; i++)
        {
            if(i ==  index)
            {
                UI[i].SetActive(true);
            }
            else
            {
                UI[i].SetActive(false);
            }
        }
    }
}
