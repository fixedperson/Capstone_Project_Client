using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fadein : MonoBehaviour
{
    public GameObject Img;
    public float FadeTime = 2f; // ���̵� �ð�
    public Image fadeImg;
    public Text fadeText;
    public GameObject button;
    float time = 0f;
    float StartTime;
    Color Imgcolor = new Color(0.15f,0,0,0);
    Color Textcolor = new Color(1, 1, 1, 0);

    void Awake()
    {
        StartTime = Time.deltaTime;
        StartFadein();
    }
    public void StartFadein()
    {
        Img.SetActive(true);
        StartCoroutine("fadein");               //������ ������ ����
        StartCoroutine("activeButton");         //������ ���� 1�� �� ��ư ����
    }

    IEnumerator fadein()
    {
        while (true)
        {
            time += StartTime / FadeTime;
            Imgcolor.a = Mathf.Lerp(0f, 0.9f, time);
            Textcolor.a = Mathf.Lerp(0f, 1f, time);
            fadeImg.color = Imgcolor;
            fadeText.color = Textcolor;
            yield return null;
        }
    }
    IEnumerator activeButton()
    {
        yield return new WaitForSeconds(FadeTime+1f);
        button.SetActive(true);
    }
}
