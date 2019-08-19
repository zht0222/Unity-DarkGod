using System;

namespace Protocol
{
    [Serializable]
    public class ReqFbFight
    {
        public int fbID;
    }

    [Serializable]
    public class RspFbFight
    {
        public int fbID;
        public int power;
    }
}
