using Qarth;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameWish.Game
{
    public class WorkCharacter
    {
        public int CharacterID;
        public int FaciityTypeID;

        private FacilityType facilityType;
        public FacilityType GetFacilityType()
        {
            if (facilityType == 0)
            {
                facilityType = (FacilityType)FaciityTypeID;
            }
            return facilityType;
        }

    }
    public class WorkSystem : MonoBehaviour
    {
        /// <summary>
        /// 可以工作的建筑
        /// </summary>
        List<FacilityType> m_CanWorkFacilitys = new List<FacilityType>();
        /// <summary>
        /// 已经工作完成，可以领取奖励的建筑
        /// </summary>
        List<WorkCharacter> m_RewardFacilitys { get { return GameDataMgr.S.GetCountdownData().workCharacters; } }
        /// <summary>
        /// 已解锁的建筑
        /// </summary>
        List<FacilityType> m_UnlockFacilitys = new List<FacilityType>();
        /// <summary>
        /// key:建筑id value：工作的弟子
        /// </summary>
        Dictionary<FacilityType, CharacterItem> m_CurrentWorkItem = new Dictionary<FacilityType, CharacterItem>();
        /// <summary>
        /// cd中的建筑
        /// </summary>
        List<FacilityType> m_CDFacilitys = new List<FacilityType>();

        CharacterMgr Mgr { get { return MainGameMgr.S.CharacterMgr; } }
        int m_MaxCanWorkFacilityLimit = 0;

        string[] tempArray;
        List<CharacterItem> characterItemTemp = new List<CharacterItem>();


        public void Init()
        {
            CheckData();
            EventSystem.S.Register(EventID.OnStartUnlockFacility, UnlockCheck);
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, LobbyLevelUp);
            EventSystem.S.Register(EventID.OnCountdownerStart, OnStart);
            //EventSystem.S.Register(EventID.OnCountdownerTick, OnTick);
            EventSystem.S.Register(EventID.OnCountdownerEnd, OnEnd);
        }

        void CheckData()
        {
            var data = GameDataMgr.S.GetClanData();
            foreach (FacilityType item in Enum.GetValues(typeof(FacilityType)))
            {
                if (item != FacilityType.None && item != FacilityType.TotalCount)
                {
                    if (!m_UnlockFacilitys.Contains(item) && data.GetFacilityData(item).facilityState == FacilityState.Unlocked)
                        m_UnlockFacilitys.Add(item);
                }
            }
            m_MaxCanWorkFacilityLimit = TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel()).workMaxAmount;

            foreach (var item in m_RewardFacilitys)
            {
                var type = (FacilityType)item.FaciityTypeID;
                if (!m_CurrentWorkItem.ContainsKey(type))
                {
                    Mgr.GetCharacterController(item.CharacterID).SetState(CharacterStateID.Working);
                    m_CurrentWorkItem.Add(type, Mgr.GetCharacterItem(item.CharacterID));
                }
            }
        }

        private void OnStart(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals("WorkFacilityCD"))//cd状态
            {
                m_CDFacilitys.Add((FacilityType)cd.ID);
            }
            else if (cd.stringID.Contains("FacilityWorking"))
            {
                tempArray = cd.stringID.Split(',');
                FacilityType type = (FacilityType)Enum.Parse(typeof(FacilityType), tempArray[1]);
                m_CurrentWorkItem.Add(type, Mgr.GetCharacterItem(cd.ID));
                //弟子状态更改
                Mgr.GetCharacterController(cd.ID).SetState(CharacterStateID.Working);
            }
        }

        //private void OnTick(int key, object[] param)
        //{
            //Countdowner cd = (Countdowner)param[0];
            //if (cd.stringID.Contains("FacilityWorking"))
            //{

            //    var tb = TDCollectConfigTable.dataList[cd.ID];

            //}
        //}

        private void OnEnd(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals("WorkFacilityCD"))//cd状态
            {
                var type = (FacilityType)cd.ID;
                if (m_CDFacilitys.Contains(type))
                {
                    m_CDFacilitys.Remove((FacilityType)cd.ID);
                    UpdateCanWorkFacilitys();
                }
            }
            else if (cd.stringID.Contains("FacilityWorking"))
            {
                tempArray = cd.stringID.Split(',');
                FacilityType type = (FacilityType)Enum.Parse(typeof(FacilityType), tempArray[1]);
                //添加至可领取奖励列表
                AddRewardFacility(type, cd.ID);
            }
        }

        private void UnlockCheck(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];
            UnlockFacility(type);
        }

        private void LobbyLevelUp(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];
            if (type == FacilityType.Lobby)
                m_MaxCanWorkFacilityLimit = TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel()).workMaxAmount;
        }
       
        /// <summary>
        /// 随机出一个空闲的弟子
        /// </summary>
        /// <returns></returns>
        CharacterItem RandomCharacterWorkFor()
        {
            var characters = Mgr.GetAllCharacterList();
            characterItemTemp.Clear();
            foreach (var item in characters)
            {
                if (item.IsFreeState())
                {
                    characterItemTemp.Add(item);
                }
            }
            if (characterItemTemp.Count > 0)
                return characterItemTemp[RandomHelper.Range(0, characterItemTemp.Count)];
            else
                return null;
        }

        void UnlockFacility(FacilityType type)
        {
            if (!m_UnlockFacilitys.Contains(type))
                m_UnlockFacilitys.Add(type);
        }

        
        /// <summary>
        /// 该建筑是否可以派遣弟子工作
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CanWork(FacilityType type)
        {
            if (!m_UnlockFacilitys.Contains(type))
                return false;
            if (m_CurrentWorkItem.ContainsKey(type))
                return false;
            if (m_CDFacilitys.Contains(type))
                return false;
            //foreach (var item in m_RewardFacilitys)
            //{
            //    if (item.GetFacilityType() == type)
            //        return false;
            //}
            return true;
        }

        /// <summary>
        /// 刷新可以工作的建筑
        /// </summary>
        /// <returns></returns>
        void UpdateCanWorkFacilitys()
        {
            List<FacilityType> list = new List<FacilityType>();
            foreach (var item in m_UnlockFacilitys)
            {
                if (CanWork(item))
                    list.Add(item);
            }
            for (int i = 0; i < m_MaxCanWorkFacilityLimit; i++)
            {
                if (list.Count > 0)
                {
                    int index = RandomHelper.Range(0, list.Count);
                    AddCanWorkFacility(list[index]);
                    list.RemoveAt(index);
                }
                else
                    break;
            }
        }

        void AddCanWorkFacility(FacilityType type)
        {
            if (!m_CanWorkFacilitys.Contains(type))
            {
                m_CanWorkFacilitys.Add(type);
                //发送消息

            }
        }
        void AddRewardFacility(FacilityType type, int characterid)
        {
            WorkCharacter temp = null;
            foreach (var item in m_RewardFacilitys)
            {
                if (item.GetFacilityType() == type)
                {
                    temp = item;
                    break;
                }
            }
            if (temp == null)
            {
                temp = new WorkCharacter();
                temp.FaciityTypeID = (int)type;
                temp.CharacterID = characterid;
                m_RewardFacilitys.Add(temp);

                GameDataMgr.S.GetPlayerData().SetDataDirty();
                //发送消息

            }
        }

        /// <summary>
        /// 得到奖励
        /// </summary>
        /// <param name="type"></param>
        public void GetReward(FacilityType type)
        {
            var tb = TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel());
            GameDataMgr.S.GetPlayerData().AddCoinNum(tb.workPay);
            //弟子恢复到待机状态
            Mgr.GetCharacterController(m_CurrentWorkItem[type].id).SetState(CharacterStateID.Wander);
            m_CurrentWorkItem.Remove(type);
            //从奖励列表里移除
            for (int i = m_RewardFacilitys.Count - 1; i >= 0; i--)
            {
                if (m_RewardFacilitys[i].GetFacilityType() == type)
                {
                    m_RewardFacilitys.RemoveAt(i);
                    break;
                }
            }
            //建筑cd
            CountdownSystem.S.StartCountdownerWithMin("WorkFacilityCD", (int)type, tb.workInterval);
        }

        public bool StartWork(FacilityType type)
        {
            //派遣弟子
            var character = RandomCharacterWorkFor();
            if (character == null)
            {
                return false;
            }


            CountdownSystem.S.StartCountdownerWithMin(string.Format("FacilityWorking,{0}", type.ToString()), character.id, TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel()).workTime);
            return true;
        }


    }
}