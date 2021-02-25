using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
	public class AdsManager : TSingleton<AdsManager>
	{
		public void PlayRewardAD(string tag,Action<bool> LookADSuccessCallBack) 
		{
			CustomExtensions.PlayAd(tag, LookADSuccessCallBack);
		}
	}	
}