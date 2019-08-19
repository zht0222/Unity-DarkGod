using System.Xml;
using System.Collections.Generic;
using System;

public class CfgSvc
{
    public const int PowerAddSpace = 5;
    public const int PowerAddCount = 2;

    private static CfgSvc _instance = null;

    public static CfgSvc Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CfgSvc();
            }
            return _instance;
        }
    }

    public void Init()
    {
        InitGuideCfg();
        InitTaskRewardCfg();
        InitStrongCfg();
        InitMapCfg();
        Common.Log("NetSvc Init Done");
    }

    #region 任务配置
    private Dictionary<int, GuideCfg> guideDic = new Dictionary<int, GuideCfg>();
    private void InitGuideCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"D:\WENJIAN\Unity\Projects\DarkGod\Client\Assets\Resources\ResCfgs\guide.xml");

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            GuideCfg mc = new GuideCfg
            {
                ID = id
            };
            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "coin":
                        mc.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        mc.exp = int.Parse(e.InnerText);
                        break;
                }
            }
            guideDic.Add(id, mc);
        }
    }
    public GuideCfg GetGuideData(int id)
    {
        GuideCfg agc = null;
        if (guideDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }
    #endregion

    #region 任务奖励配置
    private Dictionary<int, TaskRewardCfg> taskRewardDic = new Dictionary<int, TaskRewardCfg>();
    private void InitTaskRewardCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"D:\WENJIAN\Unity\Projects\DarkGod\Client\Assets\Resources\ResCfgs\taskreward.xml");

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            TaskRewardCfg tc = new TaskRewardCfg
            {
                ID = id
            };
            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "count":
                        tc.count = int.Parse(e.InnerText);
                        break;
                    case "coin":
                        tc.coin = int.Parse(e.InnerText);
                        break;
                    case "exp":
                        tc.exp = int.Parse(e.InnerText);
                        break;
                }
            }
            taskRewardDic.Add(id, tc);
        }
    }
    public TaskRewardCfg GetTaskRewardCfg(int id)
    {
        TaskRewardCfg agc = null;
        if (taskRewardDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }
    #endregion

    #region 强化配置
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"D:\WENJIAN\Unity\Projects\DarkGod\Client\Assets\Resources\ResCfgs\strong.xml");

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            StrongCfg sc = new StrongCfg
            {
                ID = id
            };

            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                int inner = int.Parse(e.InnerText);
                switch (e.Name)
                {
                    case "pos":
                        sc.pos = inner;
                        break;
                    case "starlv":
                        sc.starlv = inner;
                        break;
                    case "addhp":
                        sc.addhp = inner;
                        break;
                    case "addhurt":
                        sc.addhurt = inner;
                        break;
                    case "adddef":
                        sc.adddef = inner;
                        break;
                    case "minlv":
                        sc.minlv = inner;
                        break;
                    case "coin":
                        sc.coin = inner;
                        break;
                    case "crystal":
                        sc.crystal = inner;
                        break;
                }
            }

            Dictionary<int, StrongCfg> dic = null;
            if (strongDic.TryGetValue(sc.pos, out dic))
            {
                dic.Add(sc.starlv, sc);
            }
            else
            {
                dic = new Dictionary<int, StrongCfg>();
                dic.Add(sc.starlv, sc);

                strongDic.Add(sc.pos, dic);
            }
        }
    }
    public StrongCfg GetStrongData(int pos, int startlv)
    {
        StrongCfg sc = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongDic.TryGetValue(pos, out dic))
        {
            if (dic.TryGetValue(startlv, out sc))
            {
                return sc;
            }
        }
        return null;
    }
    #endregion

    #region 地图配置
    private Dictionary<int, MapCfg> mapDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"D:\WENJIAN\Unity\Projects\DarkGod\Client\Assets\Resources\ResCfgs\map.xml");

        XmlNodeList nodLst = doc.SelectSingleNode("root").ChildNodes;

        for (int i = 0; i < nodLst.Count; i++)
        {
            XmlElement ele = nodLst[i] as XmlElement;

            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int id = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            MapCfg mc = new MapCfg
            {
                ID = id
            };
            foreach (XmlElement e in nodLst[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "power":
                        mc.power = int.Parse(e.InnerText);
                        break;
                }
            }
            mapDic.Add(id, mc);
        }
    }
    public MapCfg GetMapCfg(int id)
    {
        MapCfg agc = null;
        if (mapDic.TryGetValue(id, out agc))
        {
            return agc;
        }
        return null;
    }
    #endregion
}

public class StrongCfg : BaseData<StrongCfg>
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

public class MapCfg : BaseData<MapCfg>
{
    public int power;
}

public class GuideCfg : BaseData<GuideCfg>
{
    public int coin;
    public int exp;
}

public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}

public class BaseData<T>
{
    public int ID;
}

