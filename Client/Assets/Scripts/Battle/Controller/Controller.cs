/****************************************************
    文件：Controller.cs
	作者：Zht
    日期：2019/8/15 10:57:49
	功能：表现实体控制器抽象基类
*****************************************************/

using UnityEngine;
using System.Collections.Generic;

public abstract class Controller : MonoBehaviour 
{
    protected TimerSvc timerSvc;
    protected Vector2 dir = Vector2.zero;
    protected bool isMove = false;
    protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();
    protected bool skillMove = false;
    protected float skillMoveSpeed = 0f;

    public Animator ani;

    public Vector2 Dir
    {
        get
        {
            return dir;
        }
        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }
    }

    public virtual void Init()
    {
        timerSvc = TimerSvc.Instance;
    }

    public virtual void SetBlend(float blend)
    {
        ani.SetFloat("Run", blend);
    }

    public virtual void SetAction(int act)
    {
        ani.SetInteger("Action", act);
    }

    public virtual void SetFX(string name, float destory)
    {

    }

    public void SetSkillMoveState(bool move, float skillSpeed = 0f)
    {
        skillMove = move;
        skillMoveSpeed = skillSpeed;
    }
}