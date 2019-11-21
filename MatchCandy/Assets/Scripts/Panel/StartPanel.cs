using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            UIEventListener.Get(btnEnter).onClick = OnEnterClick;
        }
        void OnEnterClick(GameObject go)
        {
            UIManger.ShowUISync(UIType.MainPanel,UIType.None, null, curUIID);
        }
    }
}

