using System.Collections;
using System.Collections.Generic;
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
    
    float time = 0f;
    float startTime;
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
    public void StartFadein()
    {
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
        
        while (imgColor.a < 1f)
        {
            imgColor.a += Time.deltaTime / fadeTime;
            textColor.a += Time.deltaTime / fadeTime;
            
            fadeImg.color = imgColor;
            fadeText.color = textColor;

            if (imgColor.a >= 1f)
            {
                imgColor.a = 1f;
                textColor.a = 1f;
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
        SceneManager.LoadScene("StartScene");
        Managers.Object.removeDontDestroyObjects();
        Destroy(GameObject.Find("@Managers"));
    }
}
