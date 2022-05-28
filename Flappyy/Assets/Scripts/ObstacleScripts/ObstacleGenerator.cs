
using Assets.Scripts.ObstacleScripts;
using System;
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
    public float Delta;
    public int D;
    public Transform ObstaclesContainer;
    public GameObject Obstacle;

    private float time;
    protected Vector3 startPos;
    protected GameObject top;
    protected GameObject bottom;
    protected float topHeight;
    protected float topWidth;
    protected float bottomHeight;
    protected float bottomWidth;

    GameController gc;
    public Text InGameScoreText;


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
    }
    void Start()
    {
        StartCoroutine(topRandGen());
        StartCoroutine(bottomRandGen());
        //StartCoroutine(generator());
    }
    // -----------------------------------------------

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
    private float getScore()
    {
        return (float)Math.Round(float.Parse(InGameScoreText.text.Replace("Score: ", "")),2);
    }
    private static float map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }

    const float height_defLow = 2.5f;
    const float height_defHigh = 4.2f;

    const float score_low = 0f;
    const float score_high = 20f;

    private IEnumerator topRandGen()
    {
        topWidth = WidthRange.x;

        while (true)
        {
            top = GetObstacle();
            HeightRange.x = height_defLow;
            HeightRange.y = map(getScore(), score_low, score_high, height_defLow, height_defHigh);

            topHeight = UnityEngine.Random.Range(HeightRange.x, HeightRange.y);
            updateTopTransform();
            yield return new WaitForSeconds(topInterval);
            top.SetActive(true);
        }
    }
    private IEnumerator bottomRandGen()
    {
        bottomWidth = WidthRange.x;

        while (true)
        {
            bottom = GetObstacle();
            HeightRange.x = height_defLow;
            HeightRange.y = map(getScore(), score_low, score_high, height_defLow, height_defHigh);

            bottomHeight = UnityEngine.Random.Range(HeightRange.x, HeightRange.y);
            updateBottomTransform();
            yield return new WaitForSeconds(bottomInterval);
            bottom.SetActive(true);
        }
    }

    // -----------------------------------------------
    private IEnumerator generator()
    {
        Speed = 9;
        HeightRange = new Vector2(0.5f, 2);
        WidthRange = new Vector2(3, 3);
        D = 1;
        Smooth = 2;
        Delta = 0.5f;

        while (true)
        {
            WidthRange = new Vector2(3, 3);
            Delta = 0.5f;

            yield return StartCoroutine(gen1());
            yield return StartCoroutine(gen2());

            if (HeightRange.y < 4f)
            {
                HeightRange.x += 0.5f;
                HeightRange.y += 0.5f;
            }

            if (Speed < 15)
            {
                Speed += 1f;
                Smooth += 0.5f;
            }

            if (D < 7)
            {
                D++;
            }
        }
    }

    public IEnumerator gen1()
    {
        float height = HeightRange.x;
        float width = WidthRange.x;
        bool h = true;
        int T = (int)((HeightRange.y - HeightRange.x) / Delta) * 2;
        int t = 0;

        while (t < D * T)
        {
            top = GetObstacle();
            bottom = GetObstacle();

            if (h)
                height = Mathf.MoveTowards(height, HeightRange.y, Delta);
            else
                height = Mathf.MoveTowards(height, HeightRange.x, Delta);

            if (height <= HeightRange.x)
                h = true;
            else if (height >= HeightRange.y)
                h = false;

            width = UnityEngine.Random.Range(WidthRange.x, WidthRange.y);

            topHeight = bottomHeight = height;
            topWidth = bottomWidth = width;
            this.updateTopTransform();
            updateBottomTransform();

            yield return new WaitForSeconds(topInterval);
            t++;

            top.SetActive(true);
            bottom.SetActive(true);
        }
    }
    public IEnumerator gen2()
    {
        topHeight = HeightRange.x;
        bottomHeight = HeightRange.y;

        float width = WidthRange.x;
        bool h = true;
        int T = (int)((HeightRange.y - HeightRange.x) / Delta) * 2;
        int t = 0;

        while (t < D * T)
        {
            top = GetObstacle();
            bottom = GetObstacle();

            if (h)
            {
                topHeight = Mathf.MoveTowards(topHeight, HeightRange.y, Delta);
                bottomHeight = Mathf.MoveTowards(bottomHeight, HeightRange.x, Delta);
            }
            else
            {
                topHeight = Mathf.MoveTowards(topHeight, HeightRange.x, Delta);
                bottomHeight = Mathf.MoveTowards(bottomHeight, HeightRange.y, Delta);
            }

            if (topHeight <= HeightRange.x)
            {
                h = true;
            }
            else if (topHeight >= HeightRange.y)
            {
                h = false;
            }

            width = UnityEngine.Random.Range(WidthRange.x, WidthRange.y);

            topWidth = bottomWidth = width;
            updateTopTransform();
            updateBottomTransform();

            yield return new WaitForSeconds(topInterval);
            t++;

            top.SetActive(true);
            bottom.SetActive(true);
        }
    }
    // -----------------------------------------------

}
