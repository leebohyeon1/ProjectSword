using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject GameUI;

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

        InitialUI();
    }

    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //==================================================================================

    void InitialUI()
    {
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                MainUI.SetActive(true);
                GameUI.SetActive(false);
                break;
            case 1:
                MainUI.SetActive(false);
                GameUI.SetActive(true);
                break;
        }
    }




}
