using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    OHS, THS
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    private void Awake()
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

    public Character currentCharacter;
}
