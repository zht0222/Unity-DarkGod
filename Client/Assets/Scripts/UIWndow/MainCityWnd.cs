/****************************************************
    文件：MainCityWnd.cs
	作者：Zht
    日期：2019/6/4 15:26:57
	功能：主城UI界面
*****************************************************/

using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class MainCityWnd : WindowRoot 
{
    private bool menuState = true;
    private Vector2 startPos = Vector2.zero;
    private AutoGuideCfg curtTaskData;

    public Text txtFight;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    public Transform expPrgTrans;
    public Animation menuAni;
    //public Button btnMenu;
    public Image imgTouch;
    public Image imgDirBG;
    public Image imgDirPoint;
    public Button btnGuide;

    protected override void InitWnd()
    {
        base.InitWnd();
        SetActive(imgDirPoint, false);
        RefreshUI();
        RegisterTouchEvts();
    }

    /// <summary>
    /// 更新UI
    /// </summary>
    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PPlayerData;
        SetText(txtFight, Common.GetFightByProps(pd));
        SetText(txtPower, string.Format("体力:{0}/{1}", pd.power, Common.GetPowerLimit(pd.lv)));
        imgPowerPrg.fillAmount = pd.power * 1.0f / Common.GetPowerLimit(pd.lv);
        SetText(txtLevel, pd.lv);
        SetText(txtName, pd.name);

        //expprg
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        float globalRate = (float)Screen.height / Constants.ScreenStandardHeight;
        float width = (Screen.width - 180 * globalRate) / 10;
        grid.cellSize = new Vector2(width / globalRate, 7);

        int expPrhVal = (int)((float)pd.exp / Common.GetExpUpValByLv(pd.lv) * 100);
        SetText(txtExpPrg, expPrhVal + "%");
        int index = expPrhVal / 10;
        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = expPrhVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }

        //设置自动任务图标
        curtTaskData = resSvc.GetAutoGuideData(pd.guideid);
        if (curtTaskData != null)
        {
            SetGuideBtnIcon(curtTaskData.npcID);
        }
        else
        {
            SetGuideBtnIcon(-1);
        }
    }

    /// <summary>
    /// 设置自动任务图标
    /// </summary>
    /// <param name="npcID"></param>
    private void SetGuideBtnIcon(int npcID)
    {
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        switch (npcID)
        {
            case Constants.NPCWiseMan:
                spPath = PathDefine.WiseManHead;
                break;
            case Constants.NPCGeneral:
                spPath = PathDefine.GeneralHead;
                break;
            case Constants.NPCArtisan:
                spPath = PathDefine.ArtisanHead;
                break;
            case Constants.NPCTrader:
                spPath = PathDefine.TraderHead;
                break;
            default:
                spPath = PathDefine.TaskHead;
                break;
        }
        SetSprite(img, spPath);
    }

    /// <summary>
    /// 注册虚拟方向键事件
    /// </summary>
    public void RegisterTouchEvts()
    {
        OnClickDown(imgTouch.gameObject, (eventData)=> 
        {
            startPos = eventData.position;
            SetActive(imgDirPoint);
            imgDirBG.transform.position = eventData.position;
        });

        OnClickUp(imgTouch.gameObject, (e) =>
        {
            imgDirBG.transform.localPosition = Vector2.zero;
            imgDirPoint.transform.localPosition = Vector2.zero;
            SetActive(imgDirPoint, false);

            //方向信息传递
            MainCitySys.Instance.SetMoveDir(Vector2.zero);
        });

        OnDrag(imgTouch.gameObject, (e)=> 
        {
            Vector2 dir = e.position - startPos;
            Vector2 clampDir = Vector2.ClampMagnitude(dir, Constants.ScreenOPDis);
            imgDirPoint.transform.localPosition = clampDir;

            //方向信息传递
            MainCitySys.Instance.SetMoveDir(dir.normalized);
        });
    }

    /// <summary>
    /// 点击菜单按钮
    /// </summary>
    public void ClickMenuBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIExtenBtn);
        menuState = !menuState;
        if (menuState)
        {
            menuAni.Play("OpenMCMenu");
        }
        else
        {
            menuAni.Play("CloseMCMenu");
        }
    }

    /// <summary>
    /// 点击强化按钮
    /// </summary>
    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenStrongWnd();
    }

    /// <summary>
    /// 点击头像
    /// </summary>
    public void ClickHeadBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenInfoWnd();
    }

    public void ClickChatBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenChatWnd();
    }

    /// <summary>
    /// 点击自动任务
    /// </summary>
    public void ClickGuideBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        if (curtTaskData != null)
        {
            MainCitySys.Instance.RunTask(curtTaskData);
        }
        else
        {
            GameRoot.AddTips("更多引导任务，正在开发中...");
        }
    }

    public void ClickBuyStrBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenBuyWnd(0);
    }

    public void ClickTaskBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.OpenTaskWnd();
    }

    public void ClickFubenBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        MainCitySys.Instance.EnterFuben();
    }
}