using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_LevelItem : MonoBehaviour
    {

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

        public void InitItem(LevelDao level)
        {
            Level.text = level.levelId.ToString();
        }
    }
}

