using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using Unity.VisualScripting;
using UnityEngine;

public class ErrorManager : MonoBehaviour
{
    public GameObject error;
    public GameObject selectBtn;

    public void Check()
    {
        C_PlayerSelect playerSelect = new C_PlayerSelect();
        playerSelect.PlayerCode = (int)DataManager.instance.currentCharacter;
        Managers.Network.Send(playerSelect);
    }

    public void ErrorDisplay()
    {
        StopCoroutine("Disappear");
        error.SetActive(true);

        StartCoroutine("Disappear");
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(0.5f);
        error.SetActive(false);
    }

    public void BtnDisappear()
    {
        selectBtn.SetActive(false);
    }

}
