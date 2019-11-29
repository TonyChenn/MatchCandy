using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_MapPanel : UIWndBase
    {
        [SerializeField]
        GameObject ItemPrefab;
        [SerializeField]
        UIWrapContentHelper contentHelper;
        [SerializeField]
        GameObject BtnBack;

        List<LevelDao> levelList;
        List<UI_LevelItem> ui_items = new List<UI_LevelItem>();
        protected override void SetWndFlag()
        {
            this.curUIID = UIType.UI_MapPanel;
        }
        protected override void Awake()
        {
            base.Awake();
            levelList = LevelUtil.Singlton.LevelList;
            contentHelper.onInitItem = OnInitItems;
            UIEventListener.Get(BtnBack).onClick = OnBackClick;
        }

        void OnInitItems(GameObject go, int wrapIndex, int minIndex, int maxIndex)
        {
            if (minIndex >= levelList.Count) { }
            else
            {
                ui_items[wrapIndex].InitItem(levelList[minIndex]);
            }
        }

        public override void OnShowWnd(UIWndData wndData)
        {
            base.OnShowWnd(wndData);
            ShowMainPanel(false);
            StartCoroutine(RefresnUI(true));
        }
        public override void OnHideWnd()
        {
            base.OnHideWnd();
            ShowMainPanel(true);
        }

        void ShowMainPanel(bool state)
        {
            UIWndBase wnd = UIManger.GetUIWnd(UIType.MainPanel);
            if (wnd != null)
                wnd.SetActiveByRoot(state);
        }
        IEnumerator RefresnUI(bool isResetPanel)
        {
            UIMessageMgr.ShowLoading(true, "正在刷新...");

            if (!contentHelper.WasInitPrefab)
            {
                List<GameObject> list = contentHelper.PrepareCell(ItemPrefab);
                for (int i = 0, iMax = list.Count; i < iMax; ++i)
                {
                    UI_LevelItem ctrl = list[i].GetComponent<UI_LevelItem>();
                    ui_items.Add(ctrl);
                }
            }
            yield return null;
            contentHelper.SetItemRange(ui_items.Count);
            if (isResetPanel)
                contentHelper.ResetPanelAndClildPos();
            else
                contentHelper.ResetChildPositions();

            UIMessageMgr.ShowLoading(false);
        }

        private void OnBackClick(GameObject go)
        {
            UIManger.HideUIWnd(this.UIID);
        }
    }
}

