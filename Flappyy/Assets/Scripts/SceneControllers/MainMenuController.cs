using Assets.Scripts.UIs;
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
