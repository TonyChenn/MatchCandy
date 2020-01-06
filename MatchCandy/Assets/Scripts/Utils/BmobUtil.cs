using cn.bmob.api;
using cn.bmob.io;
using cn.bmob.tools;
using Common.Messenger;
using Modules.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bmob.util
{
    public class BmobUtil : MonoBehaviour
    {
        static BmobUtil _instance;
        static BmobUnity Bmob;
        Dictionary<int, UserLevel> userLevelDict = new Dictionary<int, UserLevel>();
        public List<UserLevel> m_Config = new List<UserLevel>();
        public Dictionary<int, UserLevel> UserLevelDict
        {
            get
            {
                if (Singlton.CurUser == null)
                {
                    UIMessageMgr.ShowDialog("提示", "登录信息获取失败，请重新登录", () =>
                    {
                        Application.Quit();
                    },false);
                    return null;
                }
                else
                {
                    userLevelDict.Clear();
                    foreach (var item in m_Config)
                        userLevelDict.Add(item.levelId.Get(), item);

                    return userLevelDict;
                }
            }
        }

        public static BmobUtil Singlton
        {
            get { return _instance; }
        }
        void Start()
        {
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
            user.coin = 0;
            user.gem = 0;
            user.email = mail;
            Bmob.Signup(user, (resp, exception) =>
            {
                if (exception != null)
                    UIMessageMgr.ShowDialog("注册失败，原因：" + exception.Message, false);
                else
                {
                    UIMessageMgr.ToastMsg("注册成功，欢迎" + userName);
                    Messenger.Broadcast(MessengerEventDef.Str_RegisterSuccess);
                }
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
                    UIMessageMgr.ShowDialog("登录失败,原因:" + exception.Message,false);
                else
                {
                    UIMessageMgr.ToastMsg("登录成功，欢迎回来");
                    Messenger.Broadcast(MessengerEventDef.Str_LoginSuccess);
                    PlayerPrefsUtil.CoinCount = CurUser.coin.Get();
                    StartCoroutine(Singlton.QueryLevels(userName));
                }
            });
        }

        public GameUser CurUser
        {
            get { return BmobUser.CurrentUser as GameUser; }
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        public void Logout()
        {
            BmobUser.LogOut();
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
                Messenger.Broadcast(MessengerEventDef.Str_UpdateCurrency);
                Debug.Log("用户数据更新成功");
            });
        }

        /// <summary>
        /// 查询所有关卡信息
        /// </summary>
        IEnumerator QueryLevels(string userName)
        {
            bool isLoad = false;
            UIMessageMgr.ShowLoading(true, "正在加载关卡信息");
            //根据数量查询所有
            Debug.Log("正在查询" + userName);
            BmobQuery _query = new BmobQuery();
            _query.WhereEqualTo("userName", userName);
            _query.OrderBy("levelId");
            Bmob.Find<UserLevel>("UserLevel", _query, (res, exception) =>
            {
                isLoad = true;
                UIMessageMgr.ShowLoading(false);
                if (exception != null)
                    Debug.Log("关卡查询失败,原因：" + exception.Message);
                else
                    m_Config = res.results;
            });
            while(!isLoad)
            {
                yield return new WaitForSeconds(1);
            }
        }

        public void InsertLevel(string userName, int levelID, int starCount)
        {
            UserLevel level = new UserLevel();
            level.userName = userName;
            level.levelId = levelID;
            level.starCount = starCount;
            Bmob.Create("UserLevel", level, (resp, ex) =>
            {
                if (ex != null)
                    UIMessageMgr.ToastMsg("关卡" + level.levelId + "已开启");
                else
                {
                    UIMessageMgr.ToastMsg("关卡加载失败");
                    Debug.LogError("注册开启第一关失败，原因：" + ex.Message);
                }
            });
        }

        public void MotifyLevel(string userName, int LevelId, int starCount)
        {

        }

        public UserLevel GetUserLevelConfigById(int levelId)
        {
            if (UserLevelDict.ContainsKey(levelId))
                return UserLevelDict[levelId];
            return null;
        }
    }

    #region 服务器端DAO
    /// <summary>
    /// 用户表
    /// </summary>
    public class GameUser : BmobUser
    {
        public BmobInt coin { get; set; }
        public BmobInt gem { get; set; }

        public BmobInt clearRow { get; set; }
        public BmobInt clearCol { get; set; }
        public BmobInt clock { get; set; }
        public BmobInt hammer { get; set; }

        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
            output.Put("coin", this.coin);
            output.Put("gem", this.gem);
            output.Put("clearRow", this.clearRow);
            output.Put("clearCol", this.clearCol);
            output.Put("clock", this.clock);
            output.Put("hammer", this.hammer);
        }
        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            this.coin = input.getInt("coin");
            this.gem = input.getInt("gem");
            this.clearRow = input.getInt("clearRow");
            this.clearCol = input.getInt("clearCol");
            this.clock = input.getInt("clock");
            this.hammer = input.getInt("hammer");
        }
    }


    /// <summary>
    /// UI关卡
    /// </summary>
    public class UserLevel : BmobTable
    {
        public BmobInt levelId { get; set; }
        public BmobInt starCount { get; set; }
        public string userName { get; set; }

        /// <summary>
        /// 仅供本地使用
        /// </summary>
        private int haveStar = 0;
        public int HaveStar
        {
            get { return haveStar; }
            set { haveStar = value; }
        }

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
