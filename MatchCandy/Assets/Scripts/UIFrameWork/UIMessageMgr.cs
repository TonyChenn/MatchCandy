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
        Messenger<bool>.Broadcast(MessengerEventDef.ShowLoading, isShow);
    }
    #endregion
}
