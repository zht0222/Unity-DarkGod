using Protocol;

public class StrongSys
{
    private static StrongSys _instance = null;
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public static StrongSys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new StrongSys();
            }
            return _instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        Common.Log("StrongSys Init Done");
    }

    public void ReqStrong(MsgPack pack)
    {
        ReqStrong data = pack.gameMsg.val as ReqStrong;
        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspStrong
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int curtStarLv = pd.strongArr[data.pos];
        StrongCfg nextSD = cfgSvc.GetStrongData(data.pos, curtStarLv + 1);

        if (pd.lv < nextSD.minlv)
        {
            msg.err = (int)ErrorCode.LackLevel;
        }
        else if (pd.coin < nextSD.coin)
        {
            msg.err = (int)ErrorCode.LackCoin;
        }
        else if (pd.crystal < nextSD.crystal)
        {
            msg.err = (int)ErrorCode.LackCrystal;
        }
        else
        {
            TaskSys.Instance.CalcTaskPrgs(pd, 3);

            pd.coin -= nextSD.coin;
            pd.crystal -= nextSD.crystal;
            pd.strongArr[data.pos] += 1;
            pd.hp += nextSD.addhp;
            pd.ad += nextSD.addhurt;
            pd.ap += nextSD.addhurt;
            pd.addef += nextSD.adddef;
            pd.apdef += nextSD.adddef;
        }

        if (cacheSvc.UpdatePlayerData(pd.id, pd))
        {
            msg.val = new RspStrong()
            {
                coin = pd.coin,
                crystal = pd.crystal,
                hp = pd.hp,
                ad = pd.ad,
                ap = pd.ap,
                addef = pd.addef,
                apdef = pd.apdef,
                strong = pd.strongArr
            };
        }
        else
        {
            msg.err = (int)ErrorCode.UpdateDBError;
        }

        pack.session.SendMsg(msg);
    }
}