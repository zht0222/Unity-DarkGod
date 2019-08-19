/****************************************************
    文件：FubenWnd.cs
	作者：Zht
    日期：2019/7/29 7:9:22
	功能：副本选择界面
*****************************************************/

using UnityEngine;
using Protocol;
using UnityEngine.UI;

public class FubenWnd : WindowRoot 
{
    private PlayerData pd;

    public Transform pointerTrans;
    public Button[] fbBtnArr;

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PPlayerData;
        RefreshUI();
    }

    public void RefreshUI()
    {
        int fbID = pd.fuben;
        for (int i = 0; i < fbBtnArr.Length; i++)
        {
            SetActive(fbBtnArr[i].gameObject);
            if (i == fbID % 10000 - 1)
            {
                pointerTrans.SetParent(fbBtnArr[i].transform);
                pointerTrans.localPosition = new Vector3(25, 100, 0);
            }
            else if (i > fbID % 10000 - 1)
            {
                SetActive(fbBtnArr[i].gameObject, false);
            }
        }
    }

    public void ClickTaskBtn(int fbID)
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        int power = resSvc.GetMapCfgData(fbID).power;
        if (pd.power < power)
        {
            GameRoot.AddTips("体力值不足");
        }
        else
        {
            netSvc.SendMsg(new GameMsg()
            {
                cmd = (int)CMD.ReqFbFight,
                val = new ReqFbFight()
                {
                    fbID = fbID
                }
            });
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}