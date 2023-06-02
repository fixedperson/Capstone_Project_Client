using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject audioSlider; // ����� ��ư ������ �� �����̴��� ���̰� �ϱ����� ����
    public GameObject connect;
    public GameObject prefab;

    public void Play()
    {
        GameObject gameObject = Instantiate(prefab);
        connect.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Start()
    {
        audioSlider.SetActive(false);
    }

    public void ShowAudio()
    {
        audioSlider.SetActive(!audioSlider.activeSelf);
    }
}
