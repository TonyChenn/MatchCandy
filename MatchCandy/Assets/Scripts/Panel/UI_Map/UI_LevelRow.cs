using Bmob.util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_LevelRow : MonoBehaviour
    {
        [SerializeField]
        UI_LevelItem[] itemArray;

        public void InitData(int min,int max,List<LevelDao> list)
        {
            int count = list.Count;
            for (int i = 0; i < itemArray.Length; ++i)
            {
                if (min + i >= count)
                    itemArray[i].HideUI();
                else
                    itemArray[i].InitItem(list[min + i]);
            }
        }
        public void HideAllItem()
        {
            for (int i = 0; i < itemArray.Length; i++)
            {
                itemArray[i].HideUI();
            }
        }
    }
}

