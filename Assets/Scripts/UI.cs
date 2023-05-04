using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text timeUi;

    public float setTime = 80;
    private int min;
    private int sec;

    void Update()
    {
        min = (int)setTime / 60;
        sec = (int)setTime % 60;

        if (setTime >30)
        {
            timeUi.text = min + ":" + sec;
        }

        if( setTime <= 30)
        {
            if((int)setTime % 2 == 1)
                timeUi.text = "<color=red>" + min + ":" + sec + "</color>";
            else
                timeUi.text = "<color=white>" + min + ":" + sec + "</color>";
        }

        
        if (setTime <= 0)
        {
            timeUi.text = "0:0";
        }
    }
}
