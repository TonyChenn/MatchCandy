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
        //游戏玩法
        int GameMode = 0;
        GameUser gameUser = null;
        

        protected override void SetWndFlag()
        {
            this.curUIID = UIType.UI_Game;
        }

        public override void InitWndOnAwake()
        {
            base.InitWndOnAwake();
            anchorRightTrans.localPosition = new Vector3(150f, 0, 0);
            UIEventListener.Get(propArray[0].propTrans.gameObject).onClick = OnUseClearCol;
            UIEventListener.Get(propArray[1].propTrans.gameObject).onClick = OnUseClearRow;
            UIEventListener.Get(propArray[2].propTrans.gameObject).onClick = OnUseClock;
            UIEventListener.Get(propArray[3].propTrans.gameObject).onClick = OnUseHammer;
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
                UIMessageMgr.ShowDialog("提示", "你确定要退出游戏吗？", () => { Close(); }, true);
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
                gameState = GameState.GameOver;
            }
        }

        #region Button
        private void OnUseClearRow(GameObject go)
        {
            if (gameUser.clearRow.Get() < 1)
                UIMessageMgr.ToastMsg("无道具");
            else
            {
                //UIMessageMgr.ShowDialog("提示", "你确定要使用1个行消除道具吗？", () =>
                //{
                //    Messenger<PropsType>.Broadcast(MessengerEventDef.Str_UseProps, PropsType.ClearRow);
                //}, false);
            }
        }

        private void OnUseClearCol(GameObject go)
        {
            if (gameUser.clearCol.Get() < 1)
                UIMessageMgr.ToastMsg("无道具");
            else
            {
                //UIMessageMgr.ShowDialog("提示", "你确定要使用1个行消除道具吗？", () =>
                //{
                //    Messenger<PropsType>.Broadcast(MessengerEventDef.Str_UseProps, PropsType.ClearCol);
                //}, false);
            }
        }

        private void OnUseClock(GameObject go)
        {
            if (gameUser.clock.Get() < 1)
                UIMessageMgr.ToastMsg("无道具");
            else
            {
                //UIMessageMgr.ShowDialog("提示", "你确定要使用1个行消除道具吗？", () =>
                //{
                //    Messenger<PropsType>.Broadcast(MessengerEventDef.Str_UseProps, PropsType.Clock);
                //}, false);
            }
        }

        private void OnUseHammer(GameObject go)
        {
            if (gameUser.hammer.Get() < 1)
                UIMessageMgr.ToastMsg("无道具");
            else
            {
                //UIMessageMgr.ShowDialog("提示", "你确定要使用1个行消除道具吗？", () =>
                //{
                //    Messenger<PropsType>.Broadcast(MessengerEventDef.Str_UseProps, PropsType.Hammer);
                //}, false);
            }
        }

        #endregion

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
            gameUser = BmobUtil.Singlton.CurUser;
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


