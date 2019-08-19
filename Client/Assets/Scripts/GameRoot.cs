using Protocol;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance = null;

    private void Start()
    {
        Common.Log("Game Start...");
        Instance = this;
        DontDestroyOnLoad(this);
        ClearUIRoot();
        Init();
    }

    private void Init()
    {
        //服务模块初始化
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();
        TimerSvc timer = GetComponent<TimerSvc>();
        timer.InitSvc();

        //业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        MainCitySys mainCitySys = GetComponent<MainCitySys>();
        mainCitySys.InitSys();
        FubenSys fubenSys = GetComponent<FubenSys>();
        fubenSys.InitSys();
        BattleSys battleSys = GetComponent<BattleSys>();
        battleSys.InitSys();

        //进入登录场景并加载相应UI
        login.EnterLogin();
    }

    #region UI
    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

        dynamicWnd.SetWndState();
    }

    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }
    #endregion

    #region PlayerData
    private PlayerData playerData = null;

    public PlayerData PPlayerData
    {
        get
        {
            return playerData;
        }
    }

    public void SetPlayerData(RspLogin data)
    {
        playerData = data.playerData;
    }

    public void SetPlayerName(string name)
    {
        playerData.name = name;
    }

    public void SetPlayerDataByGuide(RspGuide data)
    {
        playerData.coin = data.coin;
        playerData.lv = data.lv;
        playerData.exp = data.exp;
        playerData.guideid = data.guideid;
    }

    public void SetPlayerDataByStrong(RspStrong data)
    {
        playerData.coin = data.coin;
        playerData.crystal = data.crystal;
        playerData.hp = data.hp;
        playerData.ad = data.ad;
        playerData.ap = data.ap;
        playerData.addef = data.addef;
        playerData.apdef = data.apdef;
        playerData.strongArr = data.strong;
    }

    public void SetPlayerDataByBuy(RspBuy data)
    {
        playerData.diamond = data.dimond;
        playerData.coin = data.coin;
        playerData.power = data.power;
    }

    public void SetPlayerDataByPower(PshPower data)
    {
        playerData.power = data.power;
    }

    public void SetPlayerDataByTask(RspTakeTaskReward data)
    {
        playerData.coin = data.coin;
        playerData.lv = data.lv;
        playerData.exp = data.exp;
        playerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByTask(PshTaskPrgs data)
    {
        playerData.taskArr = data.taskArr;
    }

    public void SetPlayerDataByFbStart(RspFbFight data)
    {
        playerData.power = data.power;
    }
    #endregion

}