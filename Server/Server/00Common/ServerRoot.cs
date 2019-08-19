public class ServerRoot
{
    private static ServerRoot _instance = null;
    public static ServerRoot Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ServerRoot();
            }
            return _instance;
        }
    }

    public void Init()
    {
        //TODO 数据层
        DBMgr.Instance.Init();

        //服务层
        NetSvc.Instance.Init();
        CacheSvc.Instance.Init();
        CfgSvc.Instance.Init();
        TimerSvc.Instance.Init();

        //业务系统层
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        StrongSys.Instance.Init();
        ChatSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();
        TaskSys.Instance.Init();
        FubenSys.Instance.Init();
    }

    public void Update()
    {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }

    private int SessionID = 0;
    public int GetSessionID()
    {
        if (SessionID == int.MaxValue)
        {
            SessionID = -1;
        }
        return SessionID++;
    }
}