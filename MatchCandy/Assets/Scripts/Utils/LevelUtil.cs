using Modules.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUtil
{
    List<LevelDao> levelList = null;
    Dictionary<int, LevelDao> levelDict = null;
    static LevelUtil _instance = null;

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
        levelList = LitJson.JsonMapper.ToObject<List<LevelDao>>(FileUtils.GameLevelJson);
    }

    public List<LevelDao> LevelList
    {
        get
        {
            if(levelList==null)
                levelList = LitJson.JsonMapper.ToObject<List<LevelDao>>(FileUtils.GameLevelJson);
            return levelList;
        }
    }
    public Dictionary<int,LevelDao> LevelDict
    {
        get
        {
            if(levelDict==null)
            {
                levelDict = new Dictionary<int, LevelDao>();
                for (int i = 0; i < LevelList.Count; i++)
                    levelDict.Add(LevelList[i].levelId, levelList[i]);
            }
            return levelDict;
        }
    }
    public LevelDao GetConfigByLevelID(int levelID)
    {
        if (LevelDict.ContainsKey(levelID))
            return LevelDict[levelID];
        return null;
    }
}
/// <summary>
/// 对应地图信息
/// </summary>
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