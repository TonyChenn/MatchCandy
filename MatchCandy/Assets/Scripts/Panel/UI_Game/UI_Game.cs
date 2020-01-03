using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_Game : UIWndBase {

        [SerializeField]
        UILabel Count;
        [SerializeField]
        UILabel Score;

        LevelDao config = null;
        GameState gameState = GameState.GamePause;

        int gameScore = 0;
        int count = 0;
        //游戏玩法
        int GameMode = 0;
        

        protected override void SetWndFlag()
        {
            this.curUIID = UIType.UI_Game;
        }

        public override void InitWndOnAwake()
        {
            base.InitWndOnAwake();
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
                    GameMode = config.mode;
                    Count.text = count + "";
                }
            }
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
            if(gameState==GameState.GamePlaying && GameMode==0)
            {
                timer += Time.fixedDeltaTime;
                if(timer>1)
                {
                    count -= 1;
                    Count.text = count + "";
                }
            }

            if(count<=0)
            {
                gameState = GameState.GameOver;
            }
        }


        #region Messenger
        public override void RegisterMessage()
        {
            base.RegisterMessage();
        }
        public override void RemoveMessage()
        {
            base.RemoveMessage();
        }
        #endregion
    }
}


