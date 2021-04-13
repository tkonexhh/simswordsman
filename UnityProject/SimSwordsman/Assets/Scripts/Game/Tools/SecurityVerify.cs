using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class SecurityVerify : MonoBehaviour
	{
        /**
            * Verify the signature is correct
            **/
        public static bool IsCorrect()
        {
#if UNITY_EDITOR
            return true;
#endif

            // 获取Android的PackageManager    
            AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject Activity = Player.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject PackageManager = Activity.Call<AndroidJavaObject>("getPackageManager");

            // 获取当前Android应用的包名
            string packageName = Activity.Call<string>("getPackageName");

            // 调用PackageManager的getPackageInfo方法来获取签名信息数组    
            int GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
            AndroidJavaObject PackageInfo = PackageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
            AndroidJavaObject[] Signatures = PackageInfo.Get<AndroidJavaObject[]>("signatures");

            // 获取当前的签名的哈希值，判断其与我们签名的哈希值是否一致
            if (Signatures != null && Signatures.Length > 0)
            {
                int hashCode = Signatures[0].Call<int>("hashCode");
                Debug.Log("----------------hashCode is: " + hashCode);
                return hashCode == 527153405;//我们签名的哈希值

            }
            return false;
        }

     //   /**
     //* MD5加密
     //* @param byteStr 需要加密的内容
     //* @return 返回 byteStr的md5值
     //*/
     //   public static String encryptionMD5(byte[] byteStr)
     //   {
     //       MessageDigest messageDigest = null;
     //       StringBuffer md5StrBuff = new StringBuffer();
     //       try
     //       {
     //           messageDigest = MessageDigest.getInstance("MD5");
     //           messageDigest.reset();
     //           messageDigest.update(byteStr);
     //           byte[] byteArray = messageDigest.digest();
     //           //            return Base64.encodeToString(byteArray,Base64.NO_WRAP);
     //           for (int i = 0; i < byteArray.length; i++)
     //           {
     //               if (Integer.toHexString(0xFF & byteArray[i]).length() == 1)
     //               {
     //                   md5StrBuff.append("0").append(Integer.toHexString(0xFF & byteArray[i]));
     //               }
     //               else
     //               {
     //                   md5StrBuff.append(Integer.toHexString(0xFF & byteArray[i]));
     //               }
     //           }
     //       }
     //       catch (NoSuchAlgorithmException e)
     //       {
     //           e.printStackTrace();
     //       }
     //       return md5StrBuff.toString();
     //   }

     //   /**
     //    * 获取app签名md5值,与“keytool -list -keystore D:\Desktop\app_key”‘keytool -printcert     *file D:\Desktop\CERT.RSA’获取的md5值一样
     //    */
     //   public string getSignMd5Str(int sign)
     //   {
     //       try
     //       {
     //           string signStr = encryptionMD5(sign.toByteArray());
     //           return signStr;
     //       }
     //       catch (Exception e)
     //       {
     //           Debug.LogError(e.StackTrace);
     //       }
     //       return "";
     //   }
    }
	
}