using Bmob.util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_LevelItem : MonoBehaviour
    {
        [SerializeField]
        UISprite Icon;
        [SerializeField]
        UISprite Star1;
        [SerializeField]
        UISprite Star2;
        [SerializeField]
        UISprite Star3;
        [SerializeField]
        UILabel Level;
        [SerializeField]
        UISprite Lock;

        LevelDao levelDaoConfig;
        UserLevel userLevelConfig;
        bool canEnterLevel = false;
        private void Awake()
        {
            UIEventListener.Get(Icon.gameObject).onClick = (go) =>
            {
                if (canEnterLevel)
                {
                    UIManger.ShowUISync(UIType.UI_Game, levelDaoConfig);
                }
                else
                    UIMessageMgr.ToastMsg("关卡还未解锁");
            };
        }

        public void InitItem(LevelDao level)
        {
            int id = level.levelId;
            levelDaoConfig = level;
            char[] infoArray = PlayerPrefsUtil.UserLevelInfo.ToCharArray();
            Debug.Log(PlayerPrefsUtil.UserLevelInfo);
            if (id <= infoArray.Length)
            {
                char starCount = infoArray[id - 1];
                if (starCount == '3')
                {
                    Star1.gameObject.SetActive(true);
                    Star2.gameObject.SetActive(true);
                    Star3.gameObject.SetActive(true);
                }
                else if (starCount == '2')
                {
                    Star1.gameObject.SetActive(true);
                    Star2.gameObject.SetActive(true);
                    Star3.gameObject.SetActive(false);
                }
                else if (starCount == '1')
                {
                    Star1.gameObject.SetActive(true);
                    Star2.gameObject.SetActive(false);
                    Star3.gameObject.SetActive(false);
                }
                else if (starCount == '0')
                {
                    Star1.gameObject.SetActive(false);
                    Star2.gameObject.SetActive(false);
                    Star3.gameObject.SetActive(false);
                }
                Lock.gameObject.SetActive(false);
                canEnterLevel = true;
            }
            else
            {
                Star1.gameObject.SetActive(false);
                Star2.gameObject.SetActive(false);
                Star3.gameObject.SetActive(false);
                Lock.gameObject.SetActive(true);
                canEnterLevel = false;
            }
            Level.text = level.levelId.ToString();
        }

        public void HideUI()
        {
            gameObject.SetActive(false);
        }
        
    }
}

