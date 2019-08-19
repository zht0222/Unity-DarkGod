/****************************************************
    文件：CreateWnd.cs
	作者：Zht
    日期：2019/6/1 9:21:44
	功能：角色创建界面
*****************************************************/

using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class CreateWnd : WindowRoot 
{
    public InputField iptName;

    protected override void InitWnd()
    {
        base.InitWnd();

        //随机一个名字
        iptName.text = resSvc.GetRdNameData(false);
    }

    public void ClickRandBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        iptName.text = resSvc.GetRdNameData(false);
    }

    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        if (iptName.text != null)
        {
            //发送名字数据到服务器，登录主城
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqRename,
                val = new ReqRename
                {
                    name = iptName.text
                }
            };
            netSvc.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("当前名字不符合规范");
        }
    }
}