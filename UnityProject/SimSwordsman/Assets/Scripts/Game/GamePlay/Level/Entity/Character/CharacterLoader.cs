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
            if (characterList.Count == 0)
            {
                AssetPreloaderMgr.S.OnLoadDone();
            }
            else
            {
                foreach (var item in characterList)
                {
                    LoadCharacterSync(item.id, item.quality, item.bodyId, (go) =>
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
        }

        public GameObject GetCharacterGo(int id)
        {
            if (m_CharacterGoDic.ContainsKey(id))
                return m_CharacterGoDic[id];

            return null;
        }

        public void LoadCharacterSync(int id, CharacterQuality characterQuality, int bodyId, Action<GameObject> onLoadDone)
        {
            string prefabName = GetPrefabName(characterQuality, bodyId);

            AddressableGameObjectLoader loader = new AddressableGameObjectLoader();
            loader.InstantiateAsync(prefabName, (obj) =>
            {
                m_CharacterLoaderDic.Add(id, loader);

                onLoadDone?.Invoke(obj);
            });
        }

        public void RemoveCharacter(int id)
        {
            if (m_CharacterLoaderDic.ContainsKey(id))
            {
                m_CharacterLoaderDic[id].Release();
                m_CharacterLoaderDic.Remove(id);
            }
            else
            {
                Log.e("Character loader not found: " + id);
            }
        }

        private string GetPrefabName(CharacterQuality characterQuality, int bodyId)
        {
            return "Character_" + characterQuality.ToString().ToLower() + "_" + bodyId; //TODO：优化获取方式？
        }
    }

}