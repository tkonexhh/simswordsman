using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public enum WeChatTex { 
        PrefectCharacter,
        Challenge30Level,
    }
	public class WeChatShareMgr : TSingleton<WeChatShareMgr>
	{
		public void Init()
        {
            EventSystem.S.Register(EventID.OnLoginWeChatSuccess, OnLoginWeChatSuccessCallBack);

#if UNITY_ANDROID && !UNITY_EDITOR
            WeShareMgr.OnLoginSuccess += OnLoginSuccessCallBack;
            WeShareMgr.OnLoginCancle += OnLoginCancleCallBack;
            WeShareMgr.OnLoginFailed += OnLoginFailedCallBack;

            WeShareMgr.S.Init();    
            //Login();
#endif
        }
        private void OnLoginWeChatSuccessCallBack(int key, object[] param)
        {
            PlayerPrefs.SetInt(Define.WeChatKey, 1);
        }
        public void Login() 
        {
            Debug.LogError("is login we chat: " + IsLoginWeChat());
#if UNITY_ANDROID && !UNITY_EDITOR
            if (IsLoginWeChat() == false)
            {
                WeShareMgr.S.Login();
            }
#endif
        }
        public bool IsLoginWeChat()
        {
            return PlayerPrefs.GetInt(Define.WeChatKey, -1) != -1;
        }
        private void OnLoginFailedCallBack(string obj)
        {
            Debug.LogError("on login faild");
        }
        private void OnLoginCancleCallBack(string obj)
        {
            Debug.LogError("on login cancle");
        }
        private void OnLoginSuccessCallBack(string obj)
        {
            try 
            {
                ThreadMgr.S.mainThread.PostAction(() =>
                {
                    EventSystem.S.Send(EventID.OnLoginWeChatSuccess);

                    Debug.LogError("on we chat login success");

                    PlayerPrefs.SetInt(Define.WeChatKey, 1);
                });
            } catch (Exception ex) 
            {
                Debug.LogError("we chat login success error:" + ex.ToString());
            }            
        }
        public bool IsInstallWeChat()
        {
            try
            {
                AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
                AndroidJavaObject appList = packageManager.Call<AndroidJavaObject>("getInstalledPackages", 0);
                int num = appList.Call<int>("size");
                for (int i = 0; i < num; i++)
                {
                    AndroidJavaObject appInfo = appList.Call<AndroidJavaObject>("get", i);
                    string packageNew = appInfo.Get<string>("packageName");
                    if (packageNew.CompareTo("com.tencent.mm") == 0)
                    {

                        return true;
                    }
                }
            }
            catch (Exception ex) {
                Debug.LogError("is install we chat error:" + ex.ToString());
            }

            return false;
        }

        public void Share(WeChatTex weChatTex) 
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (IsInstallWeChat())
            {
                try
                {
                    //if (WeChatShareMgr.S.IsLoginWeChat())
                    {
                        Debug.LogError("share texture");

                        var url = "http://fission.ewxmax.com/fission/zuiqiangmenpai.html";

                        var shareText = string.Format("{0}", url);
                        int index = weChatTex == WeChatTex.PrefectCharacter ? 2 : 1;
                        var pic = Resources.Load(string.Format("ShareTex/Img{0}", index)) as Texture2D;

                        var code = pic != null ?
                            QRCodeHelper.GenNestedQRCodeTexture(pic, shareText, QRCodeHelper.QRCodeNestPosEnum.RightBottom, 20, 20) :
                            QRCodeHelper.GenQRCodeTexture(shareText);

                        WeShareUtils.SharePicByWXSession(code, true);

                        //DataAnalysisMgr.S.CustomEvent(Define.ShareBtnClick, "1");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Œ¢–≈∑÷œÌ ß∞‹:" + ex.ToString());
                    FloatMessage.S.ShowMsg("∑÷œÌ¥ÌŒÛ£¨«ÎºÏ≤ÈŒ¢–≈ «∑Òµ«¬º£°");
                }
            }
            else
            {
                FloatMessage.S.ShowMsg("Œ¥∞≤◊∞Œ¢–≈£¨∑÷œÌ ß∞‹£°");
            }
#endif
        }
    }
}