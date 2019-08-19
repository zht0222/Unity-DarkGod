using Protocol;

public class LoginSys : SystemRoot
{
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    public override void InitSys() {
        base.InitSys();

        Instance = this;
        Common.Log("Init LoginSys...");
    }

    /// <summary>
    /// 进入登录场景
    /// </summary>
    public void EnterLogin()
    {
        //异步的加载登录场景
        //并显示加载的进度
        resSvc.AsyncLoadScene(Constants.SceneLogin, () => 
        {
            //加载完成以后再打开注册登录界面
            loginWnd.SetWndState();
            audioSvc.PlayBGMusic(Constants.BGLogin);
        });
    }

    public void RspLogin(GameMsg msg)
    {
        RspLogin data = msg.val as RspLogin;
        GameRoot.AddTips("登录成功");
        GameRoot.Instance.SetPlayerData(data);
        //关闭登录界面
        loginWnd.SetWndState(false);
        if (data.playerData.name == "")
        {
            createWnd.SetWndState();
        }
        else
        {
            MainCitySys.Instance.EnterMainCity();
        }
    }

    public void RspRename(GameMsg msg)
    {
        RspRename data = msg.val as RspRename;
        GameRoot.Instance.SetPlayerName(data.name);

        //关闭创建界面
        createWnd.SetWndState(false);
        //跳转场景进入主城
        MainCitySys.Instance.EnterMainCity();
    }
}