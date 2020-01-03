using Bmob.util;
using Common.Messenger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Goods_item : MonoBehaviour
{
    [SerializeField]
    UILabel Name;
    [SerializeField]
    UISprite Icon;
    [SerializeField]
    UILabel Price;
    [SerializeField]
    GameObject BtnBuy;

    GoodsDao config = null;

	void Start ()
    {
        UIEventListener.Get(BtnBuy).onClick = (go) =>
        {
            GameUser curUser = BmobUtil.Singlton.CurUser;
            int coin = curUser.coin.Get();
            if (coin < config.Price)
                UIMessageMgr.ToastMsg("余额不足");
            else
            {
                coin -= config.Price;
                curUser.coin = coin;
                int count = 0;

                switch (config.id)
                {
                    case 1:
                        count = curUser.clearRow.Get();
                        ++count;
                        curUser.clearRow = count;
                        break;
                    case 2:
                        count = curUser.clearCol.Get();
                        ++count;
                        curUser.clearCol = count;
                        break;
                    case 3:
                        count = curUser.clock.Get();
                        ++count;
                        curUser.clock = count;
                        break;
                    case 4:
                        count = curUser.hammer.Get();
                        ++count;
                        curUser.hammer = count;
                        break;
                }
                PlayerPrefsUtil.CoinCount = coin;
                BmobUtil.Singlton.UpdateUserInfo(curUser);
                UIMessageMgr.ToastMsg("购买成功");
            }
        };
	}

    public void ShowItemUI(GoodsDao good)
    {
        config = good;
        Name.text = good.Name;
        Price.text = good.Price.ToString();
        Icon.spriteName = GetGoodIconByID(good.id);
        Debug.Log(good.id.ToString());
    }
    string GetGoodIconByID(int id)
    {
        string spriteName = "";
        switch(id)
        {
            case 1:
                spriteName = "row";
                break;
            case 2:
                spriteName = "col";
                break;
            case 3:
                spriteName = "stick";
                break;
            case 4:
                spriteName = "clock";
                break;
        }
        return spriteName;
    }
    public void HideItemUI()
    {
        gameObject.SetActive(false);
    }
}
