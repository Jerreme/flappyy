﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstController : MonoBehaviour
{
    public float MaxDelay = 5;

    private float delay;

    void Start()
    {
        delay = 0;
    }

    void Update()
    {
        delay += Time.deltaTime;
        if (delay >= MaxDelay) //!TapsellStandardBanner.IsRequesting ||
            SceneManager.LoadScene(1);
    }
}
