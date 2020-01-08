using UnityEngine;
using System.Collections;
using Common.Messenger;
using Modules.UI;
using System;

public class UIMessageMgr
{
    #region Message Tips
    public static void ShowDialog(string content,bool mask)
    {
        ShowDialog("提示", content, mask);
    }
    public static void ShowDialog(string title,string content,bool mask)
    {
        ShowDialog(title, content, null, mask);
    }
    public static void ShowDialog(string title,string content,Action ok,bool mask)
    {
        ShowDialog(title, content, ok, null, mask);
    }

    public static void ShowDialog(string title, string content, Action ok,Action cancel, bool mask)
    {
        ShowDialog(title, content, ok, cancel, null, mask);
    }
    public static void ShowDialog(string title,string content,Action ok,Action cancel,Action close, bool mask)
    {
        Messenger<string, string, Action, Action, Action, bool>.Broadcast(MessengerEventDef.ShowUIDialog, title, content, ok, cancel, close, mask);
    }
    #endregion


    #region MessageLocading
    public static void ShowLoading(bool isShow)
    {
        if (isShow)
            ShowLoading(isShow, "加载中...");
        else
            ShowLoading(isShow, "");
    }
    public static void ShowLoading(bool isShow,string msg)
    {
        Messenger<bool,string>.Broadcast(MessengerEventDef.ShowLoading, isShow, msg);
    }

    public static void ToastMsg(string msg)
    {
        ToastMsg(msg, 2f);
    }
    public static void ToastMsg(string msg,float duration)
    {
        Messenger<string, float>.Broadcast(MessengerEventDef.Str_ShowToast, msg, duration);
    }
    #endregion
}
