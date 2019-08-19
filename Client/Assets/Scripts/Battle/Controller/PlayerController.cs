/****************************************************
    文件：PlayerController.cs
	作者：Zht
    日期：2019/6/10 11:32:54
	功能：角色控制器
*****************************************************/

using UnityEngine;

public class PlayerController : Controller 
{
    private Transform camTrans;
    private Vector3 camOffset;
    private float targetBlend;
    private float currentBlend;

    public CharacterController ctrl;
    public GameObject daggeratk1fx;

    public override void Init()
    {
        base.Init();

        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
        if (daggeratk1fx != null)
        {
            fxDic.Add(daggeratk1fx.name, daggeratk1fx);
        }
    }

    private void Update()
    {
        if (isMove)
        {
            SetDir();
            SetMove();
            SetCam();
        }
        if (targetBlend != currentBlend)
        {
            UpdateMixBlend();
        }
        if (skillMove)
        {
            SetSkillMove();
            SetCam();
        }
    }

    private void SetDir()
    {
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1)) + Camera.main.transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(0, angle, 0);
    }

    private void SetMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    public void SetSkillMove()
    {
        ctrl.Move(transform.forward * Time.deltaTime * skillMoveSpeed);
    }

    public void SetCam()
    {
        camTrans.position = transform.position - camOffset;
    }

    /////////////////////////////////////////////////////////////
    public override void SetBlend(float blend)
    {
        targetBlend = blend;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentBlend - targetBlend) < Constants.AccelerSpeed * Time.deltaTime)
        {
            currentBlend = targetBlend;
        }
        else if (currentBlend > targetBlend)
        {
            currentBlend -= Constants.AccelerSpeed * Time.deltaTime;
        }
        else
        {
            currentBlend += Constants.AccelerSpeed * Time.deltaTime;
        }
        ani.SetFloat("Run", currentBlend);
    }

    public override void SetFX(string name, float destory)
    {
        GameObject go = null;
        if (fxDic.TryGetValue(name, out go))
        {
            go.SetActive(true);
            timerSvc.AddTimerTask((int tid) => 
            {
                go.SetActive(false);
            }, destory);
        }
    }
}