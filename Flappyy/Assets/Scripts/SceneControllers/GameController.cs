using Assets.Scripts.UIs;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool IsGameOver;
    public static bool IsGamePaused;

    public GameObject PlayingUI;
    public GameObject PauseMenuUI;
    public GameObject GameOverUI;
    public Text PlayingScoreText;
    public Text GameOverScoreText;
    public Text GameOverBestScoreText;

    public Text DebugText;

    private int score;
    private int bestScore;
    private static float playingBackgroungMusicTime;
    private AudioSource playingBackgroungMusic;

    public int Score
    {
        get { return score; }
        set
        {
            if (value >= 0)
            {
                score = value;
            }
            else
            {
                score = 0;
            }

            PlayingScoreText.text = "Score: " + score;
            StaticVariables.game_Score = score;
            //ObstacleGenerator og = new ObstacleGenerator();
            //PlayingScoreText.text = "Score: " + og.Speed;
        }
    }

    public int BestScore
    {
        get { return bestScore; }

        set
        {
            if (value >= 0)
            {
                bestScore = value;
            }
            else
            {
                bestScore = 0;
            }
            GameOverBestScoreText.text = "Best Score: " + bestScore;
        }
    }

    private int HighScore
    {
        get
        {
            if (mode == 0)
                BestScore = PlayerPrefs.GetInt(MainController.Prefs_CalmKey_HS, MainController.Prefs_HS_DefValue);
            else if (mode == 1)
                BestScore = PlayerPrefs.GetInt(MainController.Prefs_NormalKey_HS, MainController.Prefs_HS_DefValue);
            else if (mode == 2)
                BestScore = PlayerPrefs.GetInt(MainController.Prefs_CrazyKey_HS, MainController.Prefs_HS_DefValue);
            else
                BestScore = PlayerPrefs.GetInt(MainController.Prefs_NormalKey_HS, MainController.Prefs_HS_DefValue);
            return BestScore;
        }

        set
        {
            if (mode == 0)
                PlayerPrefs.SetInt(MainController.Prefs_CalmKey_HS, value);
            else if (mode == 1)
                PlayerPrefs.SetInt(MainController.Prefs_NormalKey_HS, value);
            else if (mode == 2)
                PlayerPrefs.SetInt(MainController.Prefs_CrazyKey_HS, value);
            else
                PlayerPrefs.SetInt(MainController.Prefs_NormalKey_HS, value);
            BestScore = value;
        }
    }

    int mode;

    void Awake()
    {
        //        StaticVariables.resetVariables();

        mode = PlayerPrefs.GetInt(MainController.Prefs_Modes_Key, MainController.Prefs_Modes_DefIndex);
        IsGameOver = false;
        IsGamePaused = false;

        playingBackgroungMusic = GetComponent<AudioSource>();
        playingBackgroungMusic.time = playingBackgroungMusicTime;

        Score = 0;
        StaticVariables.game_Score = Score;

        BestScore = HighScore;


        PlayingUI.SetActive(true);
        GameOverUI.SetActive(false);
        PauseMenuUI.SetActive(false);
    }

    void Start()
    {
        Time.timeScale = 1;
        //TapsellStandardBanner.Hide(); //Uncomment if you want ad
        //InvokeRepeating("addScore", 1f, StaticVariables.scoreRate);
        StartCoroutine(addScore());
    }

    void Update()
    {
        if (!IsGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuToggle();
        }
    }

    private IEnumerator addScore()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            Score += StaticVariables.incrementScoreBy;
            yield return new WaitForSeconds(StaticVariables.scoreRate);
        }

            
    }

    public void GameOver()
    {
        IsGameOver = true;
        Time.timeScale = 0;
        //TapsellStandardBanner.Show(); //Uncomment if you want ad
        //CancelInvoke("addScore");

        if (Score > BestScore)
        {
            HighScore = Score;
        }

        //ColorEffect.ColorIndex++;
        PlayerPrefs.SetInt(MainController.Prefs_ColorIndex_Key, ColorEffect.ColorIndex);
        //ColorEffect.ColorIndex--;

        PlayingUI.SetActive(false);
        GameOverScoreText.text = "SCORE\n" + score;
        GameOverBestScoreText.text = "HIGH SCORE\n" + bestScore;
        GameOverUI.SetActive(true);

        playingBackgroungMusic.Pause();
        playingBackgroungMusicTime = playingBackgroungMusic.time;

        PlayerPrefs.Save();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
        IsGameOver = false;
        IsGamePaused = false;
    }

    public void PauseMenuToggle()
    {
        if (!PauseMenuUI.activeSelf)
        {
            IsGamePaused = true;
            Time.timeScale = 0;
            playingBackgroungMusic.Pause();
            PauseMenuUI.SetActive(true);
        }
        else
        {
            IsGamePaused = false;
            PauseMenuUI.SetActive(false);
            if (PlayerPrefs.GetInt("sounds", 1) == 1)
            {
                playingBackgroungMusic.UnPause();
            }
            Time.timeScale = 1f;
        }
    }
}
