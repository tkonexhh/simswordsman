using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class CharacterLoader : TSingleton<CharacterLoader>, IAssetPreloader
    {
        private Dictionary<int, GameObject> m_CharacterGoDic = new Dictionary<int, GameObject>();
        private Dictionary<int, AddressableGameObjectLoader> m_CharacterLoaderDic = new Dictionary<int, AddressableGameObjectLoader>();

        private int m_Count = 0;

        public void StartPreload()
        {
            var characterList = GameDataMgr.S.GetClanData().GetAllCharacterList();
            foreach (var item in characterList)
            {
                LoadCharacterSync(item.id, (go) => 
                {
                    m_CharacterGoDic.Add(item.id, go);
                    m_Count++;

                    if (m_Count >= characterList.Count)
                    {
                        AssetPreloaderMgr.S.OnLoadDone();
                    }
                });
            }
        }

        public GameObject GetCharacterGo(int id)
        {
            if (m_CharacterGoDic.ContainsKey(id))
                return m_CharacterGoDic[id];

            return null;
        }

        public void LoadCharacterSync(int id, Action<GameObject> onLoadDone)
        {
            string prefabName = GetPrefabName(id);

            AddressableGameObjectLoader loader = new AddressableGameObjectLoader();
            loader.InstantiateAsync(prefabName, (obj) =>
            {
                m_CharacterLoaderDic.Add(id, loader);

                onLoadDone?.Invoke(obj);
            });
        }


        private string GetPrefabName(int id)
        {
            return "Character1";
        }
    }

}