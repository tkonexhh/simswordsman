using Qarth;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameWish.Game
{
    public class WorkSystem : MonoBehaviour
    {
        /// <summary>
        /// ���Թ����Ľ���
        /// </summary>
        List<FacilityType> m_CanWorkFacilitys = new List<FacilityType>();
        /// <summary>
        /// �ѽ����Ľ���
        /// </summary>
        List<FacilityType> m_UnlockFacilitys = new List<FacilityType>();
        /// <summary>
        /// key:����id value�������ĵ���
        /// </summary>
        Dictionary<FacilityType, CharacterItem> m_CurrentWorkItem = new Dictionary<FacilityType, CharacterItem>();
        /// <summary>
        /// cd�еĽ���
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
        }

        private void OnStart(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals("WorkFacilityCD"))//cd״̬
            {
                m_CDFacilitys.Add((FacilityType)cd.ID);
            }
            else if (cd.stringID.Contains("FacilityWorking"))
            {
                tempArray = cd.stringID.Split(',');
                FacilityType type = (FacilityType)Enum.Parse(typeof(FacilityType), tempArray[1]);
                m_CurrentWorkItem.Add(type, Mgr.GetCharacterItem(cd.ID));
                //����״̬����
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
            if (cd.stringID.Equals("WorkFacilityCD"))//cd״̬
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

                //���ӻָ�������״̬
                Mgr.GetCharacterController(cd.ID).SetState(CharacterStateID.Wander);
                m_CurrentWorkItem.Remove(type);

                //��齨��cd״̬�Ƿ���
                DateTime endtime = DateTime.Parse(cd.EndTime);
                var offset = DateTime.Now - endtime;
                int cdtime = TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel()).workInterval;
                if (offset.TotalMinutes < cdtime)
                    CountdownSystem.S.StartCountdownerWithMin("WorkFacilityCD", (int)type, (int)offset.TotalMinutes);
                else if (m_CDFacilitys.Contains(type))
                {
                    m_CDFacilitys.Remove(type);
                    UpdateCanWorkFacilitys();
                }
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
        /// �����һ�����еĵ���
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
        /// �ý����Ƿ������ǲ���ӹ���
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
            return true;
        }

        /// <summary>
        /// ˢ�¿��Թ����Ľ���
        /// </summary>
        /// <returns></returns>
        public void UpdateCanWorkFacilitys()
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
                //������Ϣ

            }
        }
        
        public bool StartWork(FacilityType type)
        {
            //��ǲ����
            var character = RandomCharacterWorkFor();
            if (character == null)
            {
                return false;
            }


            CountdownSystem.S.StartCountdownerWithMin(string.Format("WorkFacility,{0}", type.ToString()), character.id, TDFacilityLobbyTable.GetData(MainGameMgr.S.FacilityMgr.GetLobbyCurLevel()).workTime);
            return true;
        }


    }
}