using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.UI;

public class StageClearManager : MonoBehaviour
{
    private static StageClearManager instance = null;
    
    public GameObject gameClearScene;
    public GameObject nextStageBtn;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Managers.Object.AddDontDestroyObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
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
