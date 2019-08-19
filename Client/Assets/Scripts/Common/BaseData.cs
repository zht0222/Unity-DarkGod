/****************************************************
    文件：BaseData.cs
	作者：Zht
    日期：2019/6/10 15:39:52
	功能：配置数据类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class SkillMoveCfg : BaseData<SkillMoveCfg>
{
    public float delayTime;
    public float moveTime;
    public float moveDis;
}

public class SkillCfg : BaseData<SkillCfg>
{
    public string skillName;
    public float skillTime;
    public int aniAction;
    public string fx;
    public List<int> skillMoveLst;
}

public class StrongCfg: BaseData<StrongCfg>
{
    public int pos;
    public int starlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}

public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}

public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int npcID;
    public string dilogArr;
    public int actID;
    public int coin;
    public int exp;
}

public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public int power;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
}

public class BaseData<T>
{
    public int ID;
}
