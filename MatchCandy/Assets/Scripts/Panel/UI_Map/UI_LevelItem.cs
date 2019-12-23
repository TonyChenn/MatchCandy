using Bmob.util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                    LevelUtil.Singlton.CurLevel = levelDaoConfig;
                    SceneManager.LoadScene("Main");
                }
                else
                    UIMessageMgr.ToastMsg("关卡还未解锁");
            };
        }

        public void InitItem(LevelDao level)
        {
            int id = level.levelId;
            levelDaoConfig = level;
            userLevelConfig = BmobUtil.Singlton.GetUserLevelConfigById(id);
            if(userLevelConfig != null)
            {
                int starCount = userLevelConfig.starCount.Get();
                if(starCount==3)
                {
                    Star1.gameObject.SetActive(true);
                    Star2.gameObject.SetActive(true);
                    Star3.gameObject.SetActive(true);
                }
                else if(starCount==2)
                {
                    Star1.gameObject.SetActive(true);
                    Star2.gameObject.SetActive(true);
                    Star3.gameObject.SetActive(false);
                }
                else if(starCount==1)
                {
                    Star1.gameObject.SetActive(true);
                    Star2.gameObject.SetActive(false);
                    Star3.gameObject.SetActive(false);
                }
                else if(starCount==0)
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
                //如果是第一关 || 上一关卡有数据
                if(id==1)
                {
                    Lock.gameObject.SetActive(false);
                    canEnterLevel = true;
                    Star1.gameObject.SetActive(false);
                    Star2.gameObject.SetActive(false);
                    Star3.gameObject.SetActive(false);
                }
                else if (BmobUtil.Singlton.GetUserLevelConfigById(id-1)!=null)
                {
                    Lock.gameObject.SetActive(false);
                    canEnterLevel = true;
                    Star1.gameObject.SetActive(false);
                    Star2.gameObject.SetActive(false);
                    Star3.gameObject.SetActive(false);
                }
                else
                {
                    Lock.gameObject.SetActive(true);
                    canEnterLevel = false;
                }
            }
            Level.text = level.levelId.ToString();
        }

        public void HideUI()
        {
            gameObject.SetActive(false);
        }
        
    }
}

