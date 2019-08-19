using Protocol;
using System.Collections.Generic;

public class CacheSvc
{
    private static CacheSvc _instance;
    private DBMgr dbMgr;
  
    public static CacheSvc Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CacheSvc();
            }
            return _instance;
        }
    }

    public void Init()
    {
        dbMgr = DBMgr.Instance;
        Common.Log("CacheSvc Init Done");
    }

    #region 登陆系统数据
    private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    public Dictionary<ServerSession, PlayerData> OnLineSessionDic { get => onLineSessionDic; }

    public bool IsAcctOnLine(string acct)
    {
        return onLineAcctDic.ContainsKey(acct);
    }

    public void AcctOnline(string acct, ServerSession session, PlayerData playerData)
    {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }

    public void AcctOffLine(ServerSession session)
    {
        foreach (var item in onLineAcctDic)
        {
            if (item.Value == session)
            {
                onLineAcctDic.Remove(item.Key);
                break;
            }
        }
        bool succ = onLineSessionDic.Remove(session);
        Common.Log(string.Format("Offline Result: SessionID: {0} disconnect {1}", session.sessionID, succ));
    }

    public List<ServerSession> GetOnlineServerSession()
    {
        List<ServerSession> lst = new List<ServerSession>();
        foreach (var item in onLineAcctDic.Values)
        {
            lst.Add(item);
        }
        return lst;
    }

    public ServerSession GetServerSession(int ID)
    {
        ServerSession serverSession = null;
        foreach (var item in onLineSessionDic)
        {
            if (item.Value.id == ID)
            {
                serverSession = item.Key;
                break;
            }
        }

        return serverSession;
    }

    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        PlayerData playerData = null;
        onLineSessionDic.TryGetValue(session, out playerData);
        return playerData;
    }

    public PlayerData GetPlayerData(string acct, string pass)
    {
        return dbMgr.QueryPlayerData(acct, pass);
    }

    public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        return dbMgr.UpdatePlayerData(id, playerData);
    }

    public bool IsNameExist(string name)
    {
        return dbMgr.QueryNameData(name);
    }
    #endregion

}