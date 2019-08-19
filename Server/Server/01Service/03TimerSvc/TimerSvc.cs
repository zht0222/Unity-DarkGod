using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TimerSvc
{
    private static TimerSvc _instance = null;
    private static readonly string lockObj = "lockObj";
    private PETimer pt = null;
    private class TaskPack
    {
        public int tid;
        public Action<int> cb;
        public TaskPack(int tid, Action<int> cb)
        {
            this.tid = tid;
            this.cb = cb;
        }
    }
    private Queue<TaskPack> tpQue = new Queue<TaskPack>();

    public static TimerSvc Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TimerSvc();
            }
            return _instance;
        }
    }

    public void Init()
    {
        pt = new PETimer(100);
        tpQue.Clear();
        pt.SetLog((info) =>
        {
            Common.Log(info);
        });
        pt.SetHandle((cb, tid) => 
        {
            if (cb != null)
            {
                lock (lockObj)
                {
                    tpQue.Enqueue(new TaskPack(tid, cb));
                }
            }
        });

        Common.Log("TimerSvc Init Done");
    }

    public void Update()
    {
        while (tpQue.Count > 0)
        {
            TaskPack tp = null;
            lock (lockObj)
            {
                tp = tpQue.Dequeue();
            }
            if (tp != null)
            {
                tp.cb(tp.tid);
            }
        }
    }

    public int AddTimerTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }

    public long GetNowTime()
    {
        return (long)pt.GetMillisecondsTime();
    }
}