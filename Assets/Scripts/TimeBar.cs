﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public Sprite[] timeBarImgs;
    public Image timeBar;

    // Use this for initialization
    void Start()
    {
        timeBar.sprite = timeBarImgs[28];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine("DoTimeBar");
        }
    }

    IEnumerator DoTimeBar()
    {
        for (int i = 28; i >= 0; --i)
        {
            timeBar.sprite = timeBarImgs[i];
            //Value determined by taking number of different time bar sprites (28) and using math to make them all appear in sequence over a 90 second time interval
            yield return new WaitForSecondsRealtime(3.2143F);
        }
        yield return null;
    }
}
