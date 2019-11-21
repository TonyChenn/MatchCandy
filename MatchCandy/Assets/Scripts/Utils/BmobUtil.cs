using cn.bmob.api;
using cn.bmob.io;
using cn.bmob.tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bmob.util
{
    public class BmobUtil : MonoBehaviour {

        static BmobUtil _instance;
        static BmobUnity Bmob;
        public static BmobUtil Singlton
        {
            get { return _instance; }
        }
	    void Start () {
            DontDestroyOnLoad(this.gameObject);
            _instance = this;
            BmobDebug.Register(print);
            BmobDebug.level = BmobDebug.Level.TRACE;
            Bmob = GetComponent<BmobUnity>();
	    }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="mail">用于找回密码</param>
        public void Register(string userName,string password,string mail)
        {
            GameUser user = new GameUser();
            user.username = userName;
            user.password = password;
            user.email = mail;
            Bmob.Signup(user, (resp, exception) =>
            {
                if(exception!=null)
                    Debug.Log("注册失败,原因：" + exception.Message);
                else
                    Debug.Log("注册成功");
            });
        }

        /// <summary>
        /// 登录
        /// </summary>
        public void Login(string userName,string password)
        {
            Bmob.Login<GameUser>(userName, password, (resp, exception) =>
            {
                if (exception != null)
                    Debug.Log("登录失败,原因" + exception.Message);
                else
                    Debug.Log("登录成功");
            });
        }
        /// <summary>
        /// 获取当前用户
        /// </summary>
        public GameUser GetCurrentUser
        {
            get
            {
                if (GameUser.CurrentUser == null) return null;
                else
                    return GameUser.CurrentUser as GameUser;
            }
        }

        /// <summary>
        /// 查询所有商品
        /// </summary>
        public List<Shop> QueryShopAllItems()
        {
            List<Shop> list = null;
            BmobQuery query = new BmobQuery();
            query.WhereContainedIn("type","1","2");
            Bmob.Find<Shop>("Shop", query, (resp, exception) =>
            {
                if(exception!=null)
                    Debug.Log("商品查询失败,原因：" + exception.Message);
                else
                    list = resp.results;
            });
            return list;
        }

        /// <summary>
        /// 查询所有关卡信息
        /// </summary>
        public List<GameLevel> QueryAllGameLevel()
        {
            List<GameLevel> list = null;
            BmobQuery query = new BmobQuery();
            query.WhereGreaterThan("levelId", "0");
            Bmob.Find<GameLevel>("GameLevel", query, (resp, exception) =>
            {
                if (exception != null)
                    Debug.Log("获取所有关卡失败,原因：" + exception.Message);
                else
                    list = resp.results;
            });
            return list;
        }
    }

    #region 服务器端DAO
    /// <summary>
    /// 用户表
    /// </summary>
    public class GameUser:BmobUser
    {
        public BmobInt coin { get; set; }
        public BmobInt gem { get; set; }

        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
            output.Put("coin", this.coin);
            output.Put("gem", this.gem);
        }
        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            this.coin = input.getInt("coin");
            this.gem = input.getInt("gem");
        }
    }

    /// <summary>
    /// 关卡表
    /// </summary>
    public class GameLevel:BmobTable
    {
        /// <summary>
        /// 地图ID
        /// </summary>
        public BmobInt levelId { get; set; }
        /// <summary>
        /// 地图中的障碍物位置（x:y）
        /// </summary>
        public List<string> map { get; set; }
        /// <summary>
        /// 玩法类型(-1.限时达到固定分数，0-7对应Unity不同类型糖果)
        /// </summary>
        public string type { get; set; }
        public string mode { get; set; }

        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            this.levelId = input.getInt("levelId");
            this.map = input.getList<string>("map");
            this.type = input.getString("type");
            this.mode = input.getString("mode");
        }
    }

    /// <summary>
    /// 商城表
    /// </summary>
    public class Shop:BmobTable
    {
        public string name { get; set; }
        public BmobInt price { get; set; }
        /// <summary>
        /// 1.金币，2钻石
        /// </summary>
        public BmobInt type { get; set; }

        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            this.name = input.getString("name");
            this.type = input.getInt("type");
            this.price = input.getInt("price");
        }
    }
    #endregion
}
