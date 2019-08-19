using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;

public class TaskSys
{
    private static TaskSys _instance = null;
    private CacheSvc cacheSvc = null;
    private CfgSvc cfgSvc = null;

    public static TaskSys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TaskSys();
            }
            return _instance;
        }
    }

    private TaskRewardData CalcTaskRewardData(PlayerData pd, int rid)
    {
        TaskRewardData trd = null;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            if (int.Parse(taskInfo[0]) == rid)
            {
                trd = new TaskRewardData()
                {
                    ID = rid,
                    prgs = int.Parse(taskInfo[1]),
                    taked = taskInfo[2].Equals("1")
                };
                break;
            }
        }
        return trd;
    }

    private void CalcTaskArr(PlayerData pd, TaskRewardData trd)
    {
        string result = trd.ID + "|" + trd.prgs + "|" + (trd.taked ? 1 : 0);
        int index = -1;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            if (int.Parse(taskInfo[0]) == trd.ID)
            {
                index = i;
                break;
            }
        }
        pd.taskArr[index] = result;
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvc = CfgSvc.Instance;
        Common.Log("TaskSys Init Done");
    }

    public void ReqTakeTaskReward(MsgPack pack)
    {
        ReqTakeTaskReward data = pack.gameMsg.val as ReqTakeTaskReward;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspTakeTaskReward
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        TaskRewardCfg trc = cfgSvc.GetTaskRewardCfg(data.rid);
        TaskRewardData trd = CalcTaskRewardData(pd, data.rid);

        if (trd.prgs == trc.count && !trd.taked)
        {
            pd.coin += trc.coin;
            Common.CalcExp(pd, trc.exp);
            trd.taked = true;
            CalcTaskArr(pd, trd);
            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.val = new RspTakeTaskReward()
                {
                    coin = pd.coin,
                    lv = pd.lv,
                    exp = pd.exp,
                    taskArr = pd.taskArr
                };
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }

        pack.session.SendMsg(msg);
    }

    public void CalcTaskPrgs(PlayerData pd, int tid)
    {
        TaskRewardData trd = CalcTaskRewardData(pd, tid);
        TaskRewardCfg trc = cfgSvc.GetTaskRewardCfg(tid);

        if (trd.prgs < trc.count)
        {
            trd.prgs++;
            CalcTaskArr(pd, trd);
            ServerSession session = cacheSvc.GetServerSession(pd.id);
            if (session != null)
            {
                session.SendMsg(new GameMsg()
                {
                    cmd = (int)CMD.PshTaskPrgs,
                    val = new PshTaskPrgs()
                    {
                        taskArr = pd.taskArr
                    }
                });
            }
        }
    }
}
