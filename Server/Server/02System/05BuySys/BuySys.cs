
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BuySys
{
    private static BuySys _instance = null;
    private CacheSvc cacheSvc = null;

    public static BuySys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BuySys();
            }
            return _instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        Common.Log("BuySys Init Done");
    }

    public void ReqBuy(MsgPack pack)
    {
        ReqBuy data = pack.gameMsg.val as ReqBuy;
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.RspBuy
        };

        if (pd.diamond < data.cost)
        {
            msg.err = (int)ErrorCode.LackDiamond;
        }
        else
        {
            pd.diamond -= data.cost;
            switch (data.type)
            {
                case 0:
                    TaskSys.Instance.CalcTaskPrgs(pd, 4);
                    pd.power += 100;
                    break;
                case 1:
                    TaskSys.Instance.CalcTaskPrgs(pd, 5);
                    pd.coin += 1000;
                    break;
            }

            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.val = new RspBuy()
                {
                    type = data.type,
                    dimond = pd.diamond,
                    coin = pd.coin,
                    power = pd.power
                };
            }
        }
        pack.session.SendMsg(msg);
    }
}