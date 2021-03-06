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

        private bool m_IsLoadDoneSent = false;

        private ResLoader m_CharacterLoader = null;

        public void StartPreload()
        {
            if (m_CharacterLoader == null)
            {
                m_CharacterLoader = ResLoader.Allocate("CharacterLoader");
            }

            m_CharacterList = GameDataMgr.S.GetClanData().GetAllCharacterList();
            //if (characterList.Count == 0 && m_CharacterTaskRewardBubble != null)
            //{
            //    AssetPreloaderMgr.S.OnLoadDone();
            //}
            //else
            //{
            foreach (var item in m_CharacterList)
            {
                LoadCharactersync(item.id, item.quality, item.bodyId, item.trialClanType);
            }
            //}

            LoadCharacterRewardBubbleAsync();
            LoadCharacterWorkProgressBarAsync();
            LoadCharacterWorkTipAsync();
            LoadCharacterWorkRewardAsync();

            SendAssetLoadedMsg();
        }

        public GameObject GetCharacterGo(int id, CharacterQuality characterQuality, int bodyId, ClanType clanType)
        {
            //if (m_CharacterLoaderDic.ContainsKey(id))
            //{
            //    return GameObject.Instantiate(m_CharacterLoaderDic[id].GetResult());
            //    //return m_CharacterGoDic[id];
            //}
            string prefabName = GetPrefabName(characterQuality, bodyId, clanType);

            GameObject go = GameObjectPoolMgr.S.Allocate(prefabName);
            if (go != null)
                go.transform.parent = GameplayMgr.S.EntityRoot;

            return go;
        }


        public void LoadCharactersync(int id, CharacterQuality characterQuality, int bodyId, ClanType clanType, int maxCount = 5, int initCount = 2)
        {
            string prefabName = GetPrefabName(characterQuality, bodyId, clanType);

            GameObject obj = m_CharacterLoader.LoadSync(prefabName) as GameObject;

            GameObjectPoolMgr.S.AddPool(prefabName, obj, maxCount, initCount);

            //GameObject go = GetCharacterGo(id, characterQuality, bodyId);

            //onLoadDone?.Invoke(go);
            //AddressableGameObjectLoader loader = new AddressableGameObjectLoader();
            //loader.InstantiateAsync(prefabName, (obj) =>
            //{
            //    m_CharacterLoaderDic.Add(id, loader);

            //    onLoadDone?.Invoke(obj);
            //});
        }

        //public void RemoveCharacter(int id)
        //{
        //    if (m_CharacterLoaderDic.ContainsKey(id))
        //    {
        //        m_CharacterLoaderDic[id].Release();
        //        m_CharacterLoaderDic.Remove(id);
        //    }
        //    else
        //    {
        //        Log.e("Character loader not found: " + id);
        //    }
        //}

        public static string GetPrefabName(CharacterQuality characterQuality, int bodyId, ClanType clanType)
        {
            if (characterQuality != CharacterQuality.Hero)
                return "Character_" + characterQuality.ToString().ToLower() + "_" + bodyId; //TODO????????????????
            else
                return "Character_" + characterQuality.ToString() + "_" + clanType.ToString() + "_" + bodyId;
        }

        private void LoadCharacterRewardBubbleAsync()
        {
            string prefabName = Define.CHARACTER_TASK_REWARD_BUBBLE;
            //m_CharacterTaskRewardBubbleLoader = new AddressableGameObjectLoader();
            //m_CharacterTaskRewardBubbleLoader.InstantiateAsync(prefabName, (obj) =>
            //{
            //    m_CharacterTaskRewardBubble = obj;
            //    m_CharacterTaskRewardBubble.transform.position = new Vector3(-1000,0,0);

            //    SendAssetLoadedMsg();
            //});
            GameObject obj = m_CharacterLoader.LoadSync(prefabName) as GameObject;

            GameObjectPoolMgr.S.AddPool(prefabName, obj, 10, 2);
        }


        private void LoadCharacterWorkTipAsync()
        {
            //string prefabName = "WorkTip";
            //m_CharacterWorkTipLoader = new AddressableGameObjectLoader();
            //m_CharacterWorkTipLoader.InstantiateAsync(prefabName, (obj) =>
            //{
            //    m_CharacterWorkTip = obj;
            //    m_CharacterWorkTip.transform.position = new Vector3(-1000, 0, 0);

            //    SendAssetLoadedMsg();
            //});

            string prefabName = Define.CHARACTER_WORK_TIP;

            GameObject obj = m_CharacterLoader.LoadSync(prefabName) as GameObject;

            GameObjectPoolMgr.S.AddPool(prefabName, obj, 10, 2);
        }

        private void LoadCharacterWorkRewardAsync()
        {
            //string prefabName = "PopRewardCanvas";
            //m_CharacterWorkRewardLoader = new AddressableGameObjectLoader();
            //m_CharacterWorkRewardLoader.InstantiateAsync(prefabName, (obj) =>
            //{
            //    m_CharacterWorkRewardPop = obj;
            //    m_CharacterWorkRewardPop.transform.position = new Vector3(-1000, 0, 0);

            //    SendAssetLoadedMsg();
            //});
            string prefabName = Define.CHARACTER_WORK_REWARD_POP;

            GameObject obj = m_CharacterLoader.LoadSync(prefabName) as GameObject;

            GameObjectPoolMgr.S.AddPool(prefabName, obj, 10, 2);
        }

        private void LoadCharacterWorkProgressBarAsync()
        {
            //string prefabName = "WorkProgressBar";
            //m_CharacterWorkProgressBarLoader = new AddressableGameObjectLoader();
            //m_CharacterWorkProgressBarLoader.InstantiateAsync(prefabName, (obj) =>
            //{
            //    m_CharacterWorkProgressBar = obj;
            //    m_CharacterWorkProgressBar.transform.position = new Vector3(-1000, 0, 0);

            //    SendAssetLoadedMsg();
            //});
            string prefabName = Define.CHARACTER_WORK_PROGRESS_BAR;

            GameObject obj = m_CharacterLoader.LoadSync(prefabName) as GameObject;

            GameObjectPoolMgr.S.AddPool(prefabName, obj, 10, 2);
        }

        private void SendAssetLoadedMsg()
        {
            //if (IsAllAssetLoaded() && m_IsLoadDoneSent == false)
            {
                m_IsLoadDoneSent = true;

                AssetPreloaderMgr.S.OnLoadDone();
            }
        }
    }

}