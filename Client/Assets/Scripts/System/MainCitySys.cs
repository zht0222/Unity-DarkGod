/****************************************************
    文件：MainCitySys.cs
	作者：Zht
    日期：2019/6/4 15:39:48
	功能：主城业务系统
*****************************************************/

using UnityEngine;
using UnityEngine.AI;
using Protocol;

public class MainCitySys : SystemRoot 
{
    private static MainCitySys _instance;

    public MainCityWnd mainCityWnd;

    public static MainCitySys Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Update()
    {
        if (isNavGuide)
        {
            IsArriveNavPos();
            playerContrl.SetCam();
        }
    }

    public override void InitSys()
    {
        Common.Log("Init MainCitySys...");
        base.InitSys();
        _instance = this;
    }

    /// <summary>
    /// 进入主城
    /// </summary>
    public void EnterMainCity()
    {
        MapCfg mapData = resSvc.GetMapCfgData(Constants.MainCityMapID);
        resSvc.AsyncLoadScene(mapData.sceneName, ()=> 
        {
            Common.Log("Enter MainCity...");
            
            //角色加载
            LoadPlayer(mapData);
            
            //UI Audio
            mainCityWnd.SetWndState();
            audioSvc.PlayBGMusic(Constants.BGMainCity);
            
            //角色观察相机
            if (charCamTrans == null)
            {
                charCamTrans = GameObject.FindWithTag("CharShowCam").transform;
            }
            charCamTrans.gameObject.SetActive(false);
            
            //npc位置
            GameObject mapRoot = GameObject.Find("MapRoot");
            npcPosTrans = mapRoot.GetComponent<MainCityMap>().NPCPosTrans;
        });
    }


    public void EnterFuben()
    {
        StopNavTask();
        FubenSys.Instance.EnterFuben();
    }

    #region Player
    private Transform charCamTrans;
    private PlayerController playerContrl;
    private NavMeshAgent nav;

    public InfoWnd infoWnd;

