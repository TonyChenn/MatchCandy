using Common.Messenger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
	public class Currency : MonoBehaviour 
	{
		[SerializeField]
		UISprite Heart;
		[SerializeField]
		UILabel HeartCount;
		[SerializeField]
		UISprite Coin;
		[SerializeField]
		UILabel CoinCount;

        private void Awake()
        {
            UpdateUI();
            Messenger.AddListener(MessengerEventDef.Str_UpdateCurrency, UpdateUI);
        }
        private void OnDestroy()
        {
            Messenger.RemoveListener(MessengerEventDef.Str_UpdateCurrency, UpdateUI);
        }

        void UpdateUI()
        {
            HeartCount.text = "x"+PlayerPrefsUtil.HeartCount;
            CoinCount.text = "x" + PlayerPrefsUtil.CoinCount;
        }
    }	
}

