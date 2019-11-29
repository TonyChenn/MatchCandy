using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UIWrapContentEx))]
public class UIWrapContentHelper : MonoBehaviour {

	public delegate void OnInitItem(GameObject go, int wrapIndex, int minIndex, int maxIndex);

	public int cellHeight = 200;
	[Range(1, 20)]
	public int maxColumns = 1; // 列
	[Range(1, 20)]
	public int maxRows = 1; // 行

	public UISlider verticalScrollBar;
	bool mIgnoreCallbacks = false;
	float maxCellHeight = 0f, panelClipHeight = 0f;

	private UIWrapContentEx mWrapContent;
	private bool wasInitPrefab = false;
	private Transform mTrans;
	private List<GameObject> list = new List<GameObject>();

	public OnInitItem onInitItem;

	public float ScrollBarValue
	{
		get { if (verticalScrollBar != null) return verticalScrollBar.value; return 0f; }
	}

	void Start()
	{
		if (verticalScrollBar != null)
		{
			verticalScrollBar.value = 0;
			EventDelegate.Add(verticalScrollBar.onChange, OnScrollBar);
			mWrapContent.CachedView.onMomentumMove = OnPanelMove;
		}
	}
	public void SetScrollBarIsShow(bool isShow)
	{
		if (verticalScrollBar != null)
		{
			verticalScrollBar.alpha = isShow ? 1 : 0;
		}
	}

	public Transform cachedTransform {
		get {
			if (mTrans == null)
				mTrans = transform;
			return mTrans;
		} 
	}
	public bool WasInitPrefab
	{
		get { return wasInitPrefab; }
	}
	public UIWrapContentEx WrapContent
	{
		get { return mWrapContent; }
	}

	void Awake()
	{
		if (mWrapContent == null)
			mWrapContent = this.gameObject.GetComponent<UIWrapContentEx>();
		mWrapContent.itemSize = cellHeight;
		mWrapContent.onInitializeItem = OnInitializeItem;
		mWrapContent.enabled = false;

	}

	public List<GameObject> PrepareCell(GameObject go)
	{
		list = new List<GameObject>();
		if (mWrapContent == null)
			mWrapContent = this.gameObject.GetComponent<UIWrapContentEx>();
		mWrapContent.itemSize = cellHeight;
		mWrapContent.onInitializeItem = OnInitializeItem;
		mWrapContent.enabled = false;

		GameObject newOne = null;
		for (int i = 0, iMax = maxRows; i < iMax; ++i)
		{
			newOne = NGUITools.AddChild(this.gameObject, go);
			newOne.SetActive(true);
			newOne.transform.localPosition = new Vector3(0, -cellHeight * i, 0);

			list.Add(newOne);
		}
		mWrapContent.SortBasedOnScrollMovement();
		wasInitPrefab = true;
		return list;
	}
	public void SetItemRange(int maxDataCount)
	{
        if (mWrapContent == null)
            mWrapContent = this.gameObject.GetComponent<UIWrapContentEx>();
		mWrapContent.enabled = false;
		int maxItemCount = maxRows * maxColumns;
		int maxDataColumns = Mathf.CeilToInt(maxDataCount / (float)maxColumns);
		maxCellHeight = maxDataColumns * cellHeight;
		panelClipHeight = mWrapContent.CachedPanel.baseClipRegion.w;
		if (maxDataCount >= maxItemCount)
		{
			mWrapContent.enabled = true;
			mWrapContent.maxIndex = 0;
			mWrapContent.minIndex = -maxDataColumns + 1;
			for (int i = 0, j = list.Count; i < j; ++i)
			{
				list[i].SetActive(true);
			}
		}
		else
		{
			mWrapContent.enabled = false;
			for (int i = 0, j = list.Count; i < j; ++i)
			{
				if (i > maxDataColumns - 1)
				{
					list[i].SetActive(false);
				}
				else
				{
					list[i].SetActive(true);
				}
			}
		}
	}
	public void ResetPanelAndClildPos()
	{
		mWrapContent.ResetPanelAndChildPos();
	}
	public void ResetChildPositions()
	{
		mWrapContent.ResetChildPos();
		mWrapContent.UpdateAllItem();
	}
	public void UpdateAllItem()
	{
        if(mWrapContent==null)
            mWrapContent=gameObject.GetComponent<UIWrapContentEx>();
		mWrapContent.UpdateAllItem();
	}

	private void OnInitializeItem(GameObject go, int wrapIndex, int realIndex)
	{
		//Debug.Log("  realIndex: " + realIndex + "  wrapIndex: " + wrapIndex);
		if (onInitItem != null)
		{
			int row = Mathf.Abs(realIndex);
			onInitItem(go, wrapIndex, row * maxColumns, row * maxColumns + (maxColumns - 1));
		}
	}

	public void SetScrollBarValue(float val)
	{
		if (verticalScrollBar != null)
			verticalScrollBar.value = val;
	}

	void OnScrollBar()
	{
		if (!mIgnoreCallbacks)
		{
			mIgnoreCallbacks = true;
			SetPanelClipPos(verticalScrollBar.value);
			mIgnoreCallbacks = false;
		}
	}

	void SetPanelClipPos(float mScroll)
	{
		if(maxCellHeight > panelClipHeight)
		{
			Vector2 v2 = mWrapContent.CachedPanel.clipOffset;
			v2.y = (maxCellHeight - panelClipHeight) * mScroll;
			mWrapContent.CachedPanel.cachedTransform.localPosition = v2;
			v2.y *= -1;
			mWrapContent.CachedPanel.clipOffset = v2;
		}
	}
	void OnPanelMove()
	{
		if (!mIgnoreCallbacks)
		{
			mIgnoreCallbacks = true;
			verticalScrollBar.value = GetCurrentScrollValue();
			mIgnoreCallbacks = false;
		}
	}
	float GetCurrentScrollValue()
	{
		if(mWrapContent.CachedPanel != null)
		{
			return Mathf.Clamp01(mWrapContent.CachedPanel.cachedTransform.localPosition.y / (maxCellHeight - panelClipHeight));
		}
		return 0;
	}
}