    /// <summary>
    /// 打开信息面板
    /// </summary>
    public void OpenInfoWnd()
    {
        StopNavTask();
        //设置相机相对位置
        charCamTrans.localPosition = playerContrl.transform.position + playerContrl.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
        charCamTrans.localEulerAngles = new Vector3(0, playerContrl.transform.localEulerAngles.y + 180, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        infoWnd.SetWndState();
    }

    /// <summary>
    /// 关闭信息面板
    /// </summary>
    public void CloseInfoWnd()
    {
        if (charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
        }
        infoWnd.SetWndState(false);
    }

    /// <summary>
    /// 加载角色
    /// </summary>
    /// <param name="mapData"></param>
    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissnCityPlayerPrefab, true);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        //相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        playerContrl = player.GetComponent<PlayerController>();
        playerContrl.Init();
        nav = player.GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 设置角色移动方向
    /// </summary>
    /// <param name="dir"></param>
    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();
        if (dir == Vector2.zero)
        {
            playerContrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerContrl.SetBlend(Constants.BlendMove);
        }
        playerContrl.Dir = dir;
    }

    //设置角色旋转
    private float startRoate = 0;
    public void SetStartRoate()
    {
        startRoate = playerContrl.transform.localEulerAngles.y;
    }
    public void SetPlayerRoate(float roate)
    {
        playerContrl.transform.localEulerAngles = new Vector3(0, startRoate + roate, 0);
    }
    #endregion

    #region Guide
    private AutoGuideCfg curtTaskData;
    private Transform[] npcPosTrans;
    private bool isNavGuide = false;

    public GuideWnd guideWnd;

    public AutoGuideCfg CurtTaskData
    {
        get
        {
            return curtTaskData;
        }
    }

    /// <summary>
    /// 打开任务面板
    /// </summary>
    private void OpenGuideWnd()
    {
        guideWnd.SetWndState();
    }

    /// <summary>
    /// 开始任务
    /// </summary>
    /// <param name="agc"></param>
    public void RunTask(AutoGuideCfg agc)
    {
        curtTaskData = agc;
        if (curtTaskData.npcID != -1)
        {
            if (!IsArriveNavPos())
            {
                isNavGuide = true;
                nav.enabled = true;
                nav.speed = Constants.PlayerMoveSpeed;
                nav.angularSpeed = 360;
                nav.SetDestination(npcPosTrans[curtTaskData.npcID].position);
                playerContrl.SetBlend(Constants.BlendMove);
            }
        }
        else
        {
            OpenGuideWnd();
        }
    }

    /// <summary>
    /// 停止自动寻路
    /// </summary>
    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;
            nav.isStopped = true;
            nav.enabled = false;
            playerContrl.SetBlend(Constants.BlendIdle);
        }
    }

    /// <summary>
    /// 是否到达目标点
    /// </summary>
    private bool IsArriveNavPos()
    {
        float dis = Vector3.Distance(playerContrl.transform.position, npcPosTrans[curtTaskData.npcID].position);
        if (dis < 0.5f)
        {
            StopNavTask();
            OpenGuideWnd();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 接受任务数据
    /// </summary>
    /// <param name="msg"></param>
    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.val as RspGuide;
        string tips = "任务奖励" + Constants.Color(" 金币+" + curtTaskData.coin, TxtColor.Yellow)
            + Constants.Color(" 经验+" + curtTaskData.coin, TxtColor.Blue);
        GameRoot.AddTips(tips);
        switch (curtTaskData.actID)
        {
            case 0:
                //与智者对话
                OpenTaskWnd();
                break;
            case 1:
                //进入副本
                EnterFuben();
                break;
            case 2:
                //进入强化界面
                OpenStrongWnd();
                break;
            case 3:
                //进入体力购买
                OpenBuyWnd(0);
                break;
            case 4:
                //进入金币铸造
                OpenBuyWnd(1);
                break;
            case 5:
                //进入世界聊天
                OpenChatWnd();
                break;
        }
        GameRoot.Instance.SetPlayerDataByGuide(data);
        mainCityWnd.RefreshUI();
    }
    #endregion

    #region Strong
    public StrongWnd strongWnd;

    /// <summary>
    /// 打开强化面板
    /// </summary>
    public void OpenStrongWnd()
    {
        StopNavTask();
        strongWnd.SetWndState();
    }

    public void RspStrong(GameMsg msg)
    {
        RspStrong data = msg.val as RspStrong;
        int zhanliPre = Common.GetFightByProps(GameRoot.Instance.PPlayerData);
        GameRoot.Instance.SetPlayerDataByStrong(data);
        int zhanliNow = Common.GetFightByProps(GameRoot.Instance.PPlayerData);
        GameRoot.AddTips(Constants.Color("战力提升" + (zhanliNow - zhanliPre), TxtColor.Blue));
        strongWnd.UpdateUI();
        mainCityWnd.RefreshUI();
    }
    #endregion

    #region Chat
    public ChatWnd chatWnd;

    public void OpenChatWnd()
    {
        StopNavTask();
        chatWnd.SetWndState();
    }

    public void PshChat(GameMsg msg)
    {
        PshChat data = msg.val as PshChat;
        chatWnd.AddChatMsg(data.name, data.chat);
    }
    #endregion

    #region Buy
    public BuyWnd buyWnd;

    public void OpenBuyWnd(int type)
    {
        StopNavTask();
        buyWnd.SetBuyType(type);
        buyWnd.SetWndState();
    }

    public void RspBuy(GameMsg msg)
    {
        RspBuy data = msg.val as RspBuy;
        GameRoot.Instance.SetPlayerDataByBuy(data);
        GameRoot.AddTips("购买成功");

        mainCityWnd.RefreshUI();
        buyWnd.SetWndState(false);
    }
    #endregion

    #region Power
    public void PshPower(GameMsg msg)
    {
        PshPower data = msg.val as PshPower;
        GameRoot.Instance.SetPlayerDataByPower(data);
        if (mainCityWnd.gameObject.activeSelf)
        {
            mainCityWnd.RefreshUI();
        }
    }
    #endregion

    #region Task
    public TaskWnd taskWnd;

    public void OpenTaskWnd()
    {
        StopNavTask();
        taskWnd.SetWndState();
    }

    public void RspTakeTaskReward(GameMsg msg)
    {
        RspTakeTaskReward data = msg.val as RspTakeTaskReward;
        GameRoot.Instance.SetPlayerDataByTask(data);
        taskWnd.RefreshUI();
        mainCityWnd.RefreshUI();
    }

    public void PshTaskPrgs(GameMsg msg)
    {
        PshTaskPrgs data = msg.val as PshTaskPrgs;
        GameRoot.Instance.SetPlayerDataByTask(data);
    }
    #endregion
}