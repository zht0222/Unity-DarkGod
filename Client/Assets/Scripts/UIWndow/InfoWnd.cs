/****************************************************
    文件：InfoWnd.cs
	作者：Zht
    日期：2019/6/12 9:52:52
	功能：角色信息展示界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;
using Protocol;

public class InfoWnd : WindowRoot 
{
    private Vector2 startPos;

    //角色信息
    public RawImage rawImage;
    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;
    public Text txtJob;
    public Text txtFight;
    public Text txtHP;
    public Text txtHurt;
    public Text txtDef;
    //详细属性
    public Transform transDetail;
    public Text dtxhp;
    public Text dtxad;
    public Text dtxap;
    public Text dtxaddef;
    public Text dtxapdef;
    public Text dtxdodge;
    public Text dtxpierce;
    public Text dtxcritical;

    protected override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
        RegTouchEvts();
    }

    /// <summary>
    /// 更新UI
    /// </summary>
    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PPlayerData;
        //角色属性
        SetText(txtInfo, pd.name + "LV." + pd.lv);
        SetText(txtExp, pd.exp + "/" + Common.GetExpUpValByLv(pd.lv));
        imgExpPrg.fillAmount = pd.exp * 1.0f / Common.GetExpUpValByLv(pd.lv);
        SetText(txtPower, pd.power + "/" + Common.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.power * 1.0f / Common.GetPowerLimit(pd.lv);
        SetText(txtJob, " 职业   暗夜刺客");
        SetText(txtFight, " 战力   " + Common.GetFightByProps(pd));
        SetText(txtHP, " 血量   " + pd.hp);
        SetText(txtHurt, " 伤害   " + (pd.ad + pd.ap));
        SetText(txtDef, " 防御   " + (pd.apdef + pd.addef));
        //详细熟悉
        SetText(dtxhp, pd.hp);
        SetText(dtxad, pd.ad);
        SetText(dtxap, pd.ap);
        SetText(dtxaddef, pd.addef);
        SetText(dtxapdef, pd.apdef);
        SetText(dtxdodge, pd.dodge + "%");
        SetText(dtxpierce, pd.pierce + "%");
        SetText(dtxcritical, pd.critical + "%");
    }

    /// <summary>
    /// 旋转角色
    /// </summary>
    private void RegTouchEvts()
    {
        OnClickDown(rawImage.gameObject, (e) =>
        {
            startPos = e.position;
            MainCitySys.Instance.SetStartRoate();
        });

        OnDrag(rawImage.gameObject, (e) =>
        {
            float roate = (e.position.x - startPos.x) * -0.4f;
            MainCitySys.Instance.SetPlayerRoate(roate);
        });
    }

    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.CloseInfoWnd();
    }

    /// <summary>
    /// 点击详细按钮
    /// </summary>
    public void ClickDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        SetActive(transDetail);
    }

    /// <summary>
    /// 点击关闭详细按钮
    /// </summary>
    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetail, false);
    }
}