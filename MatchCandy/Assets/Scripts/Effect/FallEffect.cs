using UnityEngine;

public class FallEffect : MonoBehaviour {

    [SerializeField]
    GameObject UIRoot;
    [SerializeField]
    GameObject[] fallPrefabs;

    float width;
    float height;
    GameObject go;
	void Start () {
        width = Screen.width;
        height = Screen.height;

        go = NGUITools.AddChild(UIRoot, 9);
        go.name = "FallParent";
        var panel = go.AddComponent<UIPanel>();
        panel.depth = 48;
        go.transform.localPosition = new Vector3(width / 2, height / 2, 0);
        go.transform.localScale = Vector3.one;

        if(!IsInvoking("CreateFallObject"))
        {
            InvokeRepeating("CreateFallObject", 0, 1);
        }
    }

    void CreateFallObject()
    {
        int index = Random.Range(0, 2);
        var obj = NGUITools.AddChild(go, fallPrefabs[index], go.layer);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 40));
    }
    private void OnDestroy()
    {
        if (IsInvoking("CreateFallObject"))
            CancelInvoke("CreateFallObject");
    }
}
