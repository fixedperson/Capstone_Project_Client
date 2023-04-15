using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    public Text timeUi;

    public float setTime = 65;
    private int min;
    private float sec;

    void Update()
    {
        setTime -= Time.deltaTime;
        min = (int)setTime / 60;
        sec = setTime % 60;

        if (setTime >30)
        {
            timeUi.text = min + ":" + (int)sec;
        }

        if( setTime <= 30)
        {
            if((int)setTime % 2 == 1)
                timeUi.text = "<color=red>" + min + ":" + (int)sec + "</color>";
            else
                timeUi.text = "<color=white>" + min + ":" + (int)sec + "</color>";
        }

        
        if (setTime <= 0)
        {
            timeUi.text = "0:0";
        }
    }
}
