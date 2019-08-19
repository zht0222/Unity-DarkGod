/****************************************************
    文件：StateAttack.cs
	作者：Zht
    日期：2019/8/16 9:9:48
	功能：攻击状态
*****************************************************/

using UnityEngine;

public class StateAttack : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        Debug.Log("Enter StateAttack");
        entity.currentAniState = AniState.Attack;
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        Debug.Log("Exit StateAttack");
        entity.SetAction(Constants.ActionDefault);
        entity.canControl = true;
    }

    public void Process(EntityBase entity, params object[] args)
    {
        Debug.Log("Process StateAttack");
        entity.AttackEffect((int)args[0]);
    }
}