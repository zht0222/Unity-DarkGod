using System;

namespace Protocol
{
    [Serializable]
    public class ReqBuy
    {
        public int type;
        public int cost;
    }

    [Serializable]
    public class RspBuy
    {
        public int type;
        public int dimond;
        public int coin;
        public int power;
    }
}
