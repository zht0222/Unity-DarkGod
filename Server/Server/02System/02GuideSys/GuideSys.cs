using Protocol;

public class GuideSys
{
    private static GuideSys _instance = null;
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public static GuideSys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GuideSys();
            }
            return _instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        Common.Log("GuideSys Init Done");
    }

    public void ReqGuide(MsgPack pack)
    {
        ReqGuide data = pack.gameMsg.val as ReqGuide;
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        GuideCfg gc = cfgSvc.GetGuideData(data.guideid);
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspGuide
        };

        //更新引导ID
        if (pd.guideid == data.guideid)
        {
            if (pd.guideid == 1001)
            {
                TaskSys.Instance.CalcTaskPrgs(pd, 1);
            }

            pd.guideid++;
            pd.coin += gc.coin;
            Common.CalcExp(pd, gc.exp);
            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.val = new RspGuide
                {
                    guideid = pd.guideid,
                    coin = pd.coin,
                    lv = pd.lv,
                    exp = pd.exp
                };
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ServerDataError;
        }
        pack.session.SendMsg(msg);
    }

   
}