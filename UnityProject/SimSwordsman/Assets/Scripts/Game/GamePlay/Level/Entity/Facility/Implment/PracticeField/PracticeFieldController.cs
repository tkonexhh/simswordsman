using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Qarth;

namespace GameWish.Game
{
    public class PracticeFieldController : FacilityController
    {
        public List<PracticeField> m_PracticeSlotList = new List<PracticeField>();

        private int m_MaxSlotCount = 6;

        public PracticeFieldController(FacilityType facilityType/*, int subId*/, FacilityView view) : base(facilityType/*, subId*/, view)
        {
            InitPracticeField();
        }

        public BaseSlot GetIdlePracticeSlot()
        {
            return m_PracticeSlotList.FirstOrDefault(i => i.IsEmpty());
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
                m_PracticeSlotList.Add(new PracticeField(item));

            for (int i = 0; i < m_PracticeSlotList.Count; i++)
            {
                PracticeFieldView view = (PracticeFieldView)m_View;
                m_PracticeSlotList[i].SetSlotPos(view.GetSlotPos(i)); ;
            }
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
                m_PracticeSlotList.Add(new PracticeField(eastInfos[i], i + 1, i + 1));
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