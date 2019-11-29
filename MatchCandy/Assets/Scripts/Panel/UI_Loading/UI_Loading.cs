using Common.Messenger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_Loading : UIWndBase
    {
        [SerializeField]
        GameObject child;
        [SerializeField]
        UILabel Tip;
    
        BackAndForthWindow Manger;

        protected override void SetWndFlag()
        {
            this.curUIID = UIType.UI_Loading;
        }
        public override void InitWndOnAwake()
        {
            base.InitWndOnAwake();
            Manger = GetComponent<BackAndForthWindow>();
            child.SetActive(false);
        }

        public override void RegisterMessage()
        {
            base.RegisterMessage();
            Messenger<bool,string>.AddListener(MessengerEventDef.ShowLoading, ShowHandler);
        }
        public override void RemoveMessage()
        {
            base.RemoveMessage();
            Messenger<bool,string>.RemoveListener(MessengerEventDef.ShowLoading, ShowHandler);
        }

        void ShowHandler(bool show,string msg)
        {
            if (show)
            {
                child.SetActive(true);
                Manger.Show();
                Tip.text = msg;
            }
            else
            {
                Tip.text = "";
                Manger.Hide();
            }
        }
    }
}

