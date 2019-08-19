/****************************************************
    文件：StateMove.cs
	作者：Zht
    日期：2019/8/14 16:31:5
	功能：移动状态
*****************************************************/

using UnityEngine;

public class StateMove : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        Debug.Log("Enter StateMove");
        entity.currentAniState = AniState.Move;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        Debug.Log("Exit StateMove");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        Debug.Log("Process StateMove");
        entity.SetBlend(Constants.BlendMove);
    }
}