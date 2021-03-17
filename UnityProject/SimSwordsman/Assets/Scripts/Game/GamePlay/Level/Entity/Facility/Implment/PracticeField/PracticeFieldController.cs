using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Qarth;
using System;

namespace GameWish.Game
{
    public class PracticeFieldController : FacilityController
    {
        public List<PracticeField> m_PracticeSlotList = new List<PracticeField>();

        private int m_MaxSlotCount = 6;

        public PracticeFieldController(FacilityType facilityType/*, int subId*/, FacilityView view) : base(facilityType/*, subId*/, view)
        {
            EventSystem.S.Register(EventID.DeleteDisciple, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnRefreshPracticeUnlock, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnPracticeVacancy, HandleAddListenerEvent);

            InitPracticeField();
        }
        ~PracticeFieldController()
        {
            EventSystem.S.UnRegister(EventID.DeleteDisciple, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnRefreshPracticeUnlock, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnPracticeVacancy, HandleAddListenerEvent);
        }
        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.DeleteDisciple:
                    foreach (var item in m_PracticeSlotList)
                        if (item.IsHaveSameCharacterItem((int)param[0]))
                            item.TrainingIsOver();
                    break;
                case EventID.OnRefreshPracticeUnlock:
                    RefreshExclamatoryMark(CheckSlotInfo());
                    break;
            }
        }

        private bool CheckSlotInfo()
        {
            foreach (var item in m_PracticeSlotList)
            {
                if (item.IsFree() && m_FacilityType == item.FacilityType)
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool CheckSubFunc()
        {
            if (m_FacilityState != FacilityState.Unlocked)
                return false;
            return CheckSlotInfo();
        }
        public BaseSlot GetIdlePracticeSlot(FacilityType  facilityType)
        {
            return m_PracticeSlotList.FirstOrDefault(i => i.IsEmpty() && i.FacilityType.Equals(facilityType));
        }
        private void InitPracticeField()
        {
            List<PracticeSoltDBData> practiceFieldDBDatas = GameDataMgr.S.GetClanData().GetPracticeFieldData();

            if (practiceFieldDBDatas.Count == 0)
            {
                LoopInit(FacilityType.PracticeFieldEast);
                LoopInit(FacilityType.PracticeFieldWest);
                return;
            }

            foreach (var item in practiceFieldDBDatas)
                m_PracticeSlotList.Add(new PracticeField(item,m_View));

            //for (int i = 0; i < m_PracticeSlotList.Count; i++)
            //{
            //    PracticeFieldView view = (PracticeFieldView)m_View;
            //    m_PracticeSlotList[i].SetSlotPos(view.GetSlotPos(i)); ;
            //}
        }

        /// <summary>
        /// 获取应用层练兵场信息
        /// </summary>
        /// <returns></returns>
        public List<PracticeField> GetPracticeField()
        {
            return m_PracticeSlotList;
        }

        private void LoopInit(FacilityType facilityType)
        {
            List<PracticeFieldLevelInfo> eastInfos = GetPracticeFieldLevelInfoList(facilityType);
            for (int i = 0; i < eastInfos.Count; i++)
                m_PracticeSlotList.Add(new PracticeField(eastInfos[i], i + 1, i + 1,m_View));
        }
        /// <summary>
        /// 升级刷新坑位状态
        /// </summary>
        /// <param name="facilityType"></param>
        /// <param name="facilityLevel"></param>
        public void RefreshPracticeUnlockInfo(FacilityType facilityType, int facilityLevel)
        {
            m_PracticeSlotList.ForEach(i =>
            {
                if (i.FacilityType == facilityType && i.UnlockLevel == facilityLevel)
                {
                    i.slotState = SlotState.Free;
                    GameDataMgr.S.GetClanData().RefresPracticeDBData(i);
                    EventSystem.S.Send(EventID.OnRefreshPracticeUnlock, i);
                }
            });
        }
        /// <summary>
        /// 根据类型获取所有的练功房信息
        /// </summary>
        /// <param name="facilityType"></param>
        /// <returns></returns>
        public List<PracticeFieldLevelInfo> GetPracticeFieldLevelInfoList(FacilityType facilityType)
        {
            return TDFacilityPracticeFieldTable.GetPracticeFieldLevelInfoList(facilityType);
        }
    }
}