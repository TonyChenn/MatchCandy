using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modules.UI;
using Common.Messenger;

public class PropsItem : UIDragDropItem
{
    [SerializeField]
    PropsType type;

    Transform targetTrans;
    Vector3 startPos;

    protected override void Awake()
    {
        base.Awake();
        startPos = transform.localPosition;
    }
    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        targetTrans = transform.parent;
    }
    protected override void OnPress(bool isPressed)
    {
        base.OnPress(isPressed);
        if(isPressed)
        {
            transform.GetComponent<UIWidget>().depth += 2;
            transform.localScale = new Vector3(.8f, .8f, .8f);
        }
        else
        {
            transform.GetComponent<UIWidget>().depth -= 2;
            transform.localScale = Vector3.one;
        }
    }

    IEnumerator moveCoroutine = null;
    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        if(surface.tag.Equals("candy"))
        {
            Candy candy = surface.GetComponent<Candy>();
            Messenger<PropsType, Candy>.Broadcast(MessengerEventDef.Str_UseProps, type, candy);
        }
    }
}
