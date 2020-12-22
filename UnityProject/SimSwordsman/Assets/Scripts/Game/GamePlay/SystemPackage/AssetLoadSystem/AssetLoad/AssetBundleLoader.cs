using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class AssetBundleLoader : IAssetLoadAdapter
    {
        private ResLoader m_ResLoader = null;

        private string m_Name;
        private Action<GameObject> m_Callback = null;

        public AssetBundleLoader(string name)
        {
            m_Name = name;
            m_ResLoader = ResLoader.Allocate(name);
        }

        #region IAssetLoadAdapter
        public void InstantiateAsync(string name, Action<GameObject> callback)
        {
            m_Callback = callback;

            LoadAssetAsync(name, (go) => 
            {
                GameObject result = GameObject.Instantiate(go);

                if (m_Callback != null)
                {
                    m_Callback.Invoke(result);
                }
            });
        }

        public void LoadAssetAsync(string name, Action<GameObject> callback)
        {
            m_Callback = callback;

            m_ResLoader.LoadAsync();
        }

        public void Release()
        {
            m_ResLoader.Recycle2Cache();
        }

        #endregion

        private void OnLoadDone(bool result, IRes res)
        {
            if (!result)
            {
                Release();
                return;
            }

            GameObject go = res.asset as GameObject;

            if (go == null)
            {
                Log.e("Asset Is Invalid GameObject:" + m_Name);
                Release();
                return;
            }

            if (m_Callback != null)
            {
                m_Callback.Invoke(go);
            }
        }
    }

}