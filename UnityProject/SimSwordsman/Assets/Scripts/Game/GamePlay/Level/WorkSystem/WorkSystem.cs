using Qarth;
using System.Collections.Generic;
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
                facilityType = (FacilityType)FaciityTypeID;
            return facilityType;
        }
    }
    public class WorkSystem : TSingleton<WorkSystem>
    {
        /// <summary>
        /// ���Թ����Ľ���(ð����)
        /// </summary>
        List<FacilityType> m_CanWorkFacilitys = new List<FacilityType>();
        /// <summary>
        /// �ѽ����Ľ���,���Բ������ݵĽ���
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
        /// <summary>
        /// �Ѿ�������ɣ�������ȡ�����Ľ���
        /// </summary>
        List<WorkCharacter> m_RewardFacilitys { get { return GameDataMgr.S.GetCountdownData().workCharacters; } }
        CharacterMgr characterMgr { get { return MainGameMgr.S.CharacterMgr; } }
        FacilityMgr facilityMgr { get { return MainGameMgr.S.FacilityMgr; } }
        TDFacilityLobby lobbyTable { get { return TDFacilityLobbyTable.GetData(facilityMgr.GetLobbyCurLevel()); } }

        List<CharacterItem> characterItemTemp = new List<CharacterItem>();
        List<FacilityType> tempList = new List<FacilityType>();
        private int m_MaxCanWorkFacilityLimit {
            get {
                return lobbyTable.workMaxAmount;
            }
        }
        string[] tempArray;

        private int m_WorkTimerID = -1;

        public void Init()
        {
            if (GameDataMgr.S.GetPlayerData().IsUnlockWorkSystem())
            {
                CheckData();

                BindingEvents();

                StartWork();
            }
            else {
                EventSystem.S.Register(EventID.OnUnlockWorkSystem, UnlockWorkSystem);
            }            
        }

        private void StartWork() 
        {
            Timer.S.Cancel(m_WorkTimerID);

            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby);

            TDFacilityLobby lobbyData = TDFacilityLobbyTable.GetData(lobbyLevel);
            
            if (lobbyData == null) 
            {
                lobbyData = TDFacilityLobbyTable.GetData(0);
            }

            if (lobbyData != null)
            {
                //TODO:��ʱ����
                m_WorkTimerID = Timer.S.Post2Really((x) =>
                {
                    try
                    {
                        tempList.Clear();

                        for (int i = 0; i < m_UnlockFacilitys.Count; i++)
                        {
                            FacilityType tempType = m_UnlockFacilitys[i];
                            if (m_CurrentWorkItem.ContainsKey(tempType) == false) 
                            {
                                tempList.Add(tempType);
                            }
                        }
                        if (tempList.Count == 0) return;

                        int index = UnityEngine.Random.Range(0, tempList.Count);
                        
                        FacilityType type = tempList[index];

                        if (m_CurrentWorkItem.ContainsKey(type) == false) 
                        {
                            EventSystem.S.Send(EventID.OnAddCanWorkFacility, type);
                        }                        
                    }
                    catch (Exception ex) {
                        UnityEngine.Debug.LogError("error:" + ex.ToString());
                    }                    
                }, lobbyData.workInterval, -1);
            }
            else {
                UnityEngine.Debug.LogError("error:");
            }            
        }

        void BindingEvents()
        {
            EventSystem.S.Register(EventID.OnStartUnlockFacility, OnStartUnlockFacilityCallBack);
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, OnStartUpgradeFacilityCallBack);
            EventSystem.S.Register(EventID.OnCountdownerStart, OnCountDownStartCallBack);
            EventSystem.S.Register(EventID.OnCountdownerEnd, OnCountDownEndCallBack);
        }

        void CheckData()
        {
            var unlockFacilityDataList = GameDataMgr.S.GetClanData().ownedFacilityData.GetUnlockFacilityList();

            for (int i = 0; i < unlockFacilityDataList.Count; i++)
            {
                FacilityItemDbData data = unlockFacilityDataList[i];

                FacilityType type = (FacilityType)data.id;

                if (IsCanShowWorkBubble(type) && m_UnlockFacilitys.Contains(type) == false) 
                {
                    m_UnlockFacilitys.Add(type);
                }
            }

            //TODO:������foreach��֪��������add����remove����ʱû�ҵ�������for
            for (int i = 0; i < m_RewardFacilitys.Count; i++)
            {
                var item = m_RewardFacilitys[i];

                if (item != null) 
                {
                    CharacterController controller = characterMgr.GetCharacterController(item.CharacterID);

                    controller.SetState(CharacterStateID.Working, item.GetFacilityType());

                    EventSystem.S.Send(EventID.OnAddWorkingRewardFacility, item.GetFacilityType(), controller);
                }                
            }
        }

        private bool IsCanShowWorkBubble(FacilityType type) 
        {
            if (type == FacilityType.None || type == FacilityType.BulletinBoard || type == FacilityType.TotalCount || type == FacilityType.PatrolRoom) {
                return false;
            }
            return true;
        }

        private void OnCountDownStartCallBack(int key, object[] param)
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
                m_CurrentWorkItem.Add(type, characterMgr.GetCharacterItem(cd.ID));
                //����״̬����
                characterMgr.GetCharacterController(cd.ID).SetState(CharacterStateID.Working, type);
            }
        }

        private void OnCountDownEndCallBack(int key, object[] param)
        {
            Countdowner cd = (Countdowner)param[0];
            if (cd.stringID.Equals("WorkFacilityCD"))//cd״̬
            {
                var type = (FacilityType)cd.ID;
                if (m_CDFacilitys.Contains(type))
                {
                    m_CDFacilitys.Remove(type);
                    AfterFacilityCD(type);
                }
            }
            else if (cd.stringID.Contains("FacilityWorking"))
            {
                tempArray = cd.stringID.Split(',');
                FacilityType type = (FacilityType)Enum.Parse(typeof(FacilityType), tempArray[1]);
                //���������ȡ�����б�
                AddRewardFacility(type, cd.ID);
            }
        }

        private void OnStartUnlockFacilityCallBack(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];

            if (IsCanShowWorkBubble(type) && m_UnlockFacilitys.Contains(type) == false) 
            {
                m_UnlockFacilitys.Add(type);

                if (CurrentWorkFacility() < m_MaxCanWorkFacilityLimit)
                    AddCanWorkFacility(type);
            }
        }

        /// <summary>
        /// ������ϵͳ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="param"></param>
        private void UnlockWorkSystem(int key, object[] param)
        {
            GameDataMgr.S.GetPlayerData().SetWorkSystem(true);

            CheckData();

            BindingEvents();

            AddCanWorkFacility(FacilityType.Lobby);

            EventSystem.S.Send(EventID.OnAddCanWorkFacility, FacilityType.Lobby);

            UpdateCanWorkFacilitys();

            StartWork();
        }

        /// <summary>
        /// ��ǰ������������
        /// </summary>
        /// <returns></returns>
        int CurrentWorkFacility()
        {
            return m_RewardFacilitys.Count + m_CanWorkFacilitys.Count + m_CurrentWorkItem.Count;
        }

        private void OnStartUpgradeFacilityCallBack(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];
            if (type == FacilityType.Lobby)
            {
                int nextLimit = lobbyTable.workMaxAmount;
                if (m_MaxCanWorkFacilityLimit < nextLimit)
                {
                    //�ɹ���������������
                    UpdateCanWorkFacilitys();
                }

                StartWork();
            }
        }

        /// <summary>
        /// ˢ�¿��Թ����Ľ���
        /// </summary>
        /// <returns></returns>
        public void UpdateCanWorkFacilitys()
        {
            int currentWorkCount = CurrentWorkFacility();
            //��ǰ�ɹ����Ľ�����ð�����ݣ����������������ʱ���ø���
            if (currentWorkCount >= m_MaxCanWorkFacilityLimit)
                return;

            foreach (var item in m_UnlockFacilitys)
            {
                if (CanWork(item) && !m_CanWorkFacilitys.Contains(item))
                    tempList.Add(item);
            }
            for (int i = 0; i < (m_MaxCanWorkFacilityLimit - currentWorkCount); i++)
            {
                if (tempList.Count > 0)
                {
                    int index = RandomHelper.Range(0, tempList.Count);
                    AddCanWorkFacility(tempList[index]);
                    tempList.RemoveAt(index);
                }
                else
                    break;
            }
            tempList.Clear();
        }

        /// <summary>
        /// �����һ�����еĵ���
        /// </summary>
        /// <returns></returns>
        CharacterItem RandomCharacterWorkFor()
        {
            var characters = characterMgr.GetAllCharacterList();
            characterItemTemp.Clear();
            foreach (var item in characters)
                if (item.IsFreeState())
                    characterItemTemp.Add(item);
            if (characterItemTemp.Count > 0)
                return characterItemTemp[RandomHelper.Range(0, characterItemTemp.Count)];
            else
                return null;
        }

        /// <summary>
        /// �ý����Ƿ������ǲ���ӹ���
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool CanWork(FacilityType type)
        {
            if (m_CurrentWorkItem.ContainsKey(type))
                return false;
            if (m_CDFacilitys.Contains(type))
                return false;
            foreach (var item in m_RewardFacilitys)
            {
                if (type == item.GetFacilityType())
                    return false;
            }
            return true;
        }

        void AfterFacilityCD(FacilityType type)
        {
            if (CurrentWorkFacility() < m_MaxCanWorkFacilityLimit)
                AddCanWorkFacility(type);
        }
        void AddCanWorkFacility(FacilityType type)
        {
            if (!m_CanWorkFacilitys.Contains(type))
            {
                m_CanWorkFacilitys.Add(type);
                //������Ϣ
                //EventSystem.S.Send(EventID.OnAddCanWorkFacility, type);
                EventSystem.S.Send(EventID.OnRawMaterialChangeEvent);
            }
        }
        void AddRewardFacility(FacilityType type, int characterid)
        {
            if (m_CurrentWorkItem.ContainsKey(type))
                m_CurrentWorkItem.Remove(type);

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
                GameDataMgr.S.GetCountdownData().SetDataDirty();
                //������Ϣ
                CharacterController controller = characterMgr.GetCharacterController(temp.CharacterID);
                EventSystem.S.Send(EventID.OnAddWorkingRewardFacility, type, controller);
            }
        }

        /// <summary>
        /// �õ�����
        /// </summary>
        /// <param name="type"></param>
        public void GetReward(FacilityType type)
        {
            //����Ӧ����ͭǮɢ��ɵ�ͭǮ����Ч��
            GameDataMgr.S.GetPlayerData().AddCoinNum(lobbyTable.workPay);

            //�ӽ����б����Ƴ�
            for (int i = m_RewardFacilitys.Count - 1; i >= 0; i--)
            {
                if (m_RewardFacilitys[i].GetFacilityType() == type)
                {
                    //���ӻָ�������״̬
                    characterMgr.GetCharacterController(m_RewardFacilitys[i].CharacterID).SetState(CharacterStateID.Wander);
                    m_RewardFacilitys.RemoveAt(i);
                    break;
                }
            }
            GameDataMgr.S.GetCountdownData().SetDataDirty();
            UpdateCanWorkFacilitys();
            //����cd
            CountdownSystem.S.StartCountdownerWithMin("WorkFacilityCD", (int)type, lobbyTable.workInterval);
        }

        public int GetWorkPay()
        {
            return lobbyTable.workPay;
        }
        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool StartWork(FacilityType type)
        {
            //��ǲ����
            var character = RandomCharacterWorkFor();
            if (character == null)
                return false;

            CountdownSystem.S.StartCountdownerWithSec(GetStringId(type), character.id, lobbyTable.workTime);
            return true;
        }

        public static string GetStringId(FacilityType facilityType)
        {
            return string.Format("FacilityWorking,{0}", facilityType.ToString());
        }
    }
}