using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class MainPanel : UIWndBase {

        [SerializeField]
        GameObject modelPrefab;


        GameObject model;
        float width;
        float height;

        protected override void SetWndFlag()
        {
            this.curUIID = UIType.MainPanel;
        }

        public override void InitWndOnStart()
        {
            base.InitWndOnStart();
            width = Screen.width;
            height = Screen.height;
            if (modelPrefab)
            {
                model = NGUITools.AddChild(gameObject, modelPrefab);
                model.transform.localPosition = new Vector3(-width / 4f, -height / 8f, 0);
                model.transform.localScale = Vector3.one * 400;
            }
        }
    }
}

