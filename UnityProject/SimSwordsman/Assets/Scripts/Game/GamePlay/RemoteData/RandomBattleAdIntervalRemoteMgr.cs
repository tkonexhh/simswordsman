using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class RandomBattleAdIntervalRemoteMgr : TSingleton<RandomBattleAdIntervalRemoteMgr>
    {
        private Dictionary<string, string> m_Headers = new Dictionary<string, string>();

        private int m_InterADInterval = 3;
        public int InterADInterval
        {
            get {
                return m_InterADInterval;
            }
        }

        public void Init(string channel = "all")
        {
            if (!m_Headers.ContainsKey("Content-Encoding"))
                m_Headers.Add("Content-Encoding", "gzip");
            else
                m_Headers["Content-Encoding"] = "gzip";

            string url = SDKConfig.S.remoteConfUrl;
            string appName = SDKConfig.S.remoteConfAppName;

            if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(appName))
            {
                CustomExtensions.FetchRemoteConfParams(
                    appName,
                    "RandomBattleAdInterval",
                    OnRemoteValueFetched,
                    null,
                    channel,
                    url,
                    m_Headers);
            }
        }

        void OnRemoteValueFetched(string value)
        {
            try
            {
                m_InterADInterval = int.Parse(value);
            }
            catch (Exception ex) {
                m_InterADInterval = 3;
            }
        }
    }
}