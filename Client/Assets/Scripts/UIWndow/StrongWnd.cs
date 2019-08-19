/****************************************************
    文件：StrongWnd.cs
	作者：Zht
    日期：2019/6/13 16:50:46
	功能：强化界面
*****************************************************/

using Protocol;
using UnityEngine;
using UnityEngine.UI;

public class StrongWnd : WindowRoot 
{
    private Image[] imgs;
    private int currentIndex;
    private PlayerData pd;
    private StrongCfg nextSd;

    public Image imgCurtPos;
    public Text txtStarLv;
    public Transform starTransGrp;
    public Text propHP1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHP2;
    public Text propHurt2;
    public Text propDef2;
    public Image propArr1;
    public Image propArr2;
    public Image propArr3;
    public Text txtNeedLv;
    public Text txtCostCoin;
    public Text txtCostCrystal;
    public Transform costTransRoot;
    public Text txtCoin;
    public Transform posBtnTrans;

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PPlayerData;
        RegClickEvts();
    }

    /// <summary>
    /// 注册点击事件
    /// </summary>
    private void RegClickEvts()
    {
        imgs = new Image[posBtnTrans.childCount];
        for (int i = 0; i < posBtnTrans.childCount; i++)
        {
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();
            OnClick(img.gameObject, (obj) =>
            {
                audioSvc.PlayUIAudio(Constants.UIClickBtn);
                ClickPosItem((int)obj); 
            }, i);
            imgs[i] = img;
        }
        ClickPosItem(0);
    }

    /// <summary>
    /// 点击posItem
    /// </summary>
    private void ClickPosItem(int index)
    {
        currentIndex = index;
        for (int i = 0; i < imgs.Length; i++)
        {
            Transform trans = imgs[i].transform;
            if (i == currentIndex)
            {
                SetSprite(imgs[i], PathDefine.ItemArrorBg);
                trans.localPosition = new Vector3(10, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 95);
            }
            else
            {
                SetSprite(imgs[i], PathDefine.ItemPlatBg);
                trans.localPosition = new Vector3(0, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 85);
            }
        }
        RefreshItem();
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    private void RefreshItem()
    {

        //金币
        SetText(txtCoin, pd.coin);
        switch (currentIndex)
        {
            case 0:
                SetSprite(imgCurtPos, PathDefine.ItemToukui);
                break;
            case 1:
                SetSprite(imgCurtPos, PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(imgCurtPos, PathDefine.ItemYaobu);
                break;
            case 3:
                SetSprite(imgCurtPos, PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(imgCurtPos, PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(imgCurtPos, PathDefine.ItemFoot);
                break;
        }
        SetText(txtStarLv, pd.strongArr[currentIndex] + "星级");

        int curtStarLv = pd.strongArr[currentIndex];
        for (int i = 0; i < starTransGrp.childCount; i++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (i < curtStarLv)
            {
                SetSprite(img, PathDefine.SpStar2);
            }
            else
            {
                SetSprite(img, PathDefine.SpStar1);
            }
        }

        int nextStartLv = curtStarLv + 1;
        int sumAddHp = resSvc.GetPropAddValPreLv(currentIndex, nextStartLv, 1);
        int sumAddHurt = resSvc.GetPropAddValPreLv(currentIndex, nextStartLv, 2);
        int sumAddDef = resSvc.GetPropAddValPreLv(currentIndex, nextStartLv, 3);
        SetText(propHP1, "生命  +" + sumAddHp);
        SetText(propHurt1, "伤害  +" + sumAddHurt);
        SetText(propDef1, "防御  +" + sumAddDef);

        nextSd = resSvc.GetStrongData(currentIndex, nextStartLv);
        if (nextSd != null)
        {
            SetActive(propHP2);
            SetActive(propHurt2);
            SetActive(propDef2);

            SetActive(costTransRoot);
            SetActive(propArr1);
            SetActive(propArr2);
            SetActive(propArr3);

            SetText(propHP2, "强化后 +" + nextSd.addhp);
            SetText(propHurt2, "+" + nextSd.addhurt);
            SetText(propDef2, "+" + nextSd.adddef);

            SetText(txtNeedLv, "需要等级：" + nextSd.minlv);
            SetText(txtCostCoin, "需要消耗：      " + nextSd.coin);

            SetText(txtCostCrystal, nextSd.crystal + "/" + pd.crystal);
        }
        else
        {
            SetActive(propHP2, false);
            SetActive(propHurt2, false);
            SetActive(propDef2, false);

            SetActive(costTransRoot, false);
            SetActive(propArr1, false);
            SetActive(propArr2, false);
            SetActive(propArr3, false);
        }
    }

    public void UpdateUI()
    {
        audioSvc.PlayUIAudio(Constants.FBItemEnter);
        ClickPosItem(currentIndex);
    }

    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    /// <summary>
    /// 点击强化按钮
    /// </summary>
    public void ClickStrongBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        if (pd.strongArr[currentIndex] < 10)
        {
            if (pd.lv < nextSd.minlv)
            {
                GameRoot.AddTips("角色等级不够");
            }
            else if (pd.coin < nextSd.coin)
            {
                GameRoot.AddTips("金币数量不够");
            }
            else if (pd.crystal < nextSd.crystal)
            {
                GameRoot.AddTips("水晶数量不够");
            }
            else
            {
                netSvc.SendMsg(new GameMsg()
                {
                    cmd = (int)CMD.ReqStrong,
                    val = new ReqStrong()
                    {
                        pos = currentIndex
                    }
                });
                GameRoot.AddTips("强化中...");
            }
        }
        else
        {
            GameRoot.AddTips("星级已经升满");
        }
    }
}