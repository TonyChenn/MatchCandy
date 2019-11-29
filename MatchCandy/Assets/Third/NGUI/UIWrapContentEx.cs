using UnityEngine;
using System.Collections;

public class UIWrapContentEx : UIWrapContent
{
    public UIPanel CachedPanel
    {
        get
        {
            if (mPanel == null)
                mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
            return mPanel;
        }
    }
	public UIScrollView CachedView
	{
		get {
			if(mScroll == null)
			{
				mScroll = CachedPanel.GetComponent<UIScrollView>();
			}
			return mScroll; 
		}
	}

	protected override void Start()
	{
		mFirstTime = false;
	}

	public override void SortBasedOnScrollMovement()
	{
		if (!CacheScrollView()) return;
		if (mScroll != null) mScroll.GetComponent<UIPanel>().onClipMove = OnMove;
		// Cache all children and place them in order
		mChildren.Clear();
		for (int i = 0; i < mTrans.childCount; ++i)
			mChildren.Add(mTrans.GetChild(i));

		// Sort the list of children so that they are in order
		if (mHorizontal) mChildren.Sort(UIGrid.SortHorizontal);
		else mChildren.Sort(UIGrid.SortVertical);
	}
	public void ResetChildPos()
	{
        if (mScroll == null)
        {
            mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
            mScroll = mPanel.GetComponent<UIScrollView>();
        }
		mScroll.ResetPosition();
		mScroll.restrictWithinPanel = true;
	}

	public void ResetPanelAndChildPos()
	{
		if (mPanel == null || mScroll == null)
			return;
		Vector2 cr = mPanel.clipOffset;
		cr.x = 0;
		cr.y = 0;
		mPanel.clipOffset = cr;

		mPanel.cachedTransform.localPosition = Vector3.zero;
		mScroll.DisableSpring();
		mScroll.currentMomentum = Vector3.zero;
		mScroll.restrictWithinPanel = true;

		ResetChildPositions();
	}
	protected override void OnMove(UIPanel panel)
	{
		if (!enabled) return;
		base.OnMove(panel);
	}
}
