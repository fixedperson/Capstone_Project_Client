using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    private static GameOverManager instance = null;
    
    public GameObject button;
    
    public float fadeTime = 2f; // 페이드 시간
    public Image fadeImg;
    public Text fadeText;
    public Text fadeText2;
    
    void Awake()
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
    public void StartFadein(int stage)
    {
        fadeText2.text = stage + " stage";
        StartCoroutine("FadeIn");               //서서히 유다희 등장
        StartCoroutine("ActiveButton");         //유다희 등장 1초 후 버튼 생김
    }

    IEnumerator FadeIn()
    {
        fadeImg.gameObject.SetActive(true);
        Color imgColor = fadeImg.color;
        imgColor.a = 0f;

        Color textColor = fadeText.color;
        textColor.a = 0f;
        
        Color textColor2 = fadeText2.color;
        textColor2.a = 0f;
        
        while (imgColor.a < 1f)
        {
            imgColor.a += Time.deltaTime / fadeTime;
            textColor.a += Time.deltaTime / fadeTime;
            textColor2.a += Time.deltaTime / fadeTime;
            
            fadeImg.color = imgColor;
            fadeText.color = textColor;
            fadeText2.color = textColor2;

            if (imgColor.a >= 1f)
            {
                imgColor.a = 1f;
                textColor.a = 1f;
                textColor2.a = 1f;
            }
            yield return null;
        }
    }
    IEnumerator ActiveButton()
    {
        yield return new WaitForSeconds(fadeTime+1f);
        button.SetActive(true);
    }

    public void ButtonClick()
    {
        C_LeaveGame leaveGame = new C_LeaveGame();
        Managers.Network.Send(leaveGame);
        
        Application.Quit();
        // SceneManager.LoadScene("StartScene");
        // Managers.Object.removeDontDestroyObjects();
        // Destroy(GameObject.Find("@Managers"));
    }
}
