using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class EnemyLoader : TSingleton<EnemyLoader>
    {
        private ResLoader m_EnemyLoader = null;
        private List<GameObject> m_EnemyObjList = new List<GameObject>();

        public void LoadEnemySync(int id, Action<GameObject> onLoadDone)
        {
            string prefabName = GetPrefabName(id);
            LoadEnemySync(prefabName, onLoadDone);
        }

        public void LoadEnemySync(string prefabName, Action<GameObject> onLoadDone)
        {
            if (m_EnemyLoader == null)
            {
                m_EnemyLoader = ResLoader.Allocate("EnemyLoader");
            }

            GameObject go = m_EnemyLoader.LoadSync(prefabName) as GameObject;
            GameObject enemy = GameObject.Instantiate(go);
            m_EnemyObjList.Add(enemy);
            onLoadDone.Invoke(enemy);
        }

        public void ReleaseAll()
        {

            m_EnemyLoader.ReleaseAllRes();

            m_EnemyObjList.ForEach(i =>
            {
                GameObject.Destroy(i);
            });

            m_EnemyObjList.Clear();
        }

        private string GetPrefabName(int id)
        {
            EnemyInfo enemyInfo = TDEnemyConfigTable.GetEnemyInfo(id);
            return enemyInfo.prefabName;
        }
    }

}