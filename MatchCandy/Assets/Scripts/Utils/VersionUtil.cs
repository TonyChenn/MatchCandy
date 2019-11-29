using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionUtil
{

    public static int GetServerVersion(string json)
    {
        int version = -1;
        if(!string.IsNullOrEmpty(json))
        {
            VersionDao vv = LitJson.JsonMapper.ToObject<VersionDao>(json);
            version = vv.vesion;
        }
        return version;
    }
    public static string GetNewVersionUrl(string json)
    {
        string url = "";
        if(!string.IsNullOrEmpty(json))
        {
            VersionDao vv = LitJson.JsonMapper.ToObject<VersionDao>(json);
            url = vv.url;
        }
        return url;
    }
}

public class VersionDao
{
    public int vesion { get; set; }
    public string url { get; set; }
}

