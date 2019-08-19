using PENet;
using Protocol;

public class ServerSession : PESession<GameMsg>
{
    public int sessionID = 0;

    protected override void OnConnected()
    {
        sessionID = ServerRoot.Instance.GetSessionID();
        Common.Log("SessionID:" + sessionID + " Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        Common.Log("SessionID:" + sessionID + " RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        MsgPack pack = new MsgPack(this, msg);
        NetSvc.Instance.AddMsgQue(pack);
    }

    protected override void OnDisConnected()
    {
        Common.Log("SessionID:" + sessionID + " Client DisConnect");
        LoginSys.Instance.ClearOfflineData(this);
    }
}