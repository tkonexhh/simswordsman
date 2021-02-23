using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class CharacterLoader : TSingleton<CharacterLoader>, IAssetPreloader
    {
        private List<CharacterItemDbData> m_CharacterList = null;
        //private Dictionary<int, GameObject> m_CharacterGoDic = new Dictionary<int, GameObject>();
        private Dictionary<int, AddressableGameObjectLoader> m_CharacterLoaderDic = new Dictionary<int, AddressableGameObjectLoader>();

        private GameObject m_CharacterTaskRewardBubble = null;
        private AddressableGameObjectLoader m_CharacterTaskRewardBubbleLoader = null;

        private AddressableGameObjectLoader m_CharacterWorkProgressBarLoader = null;
        private GameObject m_CharacterWorkProgressBar = null;

        private AddressableGameObjectLoader m_CharacterWorkTipLoader = null;
        private GameObject m_CharacterWorkTip = null;

        private int m_LoadedCharacterCount = 0;

        private bool m_IsLoadDoneSent = false;

        public void StartPreload()
        {
            m_CharacterList = GameDataMgr.S.GetClanData().GetAllCharacterList();
            //if (characterList.Count == 0 && m_CharacterTaskRewardBubble != null)
            //{
            //    AssetPreloaderMgr.S.OnLoadDone();
            //}
            //else
            //{
                foreach (var item in m_CharacterList)
                {
                    LoadCharacterAsync(item.id, item.quality, item.bodyId, (go) =>
                    {
                        go.transform.parent = GameplayMgr.S.EntityRoot.transform;
                        go.GetComponent<CharacterView>()?.HideBody();
                        m_LoadedCharacterCount++;

                        SendAssetLoadedMsg();
                    });
                }
            //}

            LoadCharacterRewardBubbleAsync();
            LoadCharacterWorkProgressBarAsync();
            LoadCharacterWorkTipAsync();
        }

        public GameObject GetCharacterRewardBubble()
        {
            return m_CharacterTaskRewardBubble;
        }

        public GameObject GetCharacterWorkProgressBar()
        {
            return m_CharacterWorkProgressBar;
        }

        public GameObject GetCharacterWorkTip()
        {
            return m_CharacterWorkTip;
        }

        public GameObject GetCharacterGo(int id)
        {
            if (m_CharacterLoaderDic.ContainsKey(id))
            {
                return GameObject.Instantiate(m_CharacterLoaderDic[id].GetResult());
                //return m_CharacterGoDic[id];
            }

            return null;
        }  

        public void LoadCharacterAsync(int id, CharacterQuality characterQuality, int bodyId, Action<GameObject> onLoadDone)
        {
            string prefabName = GetPrefabName(characterQuality, bodyId);

            AddressableGameObjectLoader loader = new AddressableGameObjectLoader();
            loader.InstantiateAsync(prefabName, (obj) =>
            {
                //m_CharacterGoDic.Add(id, obj);
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

        private void LoadCharacterRewardBubbleAsync()
        {
            string prefabName = "CharacterTaskRewardBubble";
            m_CharacterTaskRewardBubbleLoader = new AddressableGameObjectLoader();
            m_CharacterTaskRewardBubbleLoader.InstantiateAsync(prefabName, (obj) =>
            {
                m_CharacterTaskRewardBubble = obj;
                m_CharacterTaskRewardBubble.transform.position = new Vector3(-1000,0,0);

                SendAssetLoadedMsg();
            });
        }


        private void LoadCharacterWorkTipAsync()
        {
            string prefabName = "WorkTip";
            m_CharacterWorkTipLoader = new AddressableGameObjectLoader();
            m_CharacterWorkTipLoader.InstantiateAsync(prefabName, (obj) =>
            {
                m_CharacterWorkTip = obj;
                m_CharacterWorkTip.transform.position = new Vector3(-1000, 0, 0);

                SendAssetLoadedMsg();
            });
        }

        private void LoadCharacterWorkProgressBarAsync()
        {
            string prefabName = "WorkProgressBar";
            m_CharacterWorkProgressBarLoader = new AddressableGameObjectLoader();
            m_CharacterWorkProgressBarLoader.InstantiateAsync(prefabName, (obj) =>
            {
                m_CharacterWorkProgressBar = obj;
                m_CharacterWorkProgressBar.transform.position = new Vector3(-1000, 0, 0);

                SendAssetLoadedMsg();
            });
        }

        private bool IsAllAssetLoaded()
        {
            return m_LoadedCharacterCount >= m_CharacterList.Count && 
                m_CharacterTaskRewardBubble != null && m_CharacterWorkProgressBar != null
                && m_CharacterWorkTip != null;
        }

        private void SendAssetLoadedMsg()
        {
            if (IsAllAssetLoaded() && m_IsLoadDoneSent == false)
            {
                m_IsLoadDoneSent = true;

                AssetPreloaderMgr.S.OnLoadDone();
            }
        }
    }

}