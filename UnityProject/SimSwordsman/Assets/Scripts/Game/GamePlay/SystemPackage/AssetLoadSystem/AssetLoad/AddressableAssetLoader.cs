using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Qarth;

namespace GameWish.Game
{
    public class AddressableAssetLoader<T>
    {
        protected Action<T> m_LoadCallback = null;

        public AsyncOperationHandle<T> m_LoadHandler;

        public void LoadAssetAsync(string name, Action<T> callback)
        {
            Log.i("Is loading asset: " + name);
            if (string.IsNullOrEmpty(name))
            {
                Log.e("Load asset name should not be empty");
                return;
            }

            m_LoadCallback = callback;

            Addressables.LoadAssetAsync<T>(name).Completed += OnLoadDone;
        }

        public void Release()
        {
            if (m_LoadHandler.Result != null)
            {
                Addressables.ReleaseInstance(m_LoadHandler);
            }
        }

        private void OnLoadDone(AsyncOperationHandle<T> obj)
        {
            m_LoadHandler = obj;

            if (m_LoadCallback != null)
            {
                m_LoadCallback.Invoke(m_LoadHandler.Result);
            }
        }
    }

}