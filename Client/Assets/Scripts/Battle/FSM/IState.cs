/****************************************************
    文件：IState.cs
	作者：Zht
    日期：2019/8/14 15:54:54
	功能：状态接口
*****************************************************/

using UnityEngine;

public interface IState 
{
    void Enter(EntityBase entity, params object[] args);
    void Process(EntityBase entity, params object[] args);
    void Exit(EntityBase entity, params object[] args);
}

public enum AniState
{
    None,
    Idle,
    Move,
    Attack,
}