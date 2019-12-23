using Common;
using Common.Messenger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using LitJson;

namespace Modules.UI
{
    public class UIManger
    {
        static UIManger _instance = null;
        public static UIManger Singlton
        {
            get
            {
                if (_instance == null)
                    _instance = new UIManger();
                return _instance;
            }
        }

        public UIManger()
        {
            TextAsset jsonAsset = Resources.Load("PanelJson/panel") as TextAsset;
            string json = jsonAsset.text;
            //string json = FileUtils.ReadFile(Application.dataPath + "/Scripts/UIFrameWork/PanelJson/panel.json");
            panelPathDict.Clear();
            if(!string.IsNullOrEmpty(json))
            {
                JsonData jd = LitJson.JsonMapper.ToObject(json);
                foreach (JsonData item in jd)
                {
                    string name = item["Name"].ToString();
                    string path = item["Path"].ToString();
                    UIType type = (UIType)Enum.Parse(typeof(UIType), name);
                    panelPathDict[type] = path;
                }
            }
        }

        public static UITypeEnumComparer UITypeComparer = new UITypeEnumComparer();
        // 加载过的UI池
        Dictionary<UIType, UIWndBase> m_mapAllUIWnd = new Dictionary<UIType, UIWndBase>(UITypeComparer);
        //UI路径
        Dictionary<UIType, string> panelPathDict = new Dictionary<UI.UIType, string>();
        List<UIType> curCacheUIID = new List<UIType>();
        List<UIType> curShowUIID = new List<UIType>();

        public List<UIType> CurShowUIIDs
        {
            get { return curShowUIID; }
        }
        public Dictionary<UIType, UIWndBase> AllUIWnd
        {
            get { return m_mapAllUIWnd; }
        }

        #region StaticFun

        public static void RegisterUI(UIWndBase uiWnd)
        {
            Singlton.RegisterWnd(uiWnd);
        }

        public static void HideUIWnd(UIType hideID)
        {
            HideUIWnd(new UIType[] { hideID });
        }

        public static void HideUIWnd(UIType[] arHideID)
        {
            Singlton.HideWndSync(arHideID);
        }

        public static void ResetDataByShowUI()
        {
            for (int i = 0; i < Singlton.curShowUIID.Count; ++i)
            {
                if (Singlton.m_mapAllUIWnd.ContainsKey(Singlton.curShowUIID[i]))
                    Singlton.m_mapAllUIWnd[Singlton.curShowUIID[i]].ResetUIData();
            }
        }

        /// <summary>
        /// 同步销毁所有加载过的UIWnd
        /// </summary>
        public static void CleanAllWnd()
        {
            Singlton.CleanAllWndSync();
        }
        /// <summary>
        /// UI是否在加载中
        /// </summary>
        public static bool IsUICaching(UIType uiID)
        {
            if (uiID != UIType.None)
            {
                for (int i = 0; i < Singlton.curCacheUIID.Count; ++i)
                {
                    if (Singlton.curCacheUIID[i] == uiID)
                        return true;
                }
            }
            return false;
        }

        public static bool IsUIShowing(UIType uiType)
        {
            if (uiType != UIType.None)
            {
                for (int i = 0, j = Singlton.curShowUIID.Count; i < j; ++i)
                {
                    if (Singlton.curShowUIID[i] == uiType)
                        return true;
                }
            }
            return false;
        }
        public static List<UIType> GetAllShowingUI()
        {
            List<UIType> result = new List<UIType>();
            for (int i = 0, j = Singlton.curShowUIID.Count; i < j; ++i)
            {
                result.Add(Singlton.curShowUIID[i]);
            }
            return result;
        }

        public static UIWndBase GetUIWnd(UIType uiType)
        {
            if (uiType != UIType.None)
            {
                if (Singlton.m_mapAllUIWnd.ContainsKey(uiType))
                {
                    return Singlton.m_mapAllUIWnd[uiType];
                }
            }
            return null;
        }

        /// <summary>
        /// 同步显示UI
        /// </summary>
        /// <param name="showID"> 显示ID</param>
        /// <param name="preID"> 前置ID</param>
        /// <param name="bReturn"> 如为true 忽略preID</param>
        /// <param name="exData"> 传给显示UI的参数</param>
        /// <param name="hideID"> 需要隐藏的UI</param>
        /// 
        public static void ShowUISync(UIType showID, object exData)
        {
            ShowUISync(showID, UIType.None, false, exData, UIType.None);
        }
        public static void ShowUISync(UIType showID,object exData,UIType hideID)
        {
            ShowUISync(showID, UIType.None, null, hideID);
        }

