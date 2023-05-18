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
    private bool off = false;

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
        input.text = "";
        off = false;
    }

    public void ReceiveMsg(string msg)
    {
        chatLog.text += "\n" + msg;
        chatList.Add(msg);
        scroll_rect.verticalNormalizedPosition = 0.0f;      //ä��â�� �Ʒ��� ����
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!ChattingBox.activeSelf)                                        //��Ȱ��ȭ�϶� ���� ������ Ȱ��ȭ
            {
                ChattingBox.SetActive(true);
                input.ActivateInputField();
            }
            if (!input.isFocused)  SendButton();                                    //Ȱ��ȭ�϶� ���� ������ ä�� �۽�
            if (ChattingBox.activeSelf && off) { 
                ChattingBox.SetActive(false); 
                off = false; 
            }                                                               //ä�ùڽ� ����
            else if (ChattingBox.activeSelf && !off) off = !off;
        }
    }
}
