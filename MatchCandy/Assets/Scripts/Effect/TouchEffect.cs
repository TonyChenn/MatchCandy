using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour {
    [SerializeField]
    UIRoot root;
    [SerializeField]
    Transform effectPrefab;

    static GameObject effectObj = null;

    void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            var v3 = Input.mousePosition;
            if (effectObj == null)
                effectObj = NGUITools.AddChild(root.gameObject, effectPrefab.gameObject);
            effectObj.transform.position = UICamera.currentCamera.ScreenToWorldPoint(v3);
            effectObj.SetActive(false);
            effectObj.SetActive(true);
        }
	}
}
