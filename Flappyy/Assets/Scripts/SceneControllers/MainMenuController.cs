using Assets.Scripts.UIs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject QuitPopupUI;
    public Text BestScoreText;

    void Awake()
    {
        QuitPopupUI.SetActive(false);
        BestScoreText.text = "BEST SCORE\n" + PlayerPrefs.GetInt(MainController.Prefs_BestScore_Key, MainController.Prefs_BestScore_DefaultValue);
    }

    void Start()
    {
        //TapsellStandardBanner.Hide(); //Uncomment if you want ad
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitPopupToggle();
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(2);

        //string mode = readGameMode();
        //if (mode.Equals(mode1))
        //{
        //    SceneManager.LoadScene(2);
        //} 
        //else if (mode.Equals(mode2))
        //{
        //    SceneManager.LoadScene(3);
        //}
        //else if (mode.Equals(mode3))
        //{
        //    SceneManager.LoadScene(4);
        //}else
        //{
        //    Debug.Log(mode);
        //}

    }

    

    private string readGameMode()
    {
        switch (PlayerPrefs.GetInt(MainController.Prefs_Modes_Key, MainController.Prefs_Modes_DefIndex))
        {
            case 0:
                return StaticVariables.mode1;
            case 1:
                return StaticVariables.mode2;
            case 2:
                return StaticVariables.mode3;
            default:
                return "Game Mode Un-Identified";
        }
    }

    public void QuitPopupToggle()
    {
        if (!QuitPopupUI.activeSelf)
        {
            QuitPopupUI.SetActive(true);
        }
        else
        {
            QuitPopupUI.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
