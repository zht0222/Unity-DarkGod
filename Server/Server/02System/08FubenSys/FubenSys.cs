using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FubenSys
{
    private static FubenSys _instance = null;
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public static FubenSys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FubenSys();
            }
            return _instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        Common.Log("FubenSys Init Done");
    }

    public void ReqFbFight(MsgPack pack)
    {
        ReqFbFight data = pack.gameMsg.val as ReqFbFight;

        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspFbFight
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int power = cfgSvc.GetMapCfg(data.fbID).power;

        if (pd.fuben < data.fbID)
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        else if (pd.power < power)
        {
            msg.err = (int)ErrorCode.LackPower;
        }
        else
        {
            pd.power -= power;
            if (cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.val = new RspFbFight()
                {
                    fbID = data.fbID,
                    power = pd.power
                };
            }
            else
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
        }
        pack.session.SendMsg(msg);
    }
}
