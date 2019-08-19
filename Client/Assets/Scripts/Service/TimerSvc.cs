/****************************************************
    文件：TimerSvc.cs
	作者：Zht
    日期：2019/7/9 14:35:48
	功能：计时服务
*****************************************************/

using System;
using UnityEngine;

public class TimerSvc : MonoBehaviour 
{
    private static TimerSvc _instance;
    private PETimer pt;

    public static TimerSvc Instance
    {
        get
        {
            return _instance;
        }
    }

    public void InitSvc()
    {
        _instance = this;

        pt = new PETimer();
        pt.SetLog((info) => 
        {
            Debug.Log(info);
        });

        Debug.Log("Init TimerSvc...");
    }

    public int AddTimerTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }

    private void Update()
    {
        pt.Update();
    }
}