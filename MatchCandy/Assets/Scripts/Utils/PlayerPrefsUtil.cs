using Common.Messenger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsUtil
{
    /// <summary>
    /// 本地资源版本
    /// </summary>
    public static int LocalVersion
    {
        get
        {
            int version = 0;
            if (PlayerPrefs.HasKey("version"))
                version = PlayerPrefs.GetInt("version");
            return version;
        }
        set
        {
            PlayerPrefs.SetInt("version", value);
        }
    }

    /// <summary>
    /// 版本地址
    /// </summary>
    public static string LocalVersionUrl
    {
        get
        {
            string url = "";
            if (PlayerPrefs.HasKey("versionUrl"))
                url = PlayerPrefs.GetString("versionUrl");
            return url;
        }
        set
        {
            PlayerPrefs.SetString("versionUrl", value);
        }
    }

    public static string LocalGoodsInfoUrl
    {
        get
        {
            string url = "";
            if (PlayerPrefs.HasKey("goodsInfoUrl"))
                url = PlayerPrefs.GetString("goodsInfoUrl");
            return url;
        }
        set
        {
            PlayerPrefs.SetString("goodsInfoUrl", value);
        }
    }

    /// <summary>
    /// 是否是第一次登录
    /// </summary>
    public static bool FirstLogin
    {
        get
        {
            string firstTime = "true";
            if (PlayerPrefs.HasKey("firstTimeLogin"))
                firstTime = PlayerPrefs.GetString("firstTimeLogin");
            if (firstTime.Equals("false"))
                return false;
            else
                return true;
        }
        set
        {
            string result = value ? "true" : "false";
            PlayerPrefs.SetString("firstTimeLogin", result);
        }
    }

    /// <summary>
    /// 金币
    /// </summary>
    public static int CoinCount
    {
        get
        {
            int coinCount=0;
            if(PlayerPrefs.HasKey("CoinCount"))
            {
                coinCount=PlayerPrefs.GetInt("CoinCount");
            }
            return coinCount;
        }
        set
        {
            PlayerPrefs.SetInt("CoinCount",value);
            Messenger.Broadcast(MessengerEventDef.Str_UpdateCurrency);
        }
    }

    /// <summary>
    /// 爱心
    /// </summary>
    public static int HeartCount
    {
        get
        {
            int coinCount=0;
            if(PlayerPrefs.HasKey("HeartCount"))
            {
                coinCount=PlayerPrefs.GetInt("HeartCount");
            }
            return coinCount;
        }
        set
        {
            PlayerPrefs.SetInt("HeartCount", value);
            Messenger.Broadcast(MessengerEventDef.Str_UpdateCurrency);
        }
    }

}
