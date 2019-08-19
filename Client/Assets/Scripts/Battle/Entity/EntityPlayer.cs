/****************************************************
    文件：EntityPlayer.cs
	作者：Zht
    日期：2019/8/15 10:11:18
	功能：玩家逻辑实体
*****************************************************/

using UnityEngine;

public class EntityPlayer : EntityBase 
{
    public override Vector2 GetDirInput()
    {
        return battleMgr.GetDirInput();
    }
}