/****************************************************
    文件：GuideWnd.cs
	作者：Zht
    日期：2019/6/12 21:38:27
	功能：引导对话界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using Protocol;

public class GuideWnd : WindowRoot 
{
    private PlayerData pd;
    private AutoGuideCfg curtTaskData;
    private string[] dialogArr;
    private int index;

    public Text txtName;
    public Text txtTalk;
    public Image imgIcon;

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PPlayerData;
        curtTaskData = MainCitySys.Instance.CurtTaskData;
        dialogArr = curtTaskData.dilogArr.Split('#');
        index = 1;
        SetTalk();
    }

    /// <summary>
    /// 设置对话框内容
    /// </summary>
    private void SetTalk()
    {
        string[] talkArr = dialogArr[index].Split('|');
        if (talkArr[0] == "0")
        {
            SetSprite(imgIcon, PathDefine.SelfIcon);
            SetText(txtName, pd.name);
        }
        else
        {
            switch (curtTaskData.npcID)
            {
                case Constants.NPCWiseMan:
                    SetSprite(imgIcon, PathDefine.WiseManIcon);
                    SetText(txtName, "智者");
                    break;
                case Constants.NPCGeneral:
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    SetText(txtName, "将军");
                    break;
                case Constants.NPCArtisan:
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    SetText(txtName, "工匠");
                    break;
                case Constants.NPCTrader:
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    SetText(txtName, "商人");
                    break;
                default:
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    SetText(txtName, "小芸");
                    break;
            }
        }
        imgIcon.SetNativeSize();
        SetText(txtTalk, talkArr[1].Replace("$name", pd.name));
    }

    /// <summary>
    /// 点击下一条按钮
    /// </summary>
    public void ClickNextBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        index++;
        if (index == dialogArr.Length)
        {
            //发送网络请求
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqGuide,
                val = new ReqGuide
                {
                    guideid = curtTaskData.ID
                }
            };
            netSvc.SendMsg(msg);
            SetWndState(false);
        }
        else
        {
            SetTalk();
        }
    }
}