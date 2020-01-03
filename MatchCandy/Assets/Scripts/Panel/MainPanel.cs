using Bmob.util;
using Common.Messenger;
using System;
using System.Collections;
using System.Collections.Generic;
using UIAnimation.Actions;
using UnityEngine;

namespace Modules.UI
{
    public class MainPanel : UIWndBase {

        [SerializeField]
        GameObject modelPrefab;

        [SerializeField]
        GameObject[] buttonObjects;


        GameObject model;
        float width;
        float height;

        protected override void SetWndFlag()
        {
            this.curUIID = UIType.MainPanel;
        }

        protected override void Awake()
        {
            base.Awake();
            width = Screen.width;
            height = Screen.height;
            Debug.Log("Main panel is Loaded!");
        }
        public override void InitWndOnStart()
        {
            base.InitWndOnStart();

        }
        public override void OnShowWnd(UIWndData wndData)
        {
            base.OnShowWnd(wndData);
            Vector3 cur, pos;
            for (int i = 0; i < buttonObjects.Length; i++)
            {
                cur = buttonObjects[i].transform.localPosition;
                pos = new Vector3(cur.x + width / 2f, cur.y, cur.z);
                buttonObjects[i].transform.localPosition = pos;
                TweenPosition tweenPosition = buttonObjects[i].GetComponent<TweenPosition>();
                tweenPosition.from = pos;
            }
            UIEventListener.Get(buttonObjects[0]).onClick = Button0ClickHandler;
            UIEventListener.Get(buttonObjects[1]).onClick = Button1ClickHandler;
            UIEventListener.Get(buttonObjects[2]).onClick = Button2ClickHandler;
            UIEventListener.Get(buttonObjects[3]).onClick = Button3ClickHandler;

            StartCoroutine(LoadModel());
        }


        IEnumerator LoadModel()
        {
            if(modelPrefab)
            {
                model = NGUITools.AddChild(gameObject, modelPrefab);
                model.transform.localPosition = new Vector3(-width / 4f, -height / 8f, 0);
                model.transform.localScale = Vector3.one * 400;
            }
            yield return null;
        }

        #region Button
        private void Button0ClickHandler(GameObject go)
        {
            UIManger.ShowUISync(UIType.UI_Shop, null);
        }

        private void Button1ClickHandler(GameObject go)
        {
            UIManger.ShowUISync(UIType.UI_MapPanel, null);
        }

        private void Button2ClickHandler(GameObject go)
        {
            Debug.Log(2);
        }

        private void Button3ClickHandler(GameObject go)
        {
            BmobUtil.Singlton.Logout();
            UIManger.ShowUISync(UIType.StartPanel, null, this.UIID);
            Messenger.Broadcast(MessengerEventDef.Str_CheckLogin);
        }
        #endregion
    }
}

