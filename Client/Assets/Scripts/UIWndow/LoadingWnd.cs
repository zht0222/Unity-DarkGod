/****************************************************
    文件：LoadingWnd.cs
	作者：Zht
    日期：2019/5/29 17:40:57
	功能：Nothing
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot 
{
    public Text txtTips;
    public Text txtPrg;
    public Slider loadingSlider;

    private float fgWidth;

    protected override void InitWnd()
    {
        base.InitWnd();

        transform.SetAsLastSibling();
        SetText(txtTips, "这是一条游戏Tips");
        SetText(txtPrg, "0%");
        loadingSlider.value = 0;
    }

    public void SetProgress(float prg)
    {
        SetText(txtPrg, (int)(prg * 100) + "%");
        loadingSlider.value = prg;
    }
}