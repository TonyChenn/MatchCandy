using Common.Messenger;
using Modules.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Main : MonoBehaviour {

    UnityWebRequest webRequest;
    string versionJson = "";
    string levelJson = "";
    string goodsJson = "";

    int serverVersion = -1;
    string serverVersionUrl = "";
    string goodsVersionUrl = "";
    void Start ()
    {
        UIManger.ShowUISync(UIType.UI_Loading, null);
        UIManger.ShowUISync(UIType.UI_Dialog, null);
        UIManger.ShowUISync(UIType.StartPanel, null);
        UIManger.ShowUISync(UIType.UI_Toast, null);

        //检查更新信息
        StartCoroutine(DownLoadVersion("https://raw.githubusercontent.com/TonyChenn/TonyChenn.github.io/master/candy/version.txt"));
    }

    IEnumerator DownLoadVersion(string url)
    {
        webRequest = UnityWebRequest.Get(url);
        UIMessageMgr.ShowLoading(true, "正在检查版本信息");
        yield return webRequest.SendWebRequest();
        if(webRequest.isNetworkError || webRequest.isHttpError)
        {
            UIMessageMgr.ShowDialog("版本检查出错，原因：" + webRequest.error, false);
        }
        else
        {
            versionJson = webRequest.downloadHandler.text;

            VersionDao obj = VersionUtil.GetVersionObj(versionJson);
            serverVersion = obj.vesion;
            serverVersionUrl = obj.url;
            goodsVersionUrl = obj.goodsUrl;

            if (serverVersion == -1)
            {
                Debug.LogError("version->json信息为空");
                UIMessageMgr.ShowLoading(false);
            }
            else
            {
                if (serverVersion > PlayerPrefsUtil.LocalVersion)
                {
                    StartCoroutine(DownLoadLevel(serverVersionUrl));
                }
                else
                {
                    UIMessageMgr.ShowLoading(false);
                    Messenger.Broadcast(MessengerEventDef.Str_CheckLogin);
                }
            }
        }
    }

    //下载关卡Json信息
    IEnumerator DownLoadLevel(string url)
    {
        webRequest = UnityWebRequest.Get(url);
        UIMessageMgr.ShowLoading(true, "发现新版本，正在下载...");
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
            UIMessageMgr.ShowDialog("下载出错，原因：" + webRequest.error,false);
        else
        {
            levelJson = webRequest.downloadHandler.text;
            UIMessageMgr.ShowDialog(levelJson, false);
            //下载成功后版本信息保存本地
            PlayerPrefsUtil.LocalVersion = serverVersion;
            PlayerPrefsUtil.LocalVersionUrl = serverVersionUrl;

            FileUtils.GameLevelJson = levelJson;


            StartCoroutine(DownloadGoodsInfo(goodsVersionUrl));
        }
        Messenger.Broadcast(MessengerEventDef.Str_CheckLogin);
        UIMessageMgr.ShowLoading(false);
    }

    IEnumerator DownloadGoodsInfo(string url)
    {
        webRequest = UnityWebRequest.Get(url);
        UIMessageMgr.ShowLoading(true, "发现新版本，正在下载...");
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
            UIMessageMgr.ShowDialog("下载出错，原因：" + webRequest.error, false);
        else
        {
            goodsJson = webRequest.downloadHandler.text;
            UIMessageMgr.ShowDialog(goodsJson, false);
            //下载成功后版本信息保存本地
            PlayerPrefsUtil.LocalVersion = serverVersion;
            PlayerPrefsUtil.LocalVersionUrl = serverVersionUrl;
            PlayerPrefsUtil.LocalGoodsInfoUrl = goodsVersionUrl;

            FileUtils.GameGoodsJson = goodsJson;
        }
        Messenger.Broadcast(MessengerEventDef.Str_CheckLogin);
        UIMessageMgr.ShowLoading(false);
    }
}
