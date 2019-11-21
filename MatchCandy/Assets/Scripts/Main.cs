using Modules.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
	void Start ()
    {
        UIManger.ShowUISync(UIType.UI_Loading, null);
        UIManger.ShowUISync(UIType.UI_Dialog, null);
        UIManger.ShowUISync(UIType.StartPanel, null);
	}
}
