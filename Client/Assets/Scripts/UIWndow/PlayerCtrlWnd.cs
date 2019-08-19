/****************************************************
    文件：PlayerCtrlWnd.cs
	作者：Zht
    日期：2019/8/14 7:28:26
	功能：玩家控制界面
*****************************************************/

using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrlWnd : WindowRoot 
{
    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;
    public Transform expPrgTrans;
    public Image imgTouch;
    public Image imgDirBG;
    public Image imgDirPoint;

    private Vector2 startPos = Vector2.zero;

    public Vector2 currentDir;

    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(imgDirPoint, false);
        RefreshUI();
        RegisterTouchEvts();
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PPlayerData;
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
    }

    public void RegisterTouchEvts()
    {
        OnClickDown(imgTouch.gameObject, (eventData) =>
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
            currentDir = Vector2.zero;
            BattleSys.Instance.SetMoveDir(Vector2.zero);
        });

        OnDrag(imgTouch.gameObject, (e) =>
        {
            Vector2 dir = e.position - startPos;
            Vector2 clampDir = Vector2.ClampMagnitude(dir, Constants.ScreenOPDis);
            imgDirPoint.transform.localPosition = clampDir;

            //方向信息传递
            currentDir = dir.normalized;
            BattleSys.Instance.SetMoveDir(dir.normalized);
        });
    }

    public void ClickNormalAtk()
    {
        BattleSys.Instance.ReqReleaseSkill(0);
    }

    public void ClickSkill1Atk()
    {
        BattleSys.Instance.ReqReleaseSkill(1);
    }

    public void ClickSkill2Atk()
    {
        BattleSys.Instance.ReqReleaseSkill(2);
    }

    public void ClickSkill3Atk()
    {
        BattleSys.Instance.ReqReleaseSkill(3);
    }
}