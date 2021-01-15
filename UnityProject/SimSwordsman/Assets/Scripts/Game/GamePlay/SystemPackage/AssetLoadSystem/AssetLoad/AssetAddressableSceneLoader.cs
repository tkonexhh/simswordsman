using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GameWish.Game
{
    public class AssetAddressableSceneLoader
    {
        private Action<SceneInstance> m_LoadCallback = null;
        public AsyncOperationHandle<SceneInstance> m_LoadHandler;

        public void LoadSceneAsync(string name, Action<SceneInstance> callback)
        {
            m_LoadCallback = callback;
            Addressables.LoadSceneAsync(name, LoadSceneMode.Additive).Completed += OnLoadDone;
        }

        public void Release()
        {
            Addressables.Release(m_LoadHandler);
        }

        private void OnLoadDone(AsyncOperationHandle<SceneInstance> obj)
        {
            m_LoadHandler = obj;

            //m_LoadHandler.Result.Activate();

            if (m_LoadCallback != null)
            {
                m_LoadCallback.Invoke(m_LoadHandler.Result);
            }
        }
    }

}