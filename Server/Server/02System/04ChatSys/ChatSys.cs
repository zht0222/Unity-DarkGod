using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ChatSys
{
    private static ChatSys _instance = null;
    private CacheSvc cacheSvc = null;

    public static ChatSys Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ChatSys();
            }
            return _instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        Common.Log("ChatSys Init Done");
    }

    public void SndChat(MsgPack pack)
    {
        SndChat data = pack.gameMsg.val as SndChat;
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        TaskSys.Instance.CalcTaskPrgs(pd, 6);

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshChat,
            val = new PshChat()
            {
                name = pd.name,
                chat = data.chat
            }
        };

        List<ServerSession> lst = cacheSvc.GetOnlineServerSession();
        byte[] bytes = PENet.PETool.PackNetMsg(msg);
        for (int i = 0; i < lst.Count; i++)
        {
            lst[i].SendMsg(bytes);
        }
    }
}