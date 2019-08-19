/****************************************************
    文件：ChatWnd.cs
	作者：Zht
    日期：2019/7/7 22:8:12
	功能：；聊天界面
*****************************************************/

using Protocol;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class ChatWnd : WindowRoot 
{
    private bool canSend = true;
    private int chatType;
    private List<string> chatLst = new List<string>();

    public InputField iptChat;
    public Text txtChat;
    public Image imgWorld;
    public Image imgGuid;
    public Image imgFriend;

    protected override void InitWnd()
    {
        base.InitWnd();
        chatType = 0;
        EventSystem.current.SetSelectedGameObject(iptChat.gameObject);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (chatType == 0)
        {
            string chatMsg = "";
            for (int i = 0; i < chatLst.Count; i++)
            {
                chatMsg += chatLst[i] + "\n";
            }
            SetText(txtChat, chatMsg);
            SetSprite(imgWorld, "ResImages/btntype1");
            SetSprite(imgGuid, "ResImages/btntype2");
            SetSprite(imgWorld, "ResImages/btntype2");
        }
        else if (chatType == 1)
        {
            SetText(txtChat, "尚未加入公会");
            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuid, "ResImages/btntype1");
            SetSprite(imgWorld, "ResImages/btntype2");
        }
        else if (chatType ==  2)
        {
            SetText(txtChat, "暂无好友信息");
            SetSprite(imgWorld, "ResImages/btntype2");
            SetSprite(imgGuid, "ResImages/btntype2");
            SetSprite(imgWorld, "ResImages/btntype1");
        }
    }

    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }

    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }

    public void ClickFriendBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }

    public void ClickSendBtn()
    {
        if (!canSend)
        {
            GameRoot.AddTips("操作过于频繁");
            return;
        }

        if (!string.IsNullOrEmpty(iptChat.text) && iptChat.text != " ")
        {
            if (iptChat.text.Length > 12)
            {
                GameRoot.AddTips("输入信息不能超过12个字");
            }
            else
            {
                GameMsg msg = new GameMsg()
                {
                    cmd = (int)CMD.SndChat,
                    val = new SndChat()
                    {
                        chat = iptChat.text
                    }
                };
                netSvc.SendMsg(msg);
                iptChat.text = "";
                canSend = false;
                timerSvc.AddTimerTask((tid) =>
                {
                    canSend = true;
                }, 5, PETimeUnit.Second);
            }
        }
        else
        {
            GameRoot.AddTips("尚未输入聊天信息");
        }
    }

    private void StartCoroutine()
    {
        throw new NotImplementedException();
    }

    public void AddChatMsg(string name, string chat)
    {
        chatLst.Add(Constants.Color(name + "：", TxtColor.Blue) + chat);
        if (chatLst.Count > 12)
        {
            chatLst.RemoveAt(0);
        }
        RefreshUI();
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        SetWndState(false);
    }
}