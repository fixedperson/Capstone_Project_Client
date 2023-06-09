using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;
using UnityEngine.UI;


public class ChatManager : MonoBehaviour
{
    private static ChatManager instance = null;
    
    public List<string> chatList = new List<string>();
    public GameObject ChattingBox;
    public Button sendBtn;
    public Text chatLog;
    public InputField input;
    ScrollRect scroll_rect = null;
    private bool off = false;

    void Awake()
    {
        scroll_rect = GameObject.FindObjectOfType<ScrollRect>();
        ChattingBox.SetActive(false);
        
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

    public void SendButton()
    {
        if (input.text.Equals("")) return;
        string msg = string.Format("나 : {0}", input.text);
        SendMsgPacket(input.text);
        ReceiveMsg(msg);
        input.text = "";
        off = false;
    }

    public void ReceiveMsg(string msg)
    {
        chatLog.text += "\n" + msg;
        chatList.Add(msg);
        scroll_rect.verticalNormalizedPosition = 0.0f;      //채팅창을 아래로 고정
    }

    public void SendMsgPacket(string msg)
    {
        C_PlayerChat playerChat = new C_PlayerChat();
        playerChat.Chat = msg;
        Managers.Network.Send(playerChat);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!ChattingBox.activeSelf)                                        //비활성화일때 엔터 누르면 활성화
            {
                ChattingBox.SetActive(true);
                input.ActivateInputField();
            }
            if (!input.isFocused)  SendButton();                                    //활성화일때 엔터 누르면 채팅 송신
            if (ChattingBox.activeSelf && off) { 
                ChattingBox.SetActive(false); 
                off = false; 
            }                                                               //채팅박스 끄기
            else if (ChattingBox.activeSelf && !off) off = !off;
        }
    }
}
