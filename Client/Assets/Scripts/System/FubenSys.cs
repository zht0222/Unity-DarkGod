/****************************************************
    文件：FubenSys.cs
	作者：Zht
    日期：2019/7/29 7:13:16
	功能：副本业务系统
*****************************************************/

using UnityEngine;
using Protocol;

public class FubenSys : SystemRoot 
{
    public static FubenSys Instance = null;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        Debug.Log("Init FubenSys...");
    }

    public void EnterFuben()
    {
        SetFubenWndState();
    }

    public void RspFbFight(GameMsg msg)
    {
        RspFbFight data = msg.val as RspFbFight;
        GameRoot.Instance.SetPlayerDataByFbStart(data);
        MainCitySys.Instance.mainCityWnd.SetWndState(false);
        SetFubenWndState(false);
        BattleSys.Instance.StartBattle(data.fbID);
    }

    #region Fuben Wnd
    public FubenWnd fubenWnd;

    public void SetFubenWndState(bool isActive = true)
    {
        fubenWnd.SetWndState(isActive);
    }
    #endregion
}