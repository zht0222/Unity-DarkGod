/****************************************************
    文件：BattleMgr.cs
	作者：Zht
    日期：2019/8/1 7:55:42
	功能：战斗管理
*****************************************************/

using UnityEngine;

public class BattleMgr : MonoBehaviour 
{
    private ResSvc resSvc;
    private AudioSvc audioSvc;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;
    private EntityPlayer entitySelfPlayer;

    public void Init(int mapID)
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;

        //初始化各个管理器
        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();

        MapCfg mapData = resSvc.GetMapCfgData(mapID);
        resSvc.AsyncLoadScene(mapData.sceneName, () => 
        {
            audioSvc.PlayBGMusic(Constants.BGHuangYe);

            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            mapMgr = map.GetComponent<MapMgr>();
            mapMgr.Init();

            Camera.main.transform.position = mapData.mainCamPos;
            Camera.main.transform.eulerAngles = mapData.mainCamRote;

            LoadPlayer(mapData);
        });
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissnBattlePlayerPrefab);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;

        entitySelfPlayer = new EntityPlayer()
        {
            battleMgr = this,
            stateMgr = stateMgr,
            skillMgr = skillMgr
        };
        PlayerController playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        entitySelfPlayer.controller = playerCtrl;
        entitySelfPlayer.Idle();
    }

    #region Move
    public void SetSelfPlayerMoveDir(Vector2 dir)
    {
        if (entitySelfPlayer.canControl == false)
        {
            return;
        }

        if (dir == Vector2.zero)
        {
            entitySelfPlayer.Idle();
        }
        else
        {
            entitySelfPlayer.Move();
        }
        entitySelfPlayer.SetDir(dir);
    }

    public Vector2 GetDirInput()
    {
        return BattleSys.Instance.GetDirInput();
    }
    #endregion

    #region Skill
    public void ReqReleaseSkill(int index)
    {
        switch (index)
        {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
        }
    }

    public void ReleaseNormalAtk()
    {
        Debug.Log("common skill");
    }

    public void ReleaseSkill1()
    {
        Debug.Log("1 skill");
        entitySelfPlayer.Attack(101);
    }

    public void ReleaseSkill2()
    {
        Debug.Log("2 skill");
    }

    public void ReleaseSkill3()
    {
        Debug.Log("3 skill");
    }
    #endregion
}