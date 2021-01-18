using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class EnemyLoader : TSingleton<EnemyLoader>
    {
        private List<AddressableGameObjectLoader> m_EnemyLoaderList = new List<AddressableGameObjectLoader>();

        public void LoadEnemySync(int id, Action<GameObject> onLoadDone)
        {
            string prefabName = GetPrefabName(id);

            AddressableGameObjectLoader loader = new AddressableGameObjectLoader();
            loader.InstantiateAsync(prefabName, (obj) =>
            {
                m_EnemyLoaderList.Add(loader);

                onLoadDone?.Invoke(obj);
            });
        }

        public void ReleaseAll()
        {
            m_EnemyLoaderList.ForEach(i => 
            {
                i.Release();
            });

            m_EnemyLoaderList.Clear();
        }

        private string GetPrefabName(int id)
        {
            return "Character2";
        }
    }

}