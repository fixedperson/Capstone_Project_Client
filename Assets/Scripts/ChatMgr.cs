using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChatMgr : MonoBehaviour
{
    public List<string> chatList = new List<string>();
    public GameObject ChattingBox;
    public Button sendBtn;
    public Text chatLog;
    public InputField input;
    ScrollRect scroll_rect = null;

    void Start()
    {
        scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
        ChattingBox.SetActive(false);
    }

    public void SendButton()
    {
        if (input.text.Equals("")) return;
        string msg = string.Format("�г��� : {0}", input.text);
        ReceiveMsg(msg);
        input.ActivateInputField();
        input.text = "";                
    }

    public void ReceiveMsg(string msg)
    {
        chatLog.text += "\n" + msg;
        chatList.Add(msg);
        scroll_rect.verticalNormalizedPosition = 0.0f;      //ä��â�� �Ʒ��� ����
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !input.isFocused)                //Ȱ��ȭ�϶� ���� ������ ä�� �۽�
        {
            SendButton();
        }
        if (!ChattingBox.activeSelf && Input.GetKeyDown(KeyCode.Return))        //��Ȱ��ȭ�϶� ���� ������ Ȱ��ȭ
        {
            ChattingBox.SetActive(true);
            input.ActivateInputField();
        }
        if(input.isFocused) StopCoroutine("ChatBoxInactive");
        else StartCoroutine("ChatBoxInactive");
        if (ChattingBox.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                scroll_rect.verticalNormalizedPosition += 0.1f;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                scroll_rect.verticalNormalizedPosition -= 0.1f;
            }
        }
    }

    IEnumerator ChatBoxInactive()                                //�Է� ������ 5���Ŀ� ��Ȱ��ȭ
    {
        yield return new WaitForSeconds(5.0f);
        ChattingBox.SetActive(false);
    }
}
