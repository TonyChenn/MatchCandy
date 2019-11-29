using Common.Messenger;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_Toast : UIWndBase
    {
        [SerializeField]
        UISprite BG;
        [SerializeField]
        UILabel content;
        
        TweenScale tweenScale = null;

        protected override void SetWndFlag()
        {
            this.curUIID = UIType.UI_Toast;
        }

        protected override void Awake()
        {
            base.Awake();
            BG.gameObject.SetActive(false);
        }

        public override void RegisterMessage()
        {
            base.RegisterMessage();
            Messenger<string, float>.AddListener(MessengerEventDef.Str_ShowToast, ShowToast);
        }
        public override void RemoveMessage()
        {
            base.RemoveMessage();
            Messenger<string, float>.RemoveListener(MessengerEventDef.Str_ShowToast, ShowToast);
        }
        void ShowToast(string msg,float duration)
        {
            content.text = msg;

            if(tweenScale==null)
            {
                tweenScale = TweenScale.Begin(BG.gameObject, .2f, Vector3.one);
                tweenScale.cachedTransform.localScale = new Vector3(1f, 0.001f, 1f);
                tweenScale.SetStartToCurrentValue();
                tweenScale.SetOnFinished(()=> { StartCoroutine(ShowFinishHandler(duration)); });
            }
            else
            {
                tweenScale.ResetToBeginning();
            }
            if (!BG.gameObject.activeSelf)
                BG.gameObject.SetActive(true);
            tweenScale.PlayForward();
        }

        IEnumerator ShowFinishHandler(float duration)
        {
            yield return new WaitForSeconds(duration);
            BG.gameObject.SetActive(false);
        }
    }
}

