using PENet;
using Protocol;

public enum LogType
{
    Log,
    Warn,
    Error,
    Info
}

public class Common
{
    public static void Log(string msg = "", LogType tp = LogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }

    public static int GetFightByProps(PlayerData pd)
    {
        return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    public static int GetPowerLimit(int lv)
    {
        return ((lv - 1) / 10) * 150 + 150;
    }

    public static int GetExpUpValByLv(int lv)
    {
        return 100 * lv * lv;
    }

    public static void CalcExp(PlayerData pd, int addExp)
    {
        int curtLv = pd.lv;
        int curExp = pd.exp;
        int addRestExp = addExp;
        while (true)
        {
            int upNeedExp = Common.GetExpUpValByLv(curtLv) - curExp;
            if (addRestExp >= upNeedExp)
            {
                curtLv++;
                curExp = 0;
                addRestExp -= upNeedExp;
            }
            else
            {
                pd.lv = curtLv;
                pd.exp = curExp + addRestExp;
                break;
            }
        }
    }
}