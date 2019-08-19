/****************************************************
    文件：SkillMgr.cs
	作者：Zht
    日期：2019/8/2 6:24:28
	功能：技能管理
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour 
{
    private ResSvc resSvc;
    private TimerSvc timerSvc;

    public void Init()
    {
        resSvc = ResSvc.Instance;
        timerSvc = TimerSvc.Instance;
        Debug.Log("Init SkillMgr Done.");
    }

    public void AttackEffect(EntityBase entity, int skillID)
    {
        SkillCfg data = resSvc.GetSkillCfg(skillID);
        entity.SetAction(data.aniAction);
        entity.SetFX(data.fx, data.skillTime);

        entity.canControl = false;
        entity.SetDir(Vector2.zero);
        CalcSkillMove(entity, data);
        
        timerSvc.AddTimerTask((int tid) =>
        {
            entity.Idle();
        }, data.skillTime);
    }

    private void CalcSkillMove(EntityBase entity, SkillCfg data)
    {
        List<int> skillMoveLst = data.skillMoveLst;
        float sum = 0;
        for (int i = 0; i < skillMoveLst.Count; i++)
        {
            SkillMoveCfg moveCfg = resSvc.GetSkillMoveCfg(skillMoveLst[i]);
            float speed = moveCfg.moveDis / (moveCfg.moveTime / 1000f);

            sum += moveCfg.delayTime;
            if (sum > 0)
            {
                timerSvc.AddTimerTask((int tid) =>
                {
                    entity.SetSkillMoveState(true, speed);
                }, sum);
            }
            else
            {
                entity.SetSkillMoveState(true, speed);
            }

            sum += moveCfg.moveTime;
            timerSvc.AddTimerTask((int tid) =>
            {
                entity.SetSkillMoveState(false);
            }, sum);
        }
    }
}