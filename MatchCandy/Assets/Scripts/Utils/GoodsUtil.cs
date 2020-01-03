using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.UI;
public class GoodsUtil
{
    static GoodsUtil _instance = null;
    List<GoodsDao> goodList;
    public static GoodsUtil Singlton
    {
        get
        {
            if (_instance == null)
                _instance = new GoodsUtil();
            return _instance;
        }
    }

    private GoodsUtil()
    {
        goodList = LitJson.JsonMapper.ToObject<List<GoodsDao>>(FileUtils.GameGoodsJson);
    }

    public List<GoodsDao> GoodsList
    {
        get
        {
            if (goodList == null)
                goodList = LitJson.JsonMapper.ToObject<List<GoodsDao>>(FileUtils.GameGoodsJson);
            return goodList;
        }
    }
}

/// <summary>
/// 对应商品信息
/// </summary>
public class GoodsDao
{
    public int id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
}
