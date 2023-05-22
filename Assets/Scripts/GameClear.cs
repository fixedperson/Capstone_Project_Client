using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.UI;

public class GameClear : MonoBehaviour
{
    public GameObject gameClearScene;
    public GameObject nextStageBtn;

    public void Start()
    {
        var objs = FindObjectsOfType<GameClear>();
        if(objs.Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);
    }

    public void GameClearActive()
    {
        gameClearScene.SetActive(true);
        nextStageBtn.SetActive(true);
    }
    
    public void NextStage()
    {
        C_NextStageReady nextStageReady = new C_NextStageReady();
        Managers.Network.Send(nextStageReady);
        nextStageBtn.SetActive(false);
    }
}