        public static void ShowUISync(UIType showID, UIType preID, object exData, UIType hideID)
        {
            ShowUISync(showID, preID, false, exData, hideID);
        }

        public static void ShowUISync(UIType showID, UIType preID, bool bReturn, object exData, UIType hideID)
        {
            Singlton.ShowWnd(showID, preID, bReturn, exData, new UIType[] { hideID });
        }

        void ShowWnd(UIType showID, UIType preID, bool bReturn, object exData, UIType[] hideId)
        {
            UIMessageMgr.ShowLoading(true);
            if (!IsUICaching(showID))
            {
                if(showID!=UIType.None)
                {
                    UIWndBase wnd = GetUIWnd(showID);
                    if(wnd==null && !IsUIShowing(showID))
                    {
                        curCacheUIID.Add(showID);
                        string path;
                        panelPathDict.TryGetValue(showID, out path);
                        if(!string.IsNullOrEmpty(path))
                        {
                            var obj = Resources.Load<GameObject>(path);
                            GameObject uiroot = GameObject.Find("UI Root");
                            var panel = NGUITools.AddChild(uiroot, obj);
                            curShowUIID.Add(showID);
                            UIWndData data = new UIWndData(preID, bReturn, exData);
                            wnd = panel.GetComponent<UIWndBase>();
                            wnd.OnShowWnd(data);
                            wnd.OnReadShow();
                            curCacheUIID.Add(showID);
                        }
                        if (wnd != null) wnd.SetActiveByRoot(true);
                        else
                            Debug.LogError("Prepare UI Error: " + showID);
                    }
                    HideWndSync(hideId);
                }
            }
            UIMessageMgr.ShowLoading(false);
        }
        #endregion

        void RegisterWnd(UIWndBase uiWnd)
        {
            if (m_mapAllUIWnd.ContainsKey(uiWnd.UIID))
            {
                Debug.LogError("ui already register, please check : " + uiWnd.UIID.ToString());
            }
            else
            {
                m_mapAllUIWnd.Add(uiWnd.UIID, uiWnd);
                curCacheUIID.Remove(uiWnd.UIID);
            }
        }

        void CleanAllWndSync()
        {
            int showIDCount = curShowUIID.Count;
            for (int i = 0; i < showIDCount; ++i)
            {
                UIType hideID = curShowUIID[i];
                UIWndBase hideWnd = GetUIWnd(hideID);
                if (hideWnd != null)
                {
                    hideWnd.OnHideWnd();
                    hideWnd.SetActiveByRoot(false);
                }
            }

            curShowUIID.Clear();
            curCacheUIID.Clear();
            var ie1 = m_mapAllUIWnd.GetEnumerator();
            while (ie1.MoveNext())
            {
                GameObject.Destroy(ie1.Current.Value.CachedGameObject);
            }
            m_mapAllUIWnd.Clear();
        }

        public static void DestroyWnd(UIType uiID)
        {
            if (Singlton != null)
                Singlton.DestroyWndSync(uiID);
        }

        void DestroyWndSync(UIType uiID)
        {
            if (curShowUIID.Contains(uiID))
            {
                HideWndSync(new UIType[] { uiID });
            }
            UIWndBase wnd;
            if (m_mapAllUIWnd.TryGetValue(uiID, out wnd))
            {
                GameObject.Destroy(wnd.CachedGameObject);
                m_mapAllUIWnd.Remove(uiID);
            }
        }

        void HideWndSync(UIType[] arHideID)
        {
            if (arHideID != null)
            {
                for (int i = 0, hideLength = arHideID.Length; i < hideLength; ++i)
                {
                    UIType hideID = arHideID[i];
                    if (IsUIShowing(hideID))
                    {
                        curShowUIID.Remove(hideID);

                        UIWndBase hideWnd = GetUIWnd(hideID);
                        if (hideWnd != null)
                        {
                            hideWnd.OnHideWnd();
                            DestroyWndSync(hideID);
                        }
                    }
                    curCacheUIID.Remove(hideID);
                }
            }
        }

        public static int GetUIMaxDepth(Transform root)
        {
            UIPanel[] panels = root.GetComponentsInChildren<UIPanel>(true);
            if (panels == null || panels.Length < 1)
                return 0;

            Array.Sort(panels, (a, b) => a.depth - b.depth);
            UIPanel lastPanel = panels.LastOrDefault();
            return lastPanel != null ? lastPanel.depth : 0;
        }
    }
    
    public class UITypeEnumComparer : IEqualityComparer<UIType>
    {
        public bool Equals(UIType x, UIType y)
        {
            return x == y;
        }

        public int GetHashCode(UIType x)
        {
            return (int)x;
        }
    }
}