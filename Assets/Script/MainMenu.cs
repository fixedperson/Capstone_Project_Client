using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject audioSlider;
    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Quit()
    {
        Debug.Log("Quit");
    }

    public void Start()
    {
        audioSlider.SetActive(false);
    }

    public void ShowAudio()
    {
        if (audioSlider.activeSelf == true) audioSlider.SetActive(false);
        else if (audioSlider.activeSelf == false) audioSlider.SetActive(true);
    }
}
