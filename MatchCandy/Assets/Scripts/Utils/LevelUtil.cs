using Modules.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUtil
{
    private List<LevelDao> levelList = new List<LevelDao>();
    private static LevelUtil _instance = null;
    public static LevelUtil Singlton
    {
        get
        {
            if (_instance == null)
                _instance = new LevelUtil();
            return _instance;
        }
    }


    private LevelUtil()
    {
        levelList.Clear();
        levelList = LitJson.JsonMapper.ToObject<List<LevelDao>>(FileUtils.GameLevelJson);
        foreach (LevelDao item in levelList)
        {
            Debug.Log(item.levelId + "======");
        }
    }

    public List<LevelDao> LevelList
    {
        get { return levelList; }
    }
}
public class LevelDao
{
    public int levelId { get; set; }
    public int mode { get; set; }
    public int modeCount { get; set; }
    public int levelType { get; set; }
    public int star1 { get; set; }
    public int star2 { get; set; }
    public int star3 { get; set; }
}