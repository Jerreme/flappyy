using Assets.Scripts.UIs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public Vector2 RotateRange;
    public float Delta;
    public float Delay;
    public float Target;

    private float rot;
    private bool r;
    private float target;

    private int mode;

    void Start()
    {
        mode = PlayerPrefs.GetInt(MainController.Prefs_Modes_Key, MainController.Prefs_Modes_DefIndex);
        StartCoroutine(rotation());
    }

    void LateUpdate()
    {
        //rot1();
    }

    void rot1()
    {
        //Delta = 8-Smooth 16-Hard 28-Crazy
        if (r)
            rot = Mathf.MoveTowards(rot, RotateRange.y, Time.deltaTime * Delta);
        else
            rot = Mathf.MoveTowards(rot, RotateRange.x, Time.deltaTime * Delta);

        if (rot <= RotateRange.x)
            r = true;
        else if (rot >= RotateRange.y)
            r = false;

        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    private readonly float degreeCalm = -3f;
    private readonly float degreeNormal = 0f;
    private readonly float degreeCrazy = 3f;

    private float newTarget()
    {
        if (mode == 0)
        {
            return degreeCalm + additional();
        }
        else if (mode == 1)
        {
            return degreeNormal + additional();
        }
        else if (mode == 2)
        {
            return degreeCrazy + additional();
        }
        else
        {
            return degreeNormal;
        }
    }
    private float additional()
    {
        if (StaticVariables.game_Score > 500) 
        {
            return 5f;
        }
        else if (StaticVariables.game_Score > 200)
        {
            return 2f;
        }
        return 0f;
    }

    IEnumerator rotation()
    {
        while (true)
        {
            target = Target + newTarget();
            yield return StartCoroutine(rot2());
            target = 0;
            yield return StartCoroutine(rot2());

            target = -Target + -newTarget();
            yield return StartCoroutine(rot2());
            target = 0;
            yield return StartCoroutine(rot2());
        }
    }
    
    IEnumerator rot2()
    {
        Debug.Log(target);
        float t = 0;
        yield return new WaitForSeconds(Delay);
        while (t < 2)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, target), 0.05f);
            t += Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
}
    }