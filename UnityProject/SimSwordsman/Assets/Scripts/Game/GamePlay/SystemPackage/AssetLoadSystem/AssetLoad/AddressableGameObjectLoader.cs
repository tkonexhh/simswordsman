using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameWish.Game
{
    public class AddressableGameObjectLoader
    {
        protected Action<GameObject> m_LoadCallback = null;

        public AsyncOperationHandle<GameObject> m_LoadHandler;

        public void InstantiateAsync(string name, Action<GameObject> callback)
        {
            m_LoadCallback = callback;

            Addressables.InstantiateAsync(name).Completed += OnLoadDone;
        }

        public void Release()
        {
            if (m_LoadHandler.Result != null)
            {
                Addressables.ReleaseInstance(m_LoadHandler);
            }
        }

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