using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPrefsTool{
    [MenuItem("UIFramework/PlayerPrefs/ClearUserLevel")]
    public static void ClearUserLevel()
    {
        PlayerPrefsUtil.UserLevelInfo = "0";
    }
}
