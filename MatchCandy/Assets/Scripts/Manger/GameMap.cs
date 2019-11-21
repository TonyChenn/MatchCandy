using Common.Messenger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour {

    [SerializeField]
    GameObject CandyBg;
    [SerializeField]
    GameObject CandyNull;
    [SerializeField]
    GameObject CandyWall;
    [SerializeField]
    GameObject ClearRow;
    [SerializeField]
    GameObject ClearCol;
    [SerializeField]
    GameObject ClearType;
    [SerializeField]
    GameObject[] NormalPrefabs;

    int row = 9;
    int colum = 12;
    Candy[,] candyArray;
    Candy pressedCandy;
    Candy upCandy;
    List<Candy> tempRowList = new List<Candy>();
    List<Candy> tempColList = new List<Candy>();
    List<Candy> matchedList = new List<Candy>();

    static GameMap gameMap = null;

    public static GameMap Singlton
    {
        get{ return gameMap; }
    }

    void Start() {
        gameMap = this;
        AddListener();
        initMap();
        ///虚拟数据
        var list = new List<string>();
        list.Add("2,3");
        list.Add("5,4");
        CreateWall(list);
        StartCoroutine(FillAllMap());
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    /// <summary>
    /// 初始化地图
    /// </summary>
    void initMap()
    {
        candyArray = new Candy[row, colum];
        //生成背景
        for (int iRow = 0; iRow < row; iRow++)
        {
            for (int jCol = 0; jCol < colum; jCol++)
            {
                var obj = NGUITools.AddChild(this.gameObject, CandyBg);
                obj.transform.localPosition = GetPosition(iRow, jCol);
                obj.transform.localScale = Vector3.one;
            }
        }
        //生成空白糖果
        for (int iRow = 0; iRow < row; iRow++)
        {
            for (int jCol = 0; jCol < colum; jCol++)
            {
                CreatCandy(iRow, jCol, CandyType.TYPE_NULL);
            }
        }
    }
    void CreateWall(List<string> wallList)
    {
        int x, y;
        for (int i = 0; i < wallList.Count; i++)
        {
            string[] temp = wallList[i].Split(',');
            x = int.Parse(temp[0]);
            y = int.Parse(temp[1]);
            Destroy(candyArray[x, y].gameObject);
            CreatCandy(x, y, CandyType.TYPE_WALL);
        }
    }

    /// <summary>
    /// 填充地图协程
    /// </summary>
    IEnumerator FillAllMap()
    {
        bool needFill = true;
        while(needFill)
        {
            yield return new WaitForSeconds(0.2f);
            while(fill())
            {
                yield return new WaitForSeconds(0.2f);
            }
            needFill = ClearAllMatchedCandies();
        }

    }
    bool fill()
    {
        bool notFinished = false;
        //填充地图
        for (int iRow = 1; iRow < row; iRow++)
        {
            for (int jCol = 0; jCol < colum; jCol++)
            {
                var candy = candyArray[iRow, jCol];
                if(candy.CanMove)
                {
                    //向下填充
                    var downCandy = candyArray[iRow - 1, jCol];
                    if(downCandy.CandyType==CandyType.TYPE_NULL)
                    {
                        Destroy(downCandy.gameObject);
                        candy.Move(iRow - 1, jCol);
                        candyArray[iRow - 1, jCol] = candy;
                        CreatCandy(iRow, jCol, CandyType.TYPE_NULL);
                        notFinished = true;
                    }
                    else   //下面不是空白糖果->是否需要斜向填充
                    {

                        for (int offsetCol = -1; offsetCol <= 1; offsetCol++)
                        {
                            if (offsetCol == 0) continue;

                            int colIndex = jCol + offsetCol;

                            if (colIndex >= 0 && colIndex < colum)
                            {
                                Candy right_leftDownCandy = candyArray[iRow - 1, colIndex];

                                if (right_leftDownCandy.CandyType == CandyType.TYPE_NULL)
                                {
                                    bool canfill = true;

                                    for (int aboveRow = iRow; aboveRow < row; aboveRow++)
                                    {
                                        Candy candyAbove = candyArray[aboveRow, colIndex];
                                        if (candyAbove.CanMove) break;
                                        else if (!candyAbove.CanMove && candyAbove.CandyType != CandyType.TYPE_NULL)
                                        {
                                            canfill = false;
                                            break;
                                        }
                                    }

                                    if (!canfill)
                                    {
                                        Destroy(right_leftDownCandy.gameObject);
                                        candy.Move(iRow-1, colIndex);
                                        candyArray[iRow-1, colIndex] = candy;
                                        CreatCandy(iRow, jCol, CandyType.TYPE_NULL);
                                        notFinished = true;
                                        break;
                                    }
                                }

                            }

                        }
                    }
                }
            }
        }
        for (int jCol = 0; jCol < colum; jCol++)
        {
            var candy = candyArray[row - 1, jCol];
            if(candy.CandyType==CandyType.TYPE_NULL)
            {
                int index = Random.Range(0, NormalPrefabs.Length);
                var obj = NGUITools.AddChild(gameObject, NormalPrefabs[index]);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = GetPosition(row, jCol);

                Destroy(candyArray[row - 1, jCol].gameObject);
                candyArray[row - 1, jCol] = obj.GetComponent<Candy>();
                candyArray[row - 1, jCol].Init(CandyType.TYPE_NORMOL, jCol, row);
                candyArray[row - 1, jCol].Move(row - 1, jCol);
                notFinished = true;
            }
        }

        return notFinished;
    }

    public Vector2 GetPosition(int xRow, int yCol)
    {
        float xx = (xRow - row / 2f) * 70;
        float yy = (colum / 2f - yCol - 1) * 70;
        return new Vector2(yy, xx);
    }

    /// <summary>
    /// 生成糖果
    /// </summary>
    public Candy CreatCandy(int x_row, int y_col, CandyType type)
    {
        GameObject obj = null;
        if (type == CandyType.TYPE_NULL)
            obj = NGUITools.AddChild(gameObject, CandyNull);
        else if (type == CandyType.TYPE_WALL)
            obj = NGUITools.AddChild(gameObject, CandyWall);
        else if (type == CandyType.TYPE_NORMOL)
        {
            int index = Random.Range(0, NormalPrefabs.Length);
            obj = NGUITools.AddChild(gameObject, NormalPrefabs[index]);
        }
        else if (type == CandyType.CLEAR_ROW)
            obj = NGUITools.AddChild(gameObject, ClearRow);
        else if (type == CandyType.CLEAR_COLUM)
            obj = NGUITools.AddChild(gameObject, ClearCol);
        else if (type == CandyType.CLEAR_TYPE)
            obj = NGUITools.AddChild(gameObject, ClearType);

        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = GetPosition(x_row, y_col);

        candyArray[x_row, y_col] = obj.GetComponent<Candy>();
        candyArray[x_row, y_col].Init(type, x_row, y_col);
        return candyArray[x_row, y_col];
    }
    /// <summary>
    /// 删除糖果
    /// </summary>
    public void DeleteCandy(Candy candy)
    {
        Destroy(candyArray[candy.XRow, candy.YCol].gameObject,0.5f);
        candyArray[candy.XRow, candy.YCol] = CreatCandy(candy.XRow, candy.YCol, CandyType.TYPE_NULL);
    }

    /// <summary>
    /// 清除所有匹配的糖果
    /// </summary>
    public bool ClearAllMatchedCandies()
    {
        bool needFillCandy = false;
        List<Candy> matchedList;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < colum; j++)
            {
                if(candyArray[i,j].CanMove && candyArray[i,j].CandyType!=CandyType.TYPE_NULL)
                {
                    CandyType clearType = CandyType.TYPE_NULL;
                    matchedList = MatchCandy(candyArray[i, j], i, j,ref clearType);
                    if(matchedList!=null)
                    {
                        int xx = -1;
                        int yy = -1;
                        CandyType type = CandyType.TYPE_NULL;
                        if(matchedList.Count>3)
                        {
                            var index = Random.Range(0, matchedList.Count);
                            xx = matchedList[index].XRow;
                            yy = matchedList[index].YCol;
                        }
                        if (matchedList.Count == 4) type = clearType;
                        if (matchedList.Count >= 5) type = CandyType.CLEAR_TYPE;

                        for (int item = 0; item < matchedList.Count; item++)
                        {
                            DeleteCandy(matchedList[item]);
                            needFillCandy = true;
                        }

                        if(type!=CandyType.TYPE_NULL)
                        {
                            CreatCandy(xx, yy, type);
                        }
                    }
                }
            }
        }

        return needFillCandy;
    }

    /// <summary>
    /// 三消匹配算法
    /// </summary>
    public List<Candy> MatchCandy(Candy candy, int newX, int newY,ref CandyType clearType)
    {
        tempRowList.Clear();
        tempColList.Clear();
        matchedList.Clear();

        var typeName = candy.gameObject.name;
        int x = newX;
        int y = newY;
        ///行匹配
        //向左
        tempRowList.Add(candy);
        for (int col_offset = y+1; col_offset < colum; col_offset++)
        {
            if (candyArray[x, col_offset].gameObject.name.Equals(typeName))
                tempRowList.Add(candyArray[x, col_offset]);
            else
                break;
        }
        //向右
        for (int col_offset = y-1; col_offset >= 0; col_offset--)
        {
            if (candyArray[x, col_offset].gameObject.name.Equals(typeName))
                tempRowList.Add(candyArray[x, col_offset]);
            else
                break;
        }
        if (tempRowList.Count >= 3)
        {
            foreach (var item in tempRowList)
                matchedList.Add(item);

            var rr = 0;
            //T型，L型检查
            foreach (var item in tempRowList)
            {
                //向上
                rr = item.XRow;
                for (int row_offset = rr + 1; row_offset < row; row_offset++)
                {
                    if (candyArray[row_offset, item.YCol].gameObject.name.Equals(typeName))
                        tempColList.Add(candyArray[row_offset, item.YCol]);
                    else
                        break;
                }
                //向下
                for (int row_offset = rr-1; row_offset >= 0; row_offset--)
                {
                    if (candyArray[row_offset, item.YCol].gameObject.name.Equals(typeName))
                        tempColList.Add(candyArray[row_offset, item.YCol]);
                    else
                        break;
                }
                if (tempColList.Count < 2) tempColList.Clear();
                else
                {
                    foreach (var temp in tempColList)
                        matchedList.Add(temp);

                    break;
                }
            }
        }
        if(matchedList.Count>=3)
        {
            clearType = CandyType.CLEAR_ROW;
            return matchedList;
        }

        tempRowList.Clear();
        tempColList.Clear();
        matchedList.Clear();

        ///列匹配
        //向上
        tempColList.Add(candy);
        for (int row_offset = x + 1; row_offset < row; row_offset++)
        {
            if (candyArray[row_offset, y].gameObject.name.Equals(typeName))
                tempColList.Add(candyArray[row_offset, y]);
            else
                break;
        }
        //向下
        for (int row_offset = x - 1; row_offset >= 0; row_offset--)
        {
            if (candyArray[row_offset, y].gameObject.name.Equals(typeName))
                tempColList.Add(candyArray[row_offset, y]);
            else
                break;
        }
        if (tempColList.Count >= 3)
        {
            foreach (var item in tempColList)
                matchedList.Add(item);

            //T型，L型检查
            int cc;
            foreach (var item in tempColList)
            {
                cc = item.YCol;
                //左
                for (int col_offset = cc + 1; col_offset < colum; col_offset++)
                {
                    if (candyArray[item.XRow, col_offset].gameObject.name.Equals(typeName))
                        tempRowList.Add(candyArray[item.XRow, col_offset]);
                    else
                        break;
                }
                //右
                for (int col_offset = cc - 1; col_offset >= 0; col_offset--)
                {
                    if (candyArray[item.XRow, col_offset].gameObject.name.Equals(typeName))
                        tempRowList.Add(candyArray[item.XRow, col_offset]);
                    else
                        break;
                }
                if (tempRowList.Count < 2) tempRowList.Clear();
                else
                {
                    foreach (var temp in tempRowList)
                        matchedList.Add(temp);

                    break;
                }
            }
        }
        if (matchedList.Count >= 3)
        {
            clearType = CandyType.CLEAR_COLUM;
            return matchedList;
        }
        return null;
    }

    /// <summary>
    /// 是否相邻
    /// </summary>
    bool IsAdjoin(Candy a, Candy b)
    {
        return (a.XRow == b.XRow && Mathf.Abs(a.YCol - b.YCol) == 1) || (a.YCol == b.YCol && Mathf.Abs(a.XRow - b.XRow) == 1);
    }
    void Exchange(Candy a, Candy b)
    {
        //完整两两交换逻辑
        if (a.CandyType == CandyType.TYPE_NULL || 
            a.CandyType == CandyType.TYPE_WALL || 
            b.CandyType == CandyType.TYPE_NULL || 
            b.CandyType == CandyType.TYPE_WALL)
        {
            return;
        }

        ///都是普通糖果
        if (a.CandyType == CandyType.TYPE_NORMOL && b.CandyType == CandyType.TYPE_NORMOL)
        {
            candyArray[a.XRow, a.YCol] = b;
            candyArray[b.XRow, b.YCol] = a;
            CandyType typeA = CandyType.TYPE_NULL;
            CandyType typeB = CandyType.TYPE_NULL;
            if (MatchCandy(a, b.XRow, b.YCol, ref typeA) != null || MatchCandy(b, a.XRow, a.YCol, ref typeB) != null)
            {
                ChangeHandler(a, b);
                ClearAllMatchedCandies();
                StartCoroutine(FillAllMap());
            }
            else
            {
                candyArray[a.XRow, a.YCol] = a;
                candyArray[b.XRow, b.YCol] = b;
            }
        }
        // 一个普通，一个行消除
        else if((a.CandyType == CandyType.CLEAR_ROW && b.CandyType == CandyType.TYPE_NORMOL) ||
                (a.CandyType == CandyType.TYPE_NORMOL && b.CandyType == CandyType.CLEAR_ROW))
        {
            ChangeHandler(a, b);
            ClearRowCandiesHandler(a.CandyType == CandyType.CLEAR_ROW ? a.XRow : b.XRow);
        }
        // 一个普通，一个列消除
        else if ((a.CandyType==CandyType.TYPE_NORMOL && b.CandyType==CandyType.CLEAR_COLUM) ||
                (a.CandyType == CandyType.CLEAR_COLUM && b.CandyType == CandyType.TYPE_NORMOL))
        {
            ChangeHandler(a, b);
            ClearColumCandiesHandler(a.CandyType == CandyType.CLEAR_COLUM ? a.YCol : b.YCol);
        }
        //一个普通，一个类型消除
        else if((a.CandyType == CandyType.TYPE_NORMOL && b.CandyType == CandyType.CLEAR_TYPE) ||
                (a.CandyType == CandyType.CLEAR_TYPE && b.CandyType == CandyType.TYPE_NORMOL))
        {
            ChangeHandler(a, b);
            if(a.CandyType==CandyType.CLEAR_TYPE)
            {
                ClearNormalTypeCandiesHandler(b.gameObject.name);
                DeleteCandy(a);
            }
            else
            {
                ClearNormalTypeCandiesHandler(a.gameObject.name);
                DeleteCandy(b);
            }
        }
        //两个 row消除
        else if(a.CandyType==CandyType.CLEAR_ROW && b.CandyType==CandyType.CLEAR_ROW)
        {
            ChangeHandler(a, b);
            ClearRowCandiesHandler(a.XRow);
            ClearRowCandiesHandler(b.XRow);
        }
        //两个clolum消除
        else if(a.CandyType==CandyType.CLEAR_COLUM && b.CandyType==CandyType.CLEAR_COLUM)
        {
            ChangeHandler(a, b);
            ClearColumCandiesHandler(a.YCol);
            ClearColumCandiesHandler(b.YCol);
        }
        //一个行消除 一个列消除
        else if((a.CandyType == CandyType.CLEAR_ROW && b.CandyType == CandyType.CLEAR_COLUM) ||
                (a.CandyType == CandyType.CLEAR_COLUM && b.CandyType == CandyType.CLEAR_ROW))
        {
            ChangeHandler(a, b);
            if(a.CandyType==CandyType.CLEAR_ROW)
            {
                ClearRowCandiesHandler(a.XRow);
                ClearColumCandiesHandler(b.YCol);
            }
            else
            {
                ClearRowCandiesHandler(b.XRow);
                ClearColumCandiesHandler(a.YCol);
            }
        }
        //一个row消除   一个类型消除
        else if((a.CandyType == CandyType.CLEAR_ROW && b.CandyType == CandyType.CLEAR_TYPE) ||
                (a.CandyType == CandyType.CLEAR_TYPE && b.CandyType == CandyType.CLEAR_ROW))
        {
            ChangeHandler(a, b);
            if(a.CandyType==CandyType.CLEAR_TYPE)
            {
                if (a.XRow - 1 >= 0)
                    ClearRowCandiesHandler(a.XRow - 1);
                ClearRowCandiesHandler(a.XRow);
                if (a.XRow + 1 < row)
                    ClearRowCandiesHandler(a.XRow + 1);
            }
            else
            {
                if (b.XRow - 1 >= 0)
                    ClearRowCandiesHandler(b.XRow - 1);
                ClearRowCandiesHandler(b.XRow);
                if (b.XRow + 1 < row)
                    ClearRowCandiesHandler(b.XRow + 1);
            }
        }
        //一个colum消除 一个类型消除
        else if((a.CandyType == CandyType.CLEAR_COLUM && b.CandyType == CandyType.CLEAR_TYPE) ||
                (a.CandyType == CandyType.CLEAR_COLUM && b.CandyType == CandyType.CLEAR_TYPE))
        {
            ChangeHandler(a, b);
            if (a.CandyType == CandyType.CLEAR_TYPE)
            {
                if (a.YCol - 1 >= 0)
                    ClearColumCandiesHandler(a.YCol - 1);
                ClearColumCandiesHandler(a.YCol);
                if (a.YCol + 1 < colum)
                    ClearColumCandiesHandler(a.YCol + 1);
            }
            else
            {
                if (b.YCol - 1 >= 0)
                    ClearColumCandiesHandler(b.YCol - 1);
                ClearColumCandiesHandler(b.YCol);
                if (b.YCol + 1 < colum)
                    ClearColumCandiesHandler(b.YCol + 1);
            }
        }
        //两个 类型消除
        else if(a.CandyType==CandyType.CLEAR_TYPE && b.CandyType==CandyType.CLEAR_TYPE)
        {
            ClearAllHandler();
        }
        StartCoroutine(FillAllMap());
    }

    void ChangeHandler(Candy a,Candy b)
    {
        candyArray[a.XRow, a.YCol] = b;
        candyArray[b.XRow, b.YCol] = a;

        int x = a.XRow;
        int y = a.YCol;

        a.Move(b.XRow, b.YCol);
        b.Move(x, y);
    }

    #region 特殊糖果删除操作
    void ClearRowCandiesHandler(int rowIndex)
    {
        for (int i = 0; i < colum; i++)
            DeleteCandy(candyArray[rowIndex, i]);
    }
    void ClearColumCandiesHandler(int columIndex)
    {
        for (int i = 0; i < row; i++)
            DeleteCandy(candyArray[i, columIndex]);
    }
    void ClearNormalTypeCandiesHandler(string candyName)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < colum; j++)
            {
                if (candyArray[i, j].gameObject.name == candyName)
                    DeleteCandy(candyArray[i, j]);
            }
        }
    }
    void ClearAllHandler()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < colum; j++)
            {
                DeleteCandy(candyArray[i, j]);
            }
        }
    }
    #endregion

    #region struct
    [System.Serializable]
    public struct CandyPrefab
    {
        public CandyType type;
        public GameObject prefab;
    }
    #endregion

    #region Messenger Listener && events
    void AddListener()
    {
        Messenger<Candy>.AddListener(MessengerEventDef.Str_MousePressDown, onPressCandy);
        Messenger<Candy>.AddListener(MessengerEventDef.Str_MouseEnter, onEnterCandy);
        Messenger.AddListener(MessengerEventDef.Str_MousePressUp, onReleaseCandy);
    }
    void RemoveListener()
    {
        Messenger<Candy>.RemoveListener(MessengerEventDef.Str_MousePressDown, onPressCandy);
        Messenger<Candy>.RemoveListener(MessengerEventDef.Str_MouseEnter, onEnterCandy);
        Messenger.RemoveListener(MessengerEventDef.Str_MousePressUp, onReleaseCandy);
    }

    void onPressCandy(Candy candy)
    {
        pressedCandy = candy;
    }
    void onEnterCandy(Candy candy)
    {
        upCandy = candy;
    }
    void onReleaseCandy()
    {
        if (pressedCandy != null && upCandy != null)
        {
            if (IsAdjoin(pressedCandy, upCandy))
                Exchange(pressedCandy, upCandy);

            pressedCandy = null;
            upCandy = null;
        }
    }
    #endregion
}
