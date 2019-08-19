/****************************************************
    文件：BuyWnd.cs
	作者：Zht
    日期：2019/7/9 9:32:21
	功能：购买窗口
*****************************************************/

using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class BuyWnd : WindowRoot 
{
    private int buyType;

    public Text txtInfo;
    public Button btnSure;

    protected override void InitWnd()
    {
        base.InitWnd();
        btnSure.interactable = true;
        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (buyType)
        {
            case 0:
                SetText(txtInfo, string.Format("是否花费{0}购买{1}?", Constants.Color("10钻石", TxtColor.Red), Constants.Color("100体力", TxtColor.Green)));
                break;
            case 1:
                SetText(txtInfo, string.Format("是否花费{0}购买{1}?", Constants.Color("10钻石", TxtColor.Red), Constants.Color("1000金币", TxtColor.Green)));
                break;
        }
    }

    public void SetBuyType(int type)
    {
        buyType = type;
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.ReqBuy,
            val = new ReqBuy()
            {
                type = buyType,
                cost = 10,
            }
        };

        netSvc.SendMsg(msg);
        btnSure.interactable = false;
    }
}