/****************************************************
    文件：StateIdle.cs
	作者：Zht
    日期：2019/8/14 16:1:15
	功能：待机状态
*****************************************************/

using UnityEngine;

public class StateIdle : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        Debug.Log("Enter StateIdle");
        entity.currentAniState = AniState.Idle;
        entity.SetDir(Vector2.zero);
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        Debug.Log("Exit StateIdle");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        Debug.Log("Process StateIdle");
        if (entity.GetDirInput() != Vector2.zero)
        {
            entity.Move();
            entity.SetDir(entity.GetDirInput());
        }
        else
        {
            entity.SetBlend(Constants.BlendIdle);
        }
    }
}