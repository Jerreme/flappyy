
using Assets.Scripts.ObstacleScripts;
using Assets.Scripts.UIs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleGenerator : MonoBehaviour
{
    public static Queue<GameObject> Obstacles;

    public int PoolSize;
    public float Speed;
    public float Smooth;
    public Vector2 WidthRange;
    public Vector2 HeightRange;

    //public float Delta;
    //public int D;
    public Transform ObstaclesContainer;
    public GameObject Obstacle;

    //private float time;
    protected Vector3 startPos;
    protected GameObject top;
    protected GameObject bottom;

    protected float topHeight;
    protected float topWidth;
    protected float bottomHeight;
    protected float bottomWidth;

    GameController gc;
    ObstacleGenerator_Computer computer;
    public Text InGameScoreText;
    private float broadSpeed;

    protected float topInterval
    {
        get => (topWidth - Smooth / Speed) / Speed;
    }
    private float bottomInterval
    {
        get => (bottomWidth - Smooth / Speed) / Speed;
    }
    // -----------------------------------------------
    private Vector3 topScale
    {
        get => new Vector3(topWidth, topHeight, 1);
    }

    private Vector3 bottomScale
    {
        get => new Vector3(bottomWidth, bottomHeight, 1);
    }
    // -----------------------------------------------
    void Awake()
    {
        startPos = new Vector3(15f, 0f, 0f);
        FillPool();

        //For Debugging purposes
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        computer = new ObstacleGenerator_Computer();
        intializeGameMode();
    }
    
    void Start()
    {
        //intializeGameMode();

        StartCoroutine(topRandGen());
        StartCoroutine(bottomRandGen());
        //StartCoroutine(generator());

         
    }

    private float tempSpeed = 8f;
    private void Update()
    {
        if (StaticVariables.game_Score < 1 && Speed < 18)
        {
            if (Speed < 10)
            {
                Speed = tempSpeed;
                tempSpeed += 0.10f;
            }
            else
            {
                Speed = tempSpeed;
                tempSpeed += 0.50f;
            }
        }
        else if (StaticVariables.game_Score < 3 && Speed > 8)
        {
            tempSpeed -= 1f;
            Speed = tempSpeed;
        }


        StaticVariables.scoreRate = newScoreRate(StaticVariables.game_Score);
    }
    // -----------------------------------------------

    private int GameModeInt;

    private void intializeGameMode()
    {
        int poolsize;
        float speed, smooth;
        Vector2 widthrange, heightrange;

        GameModeInt = PlayerPrefs.GetInt(MainController.Prefs_Modes_Key, MainController.Prefs_Modes_DefIndex);
        switch (GameModeInt)
        {
            case 0: //calm
                poolsize = 50; broadSpeed = 9f; smooth = 5f;
                widthrange.x = 4f; widthrange.y = 5f;
                heightrange.x = 2.4f; heightrange.y = 4.3f;
                maxScoreRate = 0.25f; minScoreRate = 0.8f;
                break;
            case 1: //normal
                poolsize = 50; broadSpeed = 10f; smooth = 5f;
                widthrange.x = 3f; widthrange.y = 4f;
                heightrange.x = 2.5f; heightrange.y = 4.35f;
                maxScoreRate = 0.15f; minScoreRate = 0.6f;
                break;
            case 2: //crazy
                poolsize = 55; broadSpeed = 12f; smooth = 5f;
                widthrange.x = 2f; widthrange.y = 3f;
                heightrange.x = 2.6f; heightrange.y = 4.45f;
                maxScoreRate = 0.1f; minScoreRate = 0.5f;
                break;
            default: //default normal
                poolsize = 50; broadSpeed = 10f; smooth = 5f;
                widthrange.x = 3f; widthrange.y = 4f;
                heightrange.x = 2.5f; heightrange.y = 4.35f;
                maxScoreRate = 0.15f; minScoreRate = 0.6f;
                break;
        }

        speed = broadSpeed;
        setGameValues(poolsize, smooth, widthrange, heightrange);
    }

    private void setGameValues(int poolsize, float smooth,
        Vector2 widthrange, Vector2 heightrange)
    {
        this.PoolSize = poolsize;
        this.Speed = startingSpeed;
        this.Smooth = smooth;
        this.WidthRange = widthrange;
        this.HeightRange = heightrange;

        checkMode(GameModeInt);
    }
    // -----------------------------------------------
    protected void FillPool()
    {
        Obstacles = new Queue<GameObject>();
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject clone = Instantiate(Obstacle, startPos, Quaternion.identity, ObstaclesContainer);
            clone.SetActive(false);
            Obstacles.Enqueue(clone);
        }
    }
    protected GameObject GetObstacle()
    {
        GameObject clone;

        if (Obstacles.Count != 0)
        {
            clone = Obstacles.Dequeue();
        } else {
            FillPool();
            clone = Obstacles.Dequeue();
        }
        
        clone.transform.position = startPos;
        updateSpeed();
        return clone;
    }

    
    protected void updateSpeed()
    {
        if (StaticVariables.game_Score > score_low)
        {
            Speed = newSpeed(StaticVariables.game_Score);
        }
        
        Mover.Speed = Speed;
    }
    // -----------------------------------------------

    // -----------------------------------------------
    protected void updateTopTransform()
    {
        top.transform.localScale = topScale;
        top.transform.position = new Vector3(top.transform.position.x, 5 - top.transform.localScale.y / 2, 0f);
    }

    protected void updateBottomTransform()
    {
        bottom.transform.localScale = bottomScale;
        bottom.transform.position = new Vector3(bottom.transform.position.x, -5 + bottom.transform.localScale.y / 2, 0f);
    }
    //--------------------------------------------------

    private float score_low;
    private float score_high;

    private void checkMode(int gamemodeint)
    {
        if (gamemodeint == 0) // Calm
        {
            score_low = 5f; score_high = 200;
            chances = 8;
        }
        else if (gamemodeint == 1) // Normal
        {
            score_low = 8f; score_high = 300;
            chances = 7;
        }
        else if (gamemodeint == 2) // Crazy
        {
            score_low = 10; score_high = 500;
            chances = 6;
        }
        else
        {
            score_low = 8f; score_high = 400; // Normal
            chances = 7;
        }
    }
    private float newHeightRangeX(int gamemodeint, float difference)
    {
        //checkMode(gamemodeint);
        return computer.map(computer.getScore(), score_low, score_high, HeightRange.x, HeightRange.x + difference);
    }
    private float newHeightRangeY(int gamemodeint, float difference)
    {
        //checkMode(gamemodeint);
        return computer.map(computer.getScore(), score_low, score_high, HeightRange.y - difference, HeightRange.y);
    }

    private float ranHeight(int chances)
    {
        return computer.map(UnityEngine.Random.Range(1, chances+1), 1, chances, numX, numY);
    }

    private float numX, numY;
    private int chances;


    private IEnumerator topRandGen()
    {
        while (true)
        {
            topWidth = WidthRange.x;
            top = GetObstacle();

            numX = newHeightRangeX(GameModeInt, 0.5f);
            numY = newHeightRangeY(GameModeInt, 0.4f);

            topHeight = ranHeight(chances);
            //Debug.Log(topHeight);
            //topHeight = UnityEngine.Random.Range(numX, numY);
            //topHeight = HeightRange.y;

            updateTopTransform();
            yield return new WaitForSeconds(topInterval);
            top.SetActive(true);
        }
    }
    private IEnumerator bottomRandGen()
    {
        while (true)
        {
            bottomWidth = WidthRange.x;
            bottom = GetObstacle();
            //HeightRange.x = height_defLow;
            //HeightRange.y = map(getScore(), score_low, score_high, height_defLow, height_defHigh);

            numX = newHeightRangeX(GameModeInt, 0.8f);
            //numY = newHeightRangeY(GameModeInt, 0.4f);
            numY = HeightRange.y;
            //bottomHeight = UnityEngine.Random.Range(numX, numY);
            bottomHeight = ranHeight(chances);
            //bottomHeight = HeightRange.y;
            updateBottomTransform();
            yield return new WaitForSeconds(bottomInterval);
            bottom.SetActive(true);
        }
    }


    private float startingSpeed = 8f;
    private float newSpeed(float score)
    {
        if (score < score_low)
            return startingSpeed;
        else if (score > score_high)
            return broadSpeed + 2;

        return computer.map(score, score_low, score_high, startingSpeed, broadSpeed + 2);
    }


    private float maxScoreRate;
    private float minScoreRate;
    private float newScoreRate(float score)
    {   
        if (score < score_low)
            return minScoreRate;
        else if (score > score_high * 0.7f)
            return maxScoreRate;

        return computer.map(score, score_low, score_high*0.7f, minScoreRate, maxScoreRate);
    }










    // -----------------------------------------------
    //private IEnumerator generator()
    //{
    //    Speed = 9;
    //    HeightRange = new Vector2(0.5f, 2);
    //    WidthRange = new Vector2(3, 3);
    //    D = 1;
    //    Smooth = 2;
    //    Delta = 0.5f;

    //    while (true)
    //    {
    //        WidthRange = new Vector2(3, 3);
    //        Delta = 0.5f;

    //        yield return StartCoroutine(gen1());
    //        yield return StartCoroutine(gen2());

    //        if (HeightRange.y < 4f)
    //        {
    //            HeightRange.x += 0.5f;
    //            HeightRange.y += 0.5f;
    //        }

    //        if (Speed < 15)
    //        {
    //            Speed += 1f;
    //            Smooth += 0.5f;
    //        }

    //        if (D < 7)
    //        {
    //            D++;
    //        }
    //    }
    //}
    //public IEnumerator gen1()
    //{
    //    float height = HeightRange.x;
    //    float width = WidthRange.x;
    //    bool h = true;
    //    int T = (int)((HeightRange.y - HeightRange.x) / Delta) * 2;
    //    int t = 0;

    //    while (t < D * T)
    //    {
    //        top = GetObstacle();
    //        bottom = GetObstacle();

    //        if (h)
    //            height = Mathf.MoveTowards(height, HeightRange.y, Delta);
    //        else
    //            height = Mathf.MoveTowards(height, HeightRange.x, Delta);

    //        if (height <= HeightRange.x)
    //            h = true;
    //        else if (height >= HeightRange.y)
    //            h = false;

    //        width = UnityEngine.Random.Range(WidthRange.x, WidthRange.y);

    //        topHeight = bottomHeight = height;
    //        topWidth = bottomWidth = width;
    //        this.updateTopTransform();
    //        updateBottomTransform();

    //        yield return new WaitForSeconds(topInterval);
    //        t++;

    //        top.SetActive(true);
    //        bottom.SetActive(true);
    //    }
    //}
    //public IEnumerator gen2()
    //{
    //    topHeight = HeightRange.x;
    //    bottomHeight = HeightRange.y;

    //    float width = WidthRange.x;
    //    bool h = true;
    //    int T = (int)((HeightRange.y - HeightRange.x) / Delta) * 2;
    //    int t = 0;

    //    while (t < D * T)
    //    {
    //        top = GetObstacle();
    //        bottom = GetObstacle();

    //        if (h)
    //        {
    //            topHeight = Mathf.MoveTowards(topHeight, HeightRange.y, Delta);
    //            bottomHeight = Mathf.MoveTowards(bottomHeight, HeightRange.x, Delta);
    //        }
    //        else
    //        {
    //            topHeight = Mathf.MoveTowards(topHeight, HeightRange.x, Delta);
    //            bottomHeight = Mathf.MoveTowards(bottomHeight, HeightRange.y, Delta);
    //        }

    //        if (topHeight <= HeightRange.x)
    //        {
    //            h = true;
    //        }
    //        else if (topHeight >= HeightRange.y)
    //        {
    //            h = false;
    //        }

    //        width = UnityEngine.Random.Range(WidthRange.x, WidthRange.y);

    //        topWidth = bottomWidth = width;
    //        updateTopTransform();
    //        updateBottomTransform();

    //        yield return new WaitForSeconds(topInterval);
    //        t++;

    //        top.SetActive(true);
    //        bottom.SetActive(true);
    //    }
    //}
    // -----------------------------------------------

}
