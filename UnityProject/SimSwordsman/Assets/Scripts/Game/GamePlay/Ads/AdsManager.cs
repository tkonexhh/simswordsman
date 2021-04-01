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
			int lookADCount = GetLookADCount();
			lookADCount++;
			PlayerPrefs.SetInt(m_LookADKey, lookADCount);

			if (lookADCount == 3)
			{
				DataAnalysisMgr.S.CustomEvent(string.Format(m_KeyBehavior, 2));
			}
		}
		#endregion

		public void Init() 
		{
			EventSystem.S.Register(EventID.OnUpgradeFacility, OnUpgradeFacilityCallBack);
		}

        private void OnUpgradeFacilityCallBack(int key, object[] param)
        {
			int LobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);

			if (LobbyLevel == 2) 
			{
				FacilityType facilityType = EnumUtil.ConvertStringToEnum<FacilityType>(param[0].ToString());

				if (facilityType == FacilityType.Lobby)
				{
					DataAnalysisMgr.S.CustomEvent(string.Format(m_KeyBehavior, 3));
				}
			}			
		}

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