using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHPbarManager : MonoBehaviour
{
    private static CharacterHPbarManager instance = null;
    
    public MyPlayer myPlayer;

    public GameObject red;

    public Image image;

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
        
        red = GameObject.Find("Red");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        image = red.GetComponent<Image>();
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
