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
            Log.e("###############=====>4");
            string prefabName = GetPrefabName(id);
            Log.e("=======>prefabName=====>"+ prefabName);

            AddressableGameObjectLoader loader = new AddressableGameObjectLoader();
            loader.InstantiateAsync(prefabName, (obj) =>
            {
                m_EnemyLoaderList.Add(loader);
                Log.e("============>AddressableGameObjectLoader obj name<=====" + obj.name);
                onLoadDone?.Invoke(obj);
            });
            Log.e("###############=====>5");
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
            EnemyInfo enemyInfo = TDEnemyConfigTable.GetEnemyInfo(id);
            return enemyInfo.prefabName;
        }
    }

}