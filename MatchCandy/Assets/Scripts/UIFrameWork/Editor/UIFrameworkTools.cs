using Modules.UI;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UIFrameworkTools{
    /// <summary>
    /// 规范相对路径：末尾不带"\",开头带
    /// </summary>
    public static string UIFrameworkRoot = "/Scripts/UIFrameWork";
    public static string UIPanelsFolderPath = "/Resources/Panel";
    public static string UIPanelsJsonPath = "/Resources/PanelJson";

    [MenuItem("UIFramework/Generate")]
	static void Generate()
    {
        FileUtils.CreatePanelsJsonFile(UIPanelsFolderPath, UIPanelsJsonPath);
    }
}
