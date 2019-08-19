namespace Protocol
{
    public enum CMD
    {
        None = 0,
        //登录系统相关
        ReqLogin = 101,
        RspLogin = 102,
        ReqRename = 103,
        RspRename = 104,
        //主城相关
        ReqGuide = 201,
        RspGuide = 202,
        ReqStrong = 203,
        RspStrong = 204,
        SndChat = 205,
        PshChat = 206,
        ReqBuy = 207,
        RspBuy = 208,
        PshPower = 209,
        ReqTakeTaskReward = 210,
        RspTakeTaskReward = 211,
        PshTaskPrgs = 212,

        //副本
        ReqFbFight = 301,
        RspFbFight = 302,
    }

    public enum ErrorCode
    {
        None,
        ClientDataError,//客户端数据异常
        ServerDataError,//服务器数据错误
        UpdateDBError, //跟新数据库失败

        AcctIsOnline, //账号已经上线
        WrongPass, //密码错误
        NameIsExist, //名字已经存在
        
        LackLevel,
        LackCoin,
        LackCrystal,
        LackDiamond,
        LackPower,
    }
}
