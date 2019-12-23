using Bmob.util;
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

        /// <summary>
        /// 所有关卡信息
        /// </summary>
        List<LevelDao> levelList = new List<LevelDao>();
        List<UI_LevelRow> ui_Rows = new List<UI_LevelRow>();
        protected override void SetWndFlag()
        {
            this.curUIID = UIType.UI_MapPanel;
        }

        protected override void Awake()
        {
            base.Awake();
            levelList = LevelUtil.Singlton.LevelList;
            UIEventListener.Get(BtnBack).onClick = OnBackClick;

            contentHelper.onInitItem = OnInitItems;

            StartCoroutine(OnShowWndAsync(true));
        }

        public override void OnShowWnd(UIWndData wndData)
        {
            base.OnShowWnd(wndData);
            ShowMainPanel(false);

            contentHelper.ResetChildPositions();
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

        IEnumerator OnShowWndAsync(bool isResetPanel)
        {
            UIMessageMgr.ShowLoading(true, "正在加载用户数据");

            if (!contentHelper.WasInitPrefab)
            {
                List<GameObject> list = contentHelper.PrepareCell(ItemPrefab);
                for (int i = 0, iMax = list.Count; i < iMax; ++i)
                {
                    UI_LevelRow ctrl = list[i].GetComponent<UI_LevelRow>();
                    ui_Rows.Add(ctrl);
                }
            }
            yield return null;
            UIMessageMgr.ShowLoading(false);
        }

        void OnInitItems(GameObject go, int wrapIndex, int minIndex, int maxIndex)
        {
            ui_Rows[wrapIndex].InitData(minIndex, maxIndex, LevelUtil.Singlton.LevelList);
        }

        private void OnBackClick(GameObject go)
        {
            UIManger.HideUIWnd(this.UIID);
        }
    }
}

