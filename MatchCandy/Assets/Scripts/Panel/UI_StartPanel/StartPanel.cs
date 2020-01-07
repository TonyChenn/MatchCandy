using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bmob.util;
using cn.bmob.io;
using Common.Messenger;
using System;

namespace Modules.UI
{
    public class StartPanel : UIWndBase
    {

        [SerializeField]
        GameObject btnEnter;

        GameObject go;
        float width;
        float height;

        protected override void SetWndFlag()
        {
            this.curUIID = UIType.StartPanel;
        }
        public override void InitWndOnAwake()
        {
            base.InitWndOnAwake();
            Debug.Log("StartPanel is Loaded!");
            UIEventListener.Get(btnEnter).onClick = OnEnterClick;
        }
        public override void RegisterMessage()
        {
            base.RegisterMessage();
            Messenger.AddListener(MessengerEventDef.Str_CheckLogin, checkLogin);
        }
        public override void RemoveMessage()
        {
            base.RemoveMessage();
            Messenger.RemoveListener(MessengerEventDef.Str_CheckLogin, checkLogin);
        }



        void checkLogin()
        {
            if (BmobUtil.Singlton.CurUser == null)
            {
                //显示登录框
                UIManger.ShowUISync(UIType.UI_Login, null);

                //UIMessageMgr.ToastMsg("自动登录成功");
            }
            else
            {
                UIMessageMgr.ToastMsg("自动登录成功");
            }
        }

        void OnEnterClick(GameObject go)
        {
            UIManger.ShowUISync(UIType.MainPanel,UIType.None, null, curUIID);
        }
    }
}

