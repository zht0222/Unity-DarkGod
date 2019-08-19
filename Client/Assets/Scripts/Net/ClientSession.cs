using PENet;
using Protocol;

public class ClientSession : PESession<GameMsg>
{
    protected override void OnConnected()
    {
        GameRoot.AddTips("连接服务器成功");
        Common.Log("Connect To Server Succ");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        Common.Log("RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddNetPkg(msg);
    }

    protected override void OnDisConnected()
    {
        GameRoot.AddTips("服务器断开连接");
        Common.Log("DisConnect To Server");
    }
}