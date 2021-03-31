using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class AdsManager : TSingleton<AdsManager>
	{
		#region »»‘∆œ‡πÿ
		private const string m_LookADKey = "m_LookADKey";
		private const string m_KeyBehavior = "w_key_behavior{0}";
		private int GetLookADCount()
		{
			return PlayerPrefs.GetInt(m_LookADKey, 0);
		}
		private void UpdateLookADCount(bool isClick)
		{
			int lookADCount = PlayerPrefs.GetInt(m_LookADKey, 0);
			lookADCount++;
			PlayerPrefs.SetInt(m_LookADKey, lookADCount);

			if (lookADCount <= 24) 
			{
				if (lookADCount % 3 == 0) 
				{
					int keyCount = lookADCount / 3 + 1;

					DataAnalysisMgr.S.CustomEvent(string.Format(m_KeyBehavior, keyCount));
				}			
			}
		}
		#endregion

		public void PlayRewardAD(string tag,Action<bool> LookADSuccessCallBack) 
		{
			LookADSuccessCallBack += UpdateLookADCount;

			CustomExtensions.PlayAd(tag, LookADSuccessCallBack);
		}

        public void PlayInterAD(string tag, Action<bool> LookInterADCallBackMethod)
        {
			LookInterADCallBackMethod += UpdateLookADCount;

            CustomExtensions.PlayAd(tag, LookInterADCallBackMethod,null, "Inter0", "MainInter");
        }
    }
}