using Bmob.util;
using Common.Messenger;
using System;
using System.Collections;
using System.Collections.Generic;
using UIAnimation.Actions;
using UnityEngine;

namespace Modules.UI
{
    public class UI_Game : UIWndBase {

        [SerializeField]
        UILabel Count;
        [SerializeField]
        UILabel Score;
        [SerializeField]
        Transform anchorRightTrans;
        [SerializeField]
        ActionRunner showRunner;
        [SerializeField]
        Props[] propArray;

        LevelDao config = null;
        public static GameState gameState = GameState.GamePause;

        int gameScore = 0;
        int count = 0;
        GameUser gameUser = null;
        

        protected override void SetWndFlag()
        {
            this.curUIID = UIType.UI_Game;
        }

        public override void InitWndOnAwake()
        {
            base.InitWndOnAwake();
            anchorRightTrans.localPosition = new Vector3(150f, 0, 0);
            UIEventListener.Get(propArray[2].propTrans.gameObject).onClick = OnClockClick;
            gameUser = BmobUtil.Singlton.CurUser;
        }

        private void OnClockClick(GameObject go)
        {
            GameUser user = BmobUtil.Singlton.CurUser;
            if(user.clock.Get()>1)
            {
                count += 5;
                Messenger.Broadcast(MessengerEventDef.Str_UpdatePropsCount);
                BmobUtil.Singlton.UpdateUserInfo(user);
            }
            else
                UIMessageMgr.ToastMsg("道具不足");
            Messenger<PropsType, Candy>.Broadcast(MessengerEventDef.Str_UseProps, PropsType.Clock, null);
        }

        public override void OnShowWnd(UIWndData wndData)
        {
            base.OnShowWnd(wndData);
            if(wndData!=null)
            {
                if(wndData.ExData!=null)
                {
                    config = (LevelDao)wndData.ExData;
                    count = config.modeCount;
                    Count.text = count + "";
                }
            }
            Score.text = gameScore.ToString();
            UpdatePropsCount();

            showRunner.Stop();
            showRunner.Run();

            UIManger.ShowUISync(UIType.UI_CountPanel, null);
        }

        void StartGame()
        {
            gameState = GameState.GamePlaying;
        }

        float timer = 0;
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                gameState = GameState.GamePause;
                UIMessageMgr.ShowDialog(
                    "提示", 
                    "你确定要退出游戏吗？", 
                    () => 
                    {
                        Close();
                        if(UIManger.IsUIShowing(UIType.UI_CountPanel))
                        {
                            UIManger.HideUIWnd(UIType.UI_CountPanel);
                        };
                    },
                    null,
                    () => { gameState = GameState.GamePlaying; },
                    true);
            }

            if(gameScore<GameMap.Singlton.score)
            {
                gameScore = GameMap.Singlton.score;
                Score.text = gameScore + "";
            }
        }
        private void FixedUpdate()
        {
            if(gameState==GameState.GamePlaying)
            {
                timer += Time.fixedDeltaTime;
                if(timer>1)
                {
                    count -= 1;
                    Count.text = count.ToString();
                    timer = 0f;
                }
            }

            if(gameState==GameState.GamePlaying && count<=0)
            {
                int coinCount = gameUser.coin.Get();
                gameUser.coin = coinCount + gameScore / 100;
                BmobUtil.Singlton.UpdateUserInfo(gameUser);
                Messenger.Broadcast(MessengerEventDef.Str_UpdateCurrency);
                gameState = GameState.GameOver;
                int starCount = 0;
                if (gameScore >= config.star3)
                    starCount = 3;
                else if (gameScore >= config.star2)
                    starCount = 2;
                else if (gameScore >= config.star1)
                    starCount = 1;
                

                if(starCount>0)
                {
                    UIMessageMgr.ShowDialog("恭喜过关", string.Format("我的得分：[F94545FF]{0}[-]\n[FFFFFFFF]目标得分：[-]{1}", Score.text,config.star1), okActionHandler, null, okActionHandler, true);
                    char[] userLevelInfo = PlayerPrefsUtil.UserLevelInfo.ToCharArray();

                    userLevelInfo[config.levelId - 1] = char.Parse(starCount.ToString());
                    string str = new string(userLevelInfo);

                    Debug.Log(str + "\tAll\t" + PlayerPrefsUtil.UserLevelInfo);

                    if (str.Length == config.levelId)
                        str += "0";
                    PlayerPrefsUtil.UserLevelInfo = str;
                    //BmobUtil.Singlton.InsertLevel(gameUser.username, config.levelId, starCount);
                }
                else
                    UIMessageMgr.ShowDialog("过关失败", string.Format("我的得分：[F94545FF]{0}[-]\n[FFFFFFFF]目标得分：[-]{1}", Score.text, config.star1), okActionHandler, null, okActionHandler, true);
            }
        }
        void okActionHandler()
        {
            Close();
            UIManger.ShowUISync(UIType.UI_MapPanel, null);
        }

        #region Messenger
        public override void RegisterMessage()
        {
            base.RegisterMessage();
            Messenger.AddListener(MessengerEventDef.Str_UpdatePropsCount, UpdatePropsCount);
        }
        public override void RemoveMessage()
        {
            base.RemoveMessage();
            Messenger.RemoveListener(MessengerEventDef.Str_UpdatePropsCount, UpdatePropsCount);
        }

        void UpdatePropsCount()
        {
            propArray[0].countLabel.text = gameUser.clearCol.Get().ToString();
            propArray[1].countLabel.text = gameUser.clearRow.Get().ToString();
            propArray[2].countLabel.text = gameUser.clock.Get().ToString();
            propArray[3].countLabel.text = gameUser.hammer.Get().ToString();
        }
        #endregion
    }
    public enum PropsType
    {
        ClearRow,
        ClearCol,
        Clock,
        Hammer,
    }
    [System.Serializable]
    public struct Props
    {
        public Transform propTrans;
        public UILabel countLabel;
    }
}


