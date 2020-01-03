using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.UI
{
    public class UI_Goods_Row : MonoBehaviour
    {
        [SerializeField]
        UI_Goods_item[] itemArray;

        public void ShowUI(int min,int max,List<GoodsDao> list)
        {
            int count = list.Count;
            for (int i = 0; i < itemArray.Length; ++i)
            {
                if (min + i >= count)
                    itemArray[i].HideItemUI();
                else
                    itemArray[i].ShowItemUI(list[min + i]);
            }
        }

        public void HideAllUI()
        {
            for (int i = 0; i < itemArray.Length; i++)
            {
                itemArray[i].HideItemUI();
            }
        }
    }
}

