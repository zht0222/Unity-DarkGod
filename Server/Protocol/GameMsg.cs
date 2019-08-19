using PENet;
using System;

namespace Protocol
{
    [Serializable]
    public class GameMsg : PEMsg
    {
        public object val;
    }

    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 2222;
    }
}
