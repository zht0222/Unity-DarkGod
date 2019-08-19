using PENet;
using Protocol;
using System.Collections.Generic;

public class MsgPack
{
    public ServerSession session;
    public GameMsg gameMsg;
    public MsgPack(ServerSession session, GameMsg gameMsg)
    {
        this.session = session;
        this.gameMsg = gameMsg;
    }
}

public class NetSvc
{
    private static readonly string obj = "Lock";
    private static NetSvc _instance = null;
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();

    public static NetSvc Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new NetSvc();
            }
            return _instance;
        }
    }

    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(SrvCfg.srvIP, SrvCfg.srvPort);
        Common.Log("NetSvc Init Done");
    }

    public void Update()
    {
        if (msgPackQue.Count > 0)
        {
            Common.Log("PackCount:" + msgPackQue.Count);
            lock (obj)
            {
                MsgPack pack = msgPackQue.Dequeue();
                HandOutMsg(pack);
            }
        }
    }

    public void AddMsgQue(MsgPack pack)
    {
        lock (obj)
        {
            msgPackQue.Enqueue(pack);
        }
    }

    private void HandOutMsg(MsgPack pack)
    {
        switch ((CMD)pack.gameMsg.cmd)
        {
            case CMD.ReqLogin:
                LoginSys.Instance.ReqLogin(pack);
                break;
            case CMD.ReqRename:
                LoginSys.Instance.ReqRename(pack);
                break;
            case CMD.ReqGuide:
                GuideSys.Instance.ReqGuide(pack);
                break;
            case CMD.ReqStrong:
                StrongSys.Instance.ReqStrong(pack);
                break;
            case CMD.SndChat:
                ChatSys.Instance.SndChat(pack);
                break;
            case CMD.ReqBuy:
                BuySys.Instance.ReqBuy(pack);
                break;
            case CMD.ReqTakeTaskReward:
                TaskSys.Instance.ReqTakeTaskReward(pack);
                break;
            case CMD.ReqFbFight:
                FubenSys.Instance.ReqFbFight(pack);
                break;
        }
    }
}
