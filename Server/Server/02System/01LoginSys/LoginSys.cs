using PENet;
using Protocol;

public class LoginSys
{
    private static LoginSys _instance = null;
    private CacheSvc cacheSvc = null;

    public static LoginSys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LoginSys();
            }
            return _instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        Common.Log("LoginSys Init Done");
    }

    public void ReqLogin(MsgPack pack)
    {
        ReqLogin data = pack.gameMsg.val as ReqLogin;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin,
        };

        //已上线：返回错误信息
        if (cacheSvc.IsAcctOnLine(data.acct))
        {
            msg.err = (int)ErrorCode.AcctIsOnline;
        }
        else
        {
            PlayerData pd = cacheSvc.GetPlayerData(data.acct, data.pass);
            if (pd == null)
            {
                msg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                int powerMax = Common.GetPowerLimit(pd.lv);
                if (pd.power < powerMax)
                {
                    int addPower = PowerSys.GetAddPowerOffline(TimerSvc.Instance.GetNowTime() - pd.time);
                    if (addPower + pd.power >= powerMax)
                    {
                        pd.power = powerMax;
                    }
                    else
                    {
                        pd.power += addPower;
                    }
                }

                if (!cacheSvc.UpdatePlayerData(pd.id, pd))
                {
                    msg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    msg.val = new RspLogin
                    {
                        playerData = pd
                    };
                    cacheSvc.AcctOnline(data.acct, pack.session, pd);
                }
            }
        }

        pack.session.SendMsg(msg);
    }

    public void ReqRename(MsgPack pack)
    {
        ReqRename data = pack.gameMsg.val as ReqRename;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename
        };

        if (cacheSvc.IsNameExist(data.name))
        {
            msg.err = (int)ErrorCode.NameIsExist;
        }
        else
        {
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.session);
            playerData.name = data.name;

            if (!cacheSvc.UpdatePlayerData(playerData.id, playerData))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.val = new RspRename
                {
                    name = data.name
                };
            }
        }

        pack.session.SendMsg(msg);
    }
    
    public void ClearOfflineData(ServerSession session)
    {
        PlayerData pd = cacheSvc.GetPlayerDataBySession(session);
        if (pd != null)
        {
            pd.time = TimerSvc.Instance.GetNowTime();
            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                Common.Log("Update offline time error", LogType.Error);
            }
        }
        cacheSvc.AcctOffLine(session);
    }
}