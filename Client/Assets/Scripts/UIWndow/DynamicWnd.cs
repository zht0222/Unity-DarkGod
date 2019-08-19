/****************************************************
    文件：DynamicWnd.cs
	作者：Zht
    日期：2019/5/31 21:46:52
	功能：动态UI元素界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot 
{
    private Queue<string> tipsQue = new Queue<string>();
    private bool isShowTips = false;

    public Animation tipsAni;
    public Text txtTips;

    private void Update()
    {
        if (tipsQue.Count > 0 && isShowTips == false)
        {
            lock (tipsQue)
            {
                string tips = tipsQue.Dequeue();
                SetTips(tips);
            }
        }
    }

    protected override void InitWnd()
    {
        base.InitWnd();
        transform.SetAsLastSibling();
        SetActive(txtTips, false);
    }

    public void AddTips(string tips)
    {
        lock (tipsQue)
        {
            if (!tipsQue.Contains(tips))
            {
                tipsQue.Enqueue(tips);
            }
            
        }
    }

    private void SetTips(string tips)
    {
        SetActive(txtTips);
        SetText(txtTips, tips);

        isShowTips = true;
        tipsAni.Play("TipsShowAni");
        //延时关闭激活状态
        StartCoroutine(AniPlayDone(tipsAni["TipsShowAni"].length, ()=> 
        {
            SetActive(txtTips, false);
            isShowTips = false;
        }));
    }

    private IEnumerator AniPlayDone(float sec, Action cb)
    {
        yield return new WaitForSeconds(sec);
        if (cb != null)
        {
            cb();
        }
    }
}