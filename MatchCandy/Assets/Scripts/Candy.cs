using Common.Messenger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour {

    [SerializeField]
    public bool CanMove;
    [SerializeField]
    CandyType candyType;


    public int x;
    public int y;

    IEnumerator moveCoroutine;

    public int XRow
    {
        get { return x; }
        set { if (CanMove) x = value; }
    }
    public int YCol
    {
        get { return y; }
        set { if (CanMove) y = value; }
    }
    public CandyType CandyType
    {
        get { return candyType; }
    }

    private void Start()
    {
        UIEventListener.Get(gameObject).onDragEnd = (a) =>
        {
            Messenger.Broadcast(MessengerEventDef.Str_MousePressUp);
        };
        UIEventListener.Get(gameObject).onDragOver = (a) =>
        {
            Messenger<Candy>.Broadcast(MessengerEventDef.Str_MouseEnter, this);
        };
        UIEventListener.Get(gameObject).onDragStart = (a) =>
        {
            Messenger<Candy>.Broadcast(MessengerEventDef.Str_MousePressDown, this);
        };
    }

    public void Init(CandyType type,int x,int y)
    {
        this.candyType = type;
        this.XRow = x;
        this.YCol = y;
    }

    public void Move(int x, int y)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);

        moveCoroutine = MoveCoroutine(x, y, 0.2f);
        StartCoroutine(moveCoroutine);
    }
    IEnumerator MoveCoroutine(int x,int y,float timer)
    {
        this.x = x;
        this.y = y;

        Vector3 start = transform.localPosition;
        Vector3 target = GameMap.Singlton.GetPosition(x, y);

        for (float i = 0; i < timer; i+=Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(start, target, i / timer);
            yield return 0;
        }
        transform.localPosition = target;
    }
}
