using Common.Messenger;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_CountPanel : UIWndBase {
        [SerializeField]
        GameObject[] Numbers;

        [SerializeField]
        AudioClip clip;
        [SerializeField]
        AudioClip startClip;

        AudioClip curClip;

        protected override void SetWndFlag()
        {
            curUIID = UIType.UI_CountPanel;
        }
        public override void InitWndOnAwake()
        {
            base.InitWndOnAwake();
            for (int i = 0; i < Numbers.Length; i++)
            {
                Numbers[i].SetActive(false);
            }
        }

        public override void OnShowWnd(UIWndData wndData)
        {
            base.OnShowWnd(wndData);
            StartCoroutine(ShowCounter());
        }
        IEnumerator ShowCounter()
        {
            WaitForSeconds oneSeconds = new WaitForSeconds(1f);
            for (int i = Numbers.Length-1; i >=0; --i)
            {
                curClip = i == 0 ? startClip : clip;
                AudioManger.Singlton.PlayAudioClip(AudioType.Effect, curClip);
                Numbers[i].SetActive(true);
                yield return oneSeconds;
            }
            yield return oneSeconds;
            UI_Game.gameState = GameState.GamePlaying;
            Messenger.Broadcast(MessengerEventDef.Str_StartGame);
            Close();
        }
    }
}

