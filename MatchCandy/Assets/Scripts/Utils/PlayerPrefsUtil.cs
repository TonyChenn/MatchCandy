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
}
