using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatus : MonoBehaviour
{
    public MyPlayer myPlayer;

    public GameObject red;

    public Image image;

    void Awake()
    {
        red = GameObject.Find("Red");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        image = red.GetComponent<Image>();

        var objs = FindObjectsOfType<UIStatus>();
        if(objs.Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        HpUpdate();
    }
    
    void HpUpdate()
    {
        image.fillAmount = Mathf.Lerp(image.fillAmount, myPlayer.curHealth/myPlayer.maxHealth, Time.deltaTime * 10);
    }
}
