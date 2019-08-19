using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PowerSys
{
    private static PowerSys _instance = null;
    private CacheSvc cacheSvc;
    private TimerSvc timerSvc;

    public static PowerSys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PowerSys();
            }
            return _instance;
        }
    }

    public static int GetAddPowerOffline(long time)
    {
        return (int)(time / (1000 * 60 * CfgSvc.PowerAddSpace)) * CfgSvc.PowerAddCount;
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;
        timerSvc.AddTimerTask(CalcPowerAdd, CfgSvc.PowerAddSpace, PETimeUnit.Minute, 0);
        Common.Log("PowerSys Init Done");
    }

    private void CalcPowerAdd(int tid)
    {
        Common.Log("All Online Player Calc Power Incress...");
        GameMsg msg = new GameMsg()
        {
            cmd = (int)CMD.PshPower
        };

        Dictionary<ServerSession, PlayerData> onlineDic = cacheSvc.OnLineSessionDic;
        foreach (var item in onlineDic)
        {
            ServerSession session = item.Key;
            PlayerData pd = item.Value;

            int powerMax = Common.GetPowerLimit(pd.lv);
            if (pd.power >= powerMax)
            {
                continue;
            }
            else
            {
                pd.power += CfgSvc.PowerAddCount;
                if (pd.power >= powerMax)
                {
                    pd.power = powerMax;
                }
                if (!cacheSvc.UpdatePlayerData(pd.id, pd))
                {
                    msg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    msg.val = new PshPower()
                    {
                        power = pd.power
                    };
                }
                session.SendMsg(msg);
            }
        }
    }
}