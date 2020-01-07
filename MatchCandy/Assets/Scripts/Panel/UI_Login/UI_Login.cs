using Bmob.util;
using Common.Messenger;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_Login : UIWndBase
    {
        [SerializeField]
        UILabel Tip;
        [SerializeField]
        GameObject BtnA;
        [SerializeField]
        UILabel LabelA;
        [SerializeField]
        GameObject BtnB;
        [SerializeField]
        UILabel LabelB;
        [SerializeField]
        UIInput InputA;
        [SerializeField]
        UIInput InputB;
        [SerializeField]
        UIInput InputC;


        static ActionType type = ActionType.Login;
        protected override void SetWndFlag()
        {
            this.curUIID = UIType.UI_Login;
        }

        public override void InitWndOnAwake()
        {
            base.InitWndOnAwake();
            UIEventListener.Get(BtnA).onClick = OnAClick;
            UIEventListener.Get(BtnB).onClick = OnBClick;
        }
        public override void OnShowWnd(UIWndData wndData)
        {
            base.OnShowWnd(wndData);
            type = ActionType.Login;
            InputC.gameObject.SetActive(false);
        }
        public override void RegisterMessage()
        {
            base.RegisterMessage();
            Messenger.AddListener(MessengerEventDef.Str_LoginSuccess, LoginSuccessHandler);
            Messenger.AddListener(MessengerEventDef.Str_RegisterSuccess, RegisterSuccessHandler);
        }
        public override void RemoveMessage()
        {
            base.RemoveMessage();
            Messenger.RemoveListener(MessengerEventDef.Str_LoginSuccess, LoginSuccessHandler);
            Messenger.RemoveListener(MessengerEventDef.Str_RegisterSuccess, RegisterSuccessHandler);
        }

        private void OnAClick(GameObject go)
        {
            if(type==ActionType.Login)
            {
                Tip.text = "注册";
                LabelA.text = "去登录";
                LabelB.text = "注册";
                type = ActionType.Register;
            }
            else if(type==ActionType.Register)
            {
                Tip.text = "登录才能进入哦";
                LabelA.text = "去注册";
                LabelB.text = "登录";
                type = ActionType.Login;
            }
            InputA.value = "";
            InputB.value = "";
            InputC.gameObject.SetActive(type == ActionType.Register);
        }

        private void OnBClick(GameObject go)
        {
            string uid = InputA.value;
            string pwd = InputB.value;
            string mail = InputC.value;

            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(pwd))
            {
                UIMessageMgr.ToastMsg("账号或密码不能为空!");
                return;
            }
            if(type == ActionType.Register && string.IsNullOrEmpty(mail))
            {
                UIMessageMgr.ToastMsg("邮箱不能为空!");
                return;
            }

            if(type==ActionType.Login)
            {
                BmobUtil.Singlton.Login(uid, pwd);
            }
            else if(type==ActionType.Register)
            {
                BmobUtil.Singlton.Register(uid, pwd, mail);
            }
        }

        void LoginSuccessHandler()
        {
            UIManger.HideUIWnd(UIType.UI_Login);
        }
        private void RegisterSuccessHandler()
        {
            string uid = InputA.value;
            string pwd = InputB.value;
            OnAClick(null);
            InputA.value = uid;
            InputB.value = pwd;
        }

        enum ActionType
        {
            Login, Register,
        }
    }

}

