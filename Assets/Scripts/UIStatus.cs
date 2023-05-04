using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatus : MonoBehaviour
{
    public MyPlayer myPlayer;

    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
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
