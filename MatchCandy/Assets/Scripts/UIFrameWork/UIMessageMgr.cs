using UnityEngine;
using System.Collections;
using Common.Messenger;
using Modules.UI;
using System;

public class UIMessageMgr
{
    #region Message Tips
    public static void ShowDialog(string content)
    {
        ShowDialog("提示", content);
    }
    public static void ShowDialog(string title,string content)
    {
        ShowDialog(title, content, null);
    }
    public static void ShowDialog(string title,string content,Action ok)
    {
        ShowDialog(title, content, ok, null);
    }
    public static void ShowDialog(string title,string content,Action ok,Action cancel)
    {
        Messenger<string, string, Action, Action>.Broadcast(MessengerEventDef.ShowUIDialog, title, content, ok, cancel);
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
