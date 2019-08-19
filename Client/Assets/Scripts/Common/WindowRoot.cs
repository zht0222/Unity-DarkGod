using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    protected ResSvc resSvc = null;
    protected AudioSvc audioSvc = null;
    protected NetSvc netSvc = null;
    protected TimerSvc timerSvc = null;

    public void SetWndState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            SetActive(gameObject, isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }

    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
        timerSvc = TimerSvc.Instance;
    }

    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
        netSvc = null;
    }

    #region Tool Functions
    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true)
    {
        img.transform.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true)
    {
        txt.transform.gameObject.SetActive(state);
    }

    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }

    protected T GetOrAddComponent<T>(GameObject go) 
        where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null) {
            t = go.AddComponent<T>();
        }
        return t;
    }
   
    protected void SetSprite(Image img, string path)
    {
        Sprite sp = resSvc.LoadSprite(path);
        img.sprite = sp;
    }

    protected Transform GetTrans(Transform trans, string name)
    {
        if (trans != null)
        {
            return trans.Find(name);
        }
        else
        {
            return transform.Find(name);
        }
    }
    #endregion

    #region Click Evts
    protected void OnClick(GameObject go, Action<object> cb, object arg = null)
    {
        UIEventListener listener = GetOrAddComponent<UIEventListener>(go);
        listener.arg = arg;
        listener.onClick = cb;
    }
    protected void OnClickDown(GameObject go, Action<PointerEventData> cb)
    {
        UIEventListener listener = GetOrAddComponent<UIEventListener>(go);
        listener.onClickDown = cb;
    }
    protected void OnClickUp(GameObject go, Action<PointerEventData> cb)
    {
        UIEventListener listener = GetOrAddComponent<UIEventListener>(go);
        listener.onClickUp = cb;
    }
    protected void OnDrag(GameObject go, Action<PointerEventData> cb)
    {
        UIEventListener listener = GetOrAddComponent<UIEventListener>(go);
        listener.onDrag = cb;
    }
    #endregion
}