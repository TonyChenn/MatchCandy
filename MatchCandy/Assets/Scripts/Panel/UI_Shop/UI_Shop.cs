using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_Shop : UIWndBase
    {
        [SerializeField]
        GameObject RowPrefab;
        [SerializeField]
        UIWrapContentHelper contentHelper;
        [SerializeField]
        GameObject BtnBack;
        
        List<UI_Goods_Row> ui_Rows = new List<UI_Goods_Row>();
        protected override void SetWndFlag()
        {
            this.curUIID = UIType.UI_Shop;
        }
        public override void InitWndOnAwake()
        {
            base.InitWndOnAwake();
            UIEventListener.Get(BtnBack).onClick = go => { Close(); };

            contentHelper.onInitItem = InitItems;
        }

        public override void OnShowWnd(UIWndData wndData)
        {
            base.OnShowWnd(wndData);
            StartCoroutine(OnShowWndAsync(true));
            contentHelper.ResetChildPositions();
        }
        IEnumerator OnShowWndAsync(bool isResetPanel)
        {
            UIMessageMgr.ShowLoading(true, "正在加载商品数据");

            if (!contentHelper.WasInitPrefab)
            {
                List<GameObject> list = contentHelper.PrepareCell(RowPrefab);
                for (int i = 0, iMax = list.Count; i < iMax; ++i)
                {
                    UI_Goods_Row ctrl = list[i].GetComponent<UI_Goods_Row>();
                    ui_Rows.Add(ctrl);
                }
            }
            yield return null;
            UIMessageMgr.ShowLoading(false);
        }

        private void InitItems(GameObject go, int wrapIndex, int minIndex, int maxIndex)
        {
            ui_Rows[wrapIndex].ShowUI(minIndex, maxIndex, GoodsUtil.Singlton.GoodsList);
        }
    }
}

