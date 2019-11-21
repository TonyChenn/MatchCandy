using UnityEngine;
using System.Collections;

namespace Modules.UI
{
    public class UIWndData
    {
        /// <summary>
        /// 上一级界面ID
        /// </summary>
        public UIType PreUIID
        {
            get { return preUIID; }
        }
        /// <summary>
        /// 扩展参数
        /// </summary>
        public object ExData
        {
            get { return exData; }
        }
        public bool IsReturn
        {
            get { return m_bReturn; }
        }

        private UIType preUIID = UIType.None;
        private bool m_bReturn;
        private object exData = null;

        public UIWndData(UIType preID, bool bReturn, object data)
        {
            preUIID = preID;
            m_bReturn = bReturn;
            exData = data;
        }
    }

    /// <summary>
    /// UI 基类
    /// </summary>
    public abstract class UIWndBase : MonoBehaviour
    {
        public UIType UIID
        {
            get
            {
                if (curUIID == UIType.None)
                    SetWndFlag();
                return curUIID;
            }
        }
        /// <summary>
        /// 上一级界面ID
        /// </summary>
        public UIType PreUIID
        {
            get { return preUIID; }
        }
        /// <summary>
        /// UI是否已准备完毕
        /// </summary>
        public bool ReadyShow
        {
            get { return readyShow; }
            set { readyShow = value; }
        }
        public Transform CachedTransform { get { if (cacheTrans == null) cacheTrans = this.transform; return cacheTrans; } }
        public GameObject CachedGameObject { get { if (cacheObj == null) cacheObj = this.gameObject; return cacheObj; } }

        protected UIType curUIID = UIType.None;
        protected UIType preUIID = UIType.None;
        protected Transform cacheTrans;
        protected GameObject cacheObj;
        private bool readyShow = false;

        protected virtual void Awake()
        {
            cacheObj = this.gameObject;
            cacheTrans = this.gameObject.transform;
            SetWndFlag();
            InitWndOnAwake();
            UIManger.RegisterUI(this);
        }
        protected virtual void Start()
        {
            InitWndOnStart();
        }
        public virtual void OnShowWnd(UIWndData wndData)
        {
            RegisterMessage();
            if (!wndData.IsReturn)
                preUIID = wndData.PreUIID;
            readyShow = true;
        }

        public virtual void OnReadShow()//gameobject设置为true之后
        {
        }
        public virtual void OnHideWnd()
        {
            RemoveMessage();
        }
        public virtual void InitWndOnAwake()
        {
        }
        public virtual void InitWndOnStart()
        {
        }
        // 注册wnd显示时需要监听的消息
        public virtual void RegisterMessage()
        {
        }
        // 释放wnd显示时监听的消息
        public virtual void RemoveMessage()
        {
        }
        public virtual void SetActiveByRoot(bool state)
        {
            if (cacheObj != null)
                cacheObj.SetActive(state);
        }
        // 重连后重置ui数据 
        public virtual void ResetUIData()
        {

        }
        abstract protected void SetWndFlag();

        public void Close()
        {
            UIManger.HideUIWnd(this.curUIID);
        }
    }

}
