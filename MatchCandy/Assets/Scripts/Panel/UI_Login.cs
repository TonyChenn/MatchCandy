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
        }

        private void OnBClick(GameObject go)
        {
            string uid = InputA.value;
            string pwd = InputB.value;

            if(string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(pwd))
            {
                UIMessageMgr.ToastMsg("账号或密码不能为空!");
                return;
            }

            if(type==ActionType.Login)
            {

            }
            else if(type==ActionType.Register)
            {

            }
        }

        enum ActionType
        {
            Login, Register,
        }
    }

}

