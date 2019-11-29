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
        void Start() {
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
        public void Register(string userName, string password, string mail)
        {
            GameUser user = new GameUser();
            user.username = userName;
            user.password = password;
            user.email = mail;
            Bmob.Signup(user, (resp, exception) =>
            {
                if (exception != null)
                    UIMessageMgr.ShowDialog("注册失败,原因:" + exception.Message);
                else
                    Debug.Log("注册成功");
            });
        }

        /// <summary>
        /// 登录
        /// </summary>
        public void Login(string userName, string password)
        {
            Bmob.Login<GameUser>(userName, password, (resp, exception) =>
            {
                if (exception != null)
                    UIMessageMgr.ShowDialog("登录失败,原因:" + exception.Message);
                else
                    Debug.Log("登录成功");
            });
        }

        /// <summary>
        /// 更新用户数据
        /// </summary>
        public void UpdateUserInfo(GameUser user)
        {
            Bmob.UpdateUser(user.objectId, user, user.sessionToken, (resp, ex) =>
            {
                if (ex != null)
                {
                    Debug.LogWarning("用户数据保存失败, 原因为：" + ex.Message);
                    return;
                }
                Debug.Log("用户数据更新成功");
            });
        }

        /// <summary>
        /// 查询所有商品
        /// </summary>
        public List<UserLevel> QueryUSerAllLevels(string userName)
        {
            int levelCount = QueryUserLevelCount(userName);
            Debug.Log(userName + ",LevelCount: " + levelCount);

            List<UserLevel> list = null;
            BmobQuery query = new BmobQuery();
            query.Limit(20);

            query.WhereEqualTo("userName", userName);
            query.OrderBy("levelId");
            Bmob.Find<UserLevel>("UserLevel", query, (resp, exception) =>
            {
                if (exception != null)
                    Debug.Log("商品查询失败,原因：" + exception.Message);
                else
                    list = resp.results;
            });
            return list;
        }

        public int QueryUserLevelCount(string userName)
        {
            int result = -1;
            BmobQuery query = new BmobQuery();
            query.WhereEqualTo("userName", userName);
            query.Count();
            Bmob.Find<UserLevel>("UserLevel", query, (resp, exp) =>
            {
                if (exp != null)
                {
                    Debug.LogWarning("查询UserLevel数量失败，原因：" + exp.Message);
                    return;
                }
                else
                {
                    result = resp.count.Get();
                }
            });
            return result;
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
    /// UI关卡
    /// </summary>
    public class UserLevel:BmobTable
    {
        public BmobInt levelId { get; set; }
        public BmobInt starCount { get; set; }
        public string userName { get; set; }

        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            this.levelId = input.getInt("levelId");
            this.starCount = input.getInt("starCount");
            this.userName = input.getString("userName");
        }
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
            output.Put("levelId", this.levelId);
            output.Put("starCount", this.starCount);
            output.Put("userName", this.userName);
        }
    }
    #endregion
}
