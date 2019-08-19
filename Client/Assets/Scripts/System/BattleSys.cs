/****************************************************
    文件：BattleSys.cs
	作者：Zht
    日期：2019/8/1 6:54:22
	功能：战斗系统
*****************************************************/

using UnityEngine;

public class BattleSys : SystemRoot 
{
    public PlayerCtrlWnd playerCtrlWnd;

    private BattleMgr battleMgr;

    public static BattleSys Instance = null;

    public override void InitSys()
    {
        base.InitSys();
        Instance = this;
        Debug.Log("Init BattleSys...");
    }

    public void StartBattle(int mapID)
    {
        GameObject go = new GameObject()
        {
            name = "BattleRoot"
        };
        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();
        battleMgr.Init(mapID);
        SetPlayerCtrlWndState();
    }

    public void SetPlayerCtrlWndState(bool isActive = true)
    {
        playerCtrlWnd.SetWndState(isActive);
    }

    public void SetMoveDir(Vector2 dir)
    {
        battleMgr.SetSelfPlayerMoveDir(dir);
    }

    public void ReqReleaseSkill(int index)
    {
        battleMgr.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput()
    {
        return playerCtrlWnd.currentDir;
    }
}