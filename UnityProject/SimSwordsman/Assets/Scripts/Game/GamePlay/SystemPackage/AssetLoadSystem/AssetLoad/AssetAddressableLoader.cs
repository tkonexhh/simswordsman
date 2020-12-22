using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameWish.Game
{
    public class AssetAddressableLoader : IAssetLoadAdapter
    {
        private Action<GameObject> m_LoadCallback = null;
        public AsyncOperationHandle<GameObject> m_LoadHandler;

        #region IAssetLoadAdapter
        public void InstantiateAsync(string name, Action<GameObject> callback)
        {
            m_LoadCallback = callback;
            Addressables.InstantiateAsync(name).Completed += OnLoadDone;
        }

        public void LoadAssetAsync(string name, Action<GameObject> callback)
        {
            m_LoadCallback = callback;
            Addressables.LoadAssetAsync<GameObject>(name).Completed += OnLoadDone;
        }

        public void Release()
        {
            Addressables.Release(m_LoadHandler);
        }
        #endregion

        private void OnLoadDone(AsyncOperationHandle<GameObject> obj)
        {
            m_LoadHandler = obj;

            if (m_LoadCallback != null)
            {
                m_LoadCallback.Invoke(m_LoadHandler.Result);
            }
        }
    }

}