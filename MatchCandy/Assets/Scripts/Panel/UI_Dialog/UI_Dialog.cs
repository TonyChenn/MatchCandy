using Common.Messenger;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_Dialog : UIWndBase
    {
        [SerializeField]
        Transform Root;
        [SerializeField]
        UISprite Mask;
        [SerializeField]
        UILabel Title;
        [SerializeField]
        UILabel Content;
        [SerializeField]
        GameObject BtnClose;

        [SerializeField]
        Transform OneButtonGroup;
        [SerializeField]
        GameObject OneOkClick;

        [SerializeField]
        Transform TwoButtonGroup;
        [SerializeField]
        GameObject TwoOkClick;
        [SerializeField]
        GameObject TwoCancelClick;

        Action okCallBack = null;
        Action cancelCallBack = null;
        Action closeCallBack = null;

        protected override void SetWndFlag()
        {
            this.curUIID = UIType.UI_Dialog;
        }
        public override void InitWndOnAwake()
        {
            base.InitWndOnAwake();
            Root.gameObject.SetActive(false);
            UIEventListener.Get(BtnClose).onClick = CloseClickHandler;
            UIEventListener.Get(OneOkClick).onClick = OKClickHandler;
            UIEventListener.Get(TwoOkClick).onClick = OKClickHandler;
            UIEventListener.Get(TwoCancelClick).onClick = CancelClickHandler;
        }
        public override void RegisterMessage()
        {
            base.RegisterMessage();
            Messenger<string, string, Action, Action, Action, bool>.AddListener(MessengerEventDef.ShowUIDialog, ShowDialog);
        }
        public override void RemoveMessage()
        {
            base.RemoveMessage();
            Messenger<string, string, Action, Action, Action, bool>.RemoveListener(MessengerEventDef.ShowUIDialog, ShowDialog);
        }

        #region ButtonHandler
        void CancelClickHandler(GameObject go)
        {
            Root.gameObject.SetActive(false);
            if (cancelCallBack != null)
            {
                cancelCallBack();
                clearCallBack();
            }
        }

        void OKClickHandler(GameObject go)
        {
            Root.gameObject.SetActive(false);
            if (okCallBack != null)
            {
                okCallBack();
                clearCallBack();
            }
        }

        void CloseClickHandler(GameObject go)
        {
            Root.gameObject.SetActive(false);
            closeCallBack?.Invoke();
            clearCallBack();
        }
        #endregion

        void clearCallBack()
        {
            okCallBack = null;
            cancelCallBack = null;
            closeCallBack = null;
        }
        void ShowDialog(string title, string content, Action ok, Action cancel,Action close,bool mask)
        {
            Mask.gameObject.SetActive(mask);
            closeCallBack = close;
            if(ok==null && cancel==null)
            {
                OneButtonGroup.gameObject.SetActive(false);
                TwoButtonGroup.gameObject.SetActive(false);
            }
            else if(ok!=null && cancel==null)
            {
                OneButtonGroup.gameObject.SetActive(true);
                TwoButtonGroup.gameObject.SetActive(false);
                okCallBack = ok;
            }
            else if(ok!=null && cancel!=null)
            {
                OneButtonGroup.gameObject.SetActive(false);
                TwoButtonGroup.gameObject.SetActive(true);
                okCallBack = ok;
                cancelCallBack = cancel;
            }
            Title.text = title;
            Content.text = content;
            Root.gameObject.SetActive(true);
        }
    }
}

