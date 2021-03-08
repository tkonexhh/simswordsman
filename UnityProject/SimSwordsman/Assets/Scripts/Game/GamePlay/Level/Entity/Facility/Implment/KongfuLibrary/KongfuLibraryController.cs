using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Qarth;
using System;

namespace GameWish.Game
{
    public class KongfuLibraryController : FacilityController
    {
        public List<KungfuLibraySlot> m_ReadingSlotList = new List<KungfuLibraySlot>();

        public const int MaxSlotCount = 4;

        public KongfuLibraryController(FacilityType facilityType/*, int subId*/, FacilityView view) : base(facilityType/*, subId*/, view)
        {
            EventSystem.S.Register(EventID.DeleteDisciple, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnRefresKungfuSoltInfo, HandleAddListenerEvent);
            EventSystem.S.Register(EventID.OnKongfuLibraryVacancy, HandleAddListenerEvent);

            InitKungfuField();
        }

        private void HandleAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.DeleteDisciple:
                    foreach (var item in m_ReadingSlotList)
                        if (item.IsHaveSameCharacterItem((int)param[0]))
                            item.TrainingIsOver();
                    break;
                case EventID.OnRefresKungfuSoltInfo:
                    RefreshExclamatoryMark(CheckSlotInfo());
                    break;
                default:
                    break;
            }
        }

        protected override bool CheckSubFunc()
        {
            if (m_FacilityState != FacilityState.Unlocked)
                return false;
            return CheckSlotInfo();
        }

        ~KongfuLibraryController()
        {
            EventSystem.S.UnRegister(EventID.DeleteDisciple, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnRefresKungfuSoltInfo, HandleAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnKongfuLibraryVacancy, HandleAddListenerEvent);
        }

        public override void SetState(FacilityState facilityState, bool isFile = false)
        {
            base.SetState(facilityState, isFile);
        }

        public KungfuLibraySlot GetIdlePracticeSlot()
        {
            return m_ReadingSlotList.FirstOrDefault(i => i.IsEmpty());
        }

        public List<KungfuLibraySlot> GetReadingSlotList()
        {
            return m_ReadingSlotList;
        }

        private void InitKungfuField()
        {
            List<KungfuSoltDBData> kungfuLibraryDBDatas = GameDataMgr.S.GetClanData().GetKungfuLibraryData();

            if (kungfuLibraryDBDatas.Count == 0)
            {
                LoopInit(FacilityType.KongfuLibrary);
                return;
            }

            foreach (var item in kungfuLibraryDBDatas)
                m_ReadingSlotList.Add(new KungfuLibraySlot(item));


            for (int i = 0; i < m_ReadingSlotList.Count; i++)
            {
                KongfuLibraryView view = (KongfuLibraryView)m_View;
                m_ReadingSlotList[i].SetSlotPos(view.GetSlotPos(i)); ;
            }
        }
        public void RefreshSlotInfo(int facilityLevel)
        {
            m_ReadingSlotList.ForEach(i =>
            {
                List<KongfuLibraryLevelInfo> infos = TDFacilityKongfuLibraryTable.GetSameSoltList(i);

                foreach (var item in infos)
                {
                    if (item.level == facilityLevel)
                    {
                        i.Warp(item);
                        GameDataMgr.S.GetClanData().RefresKungfuDBData(i);
                        EventSystem.S.Send(EventID.OnRefresKungfuSoltInfo, i);
                    }
                }
            });
        }
        /// <summary>
        /// 检测是否有空闲的抄经空位
        /// </summary>
        private bool CheckSlotInfo()
        {
            foreach (var item in m_ReadingSlotList)
            {
                if (item.IsFree())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 第一次打开时初始化坑位信息
        /// </summary>
        /// <param name="facilityType"></param>
        private void LoopInit(FacilityType facilityType)
        {
            int count = 0;
            List<KongfuLibraryLevelInfo> eastInfos = MainGameMgr.S.FacilityMgr.GetKungfuLibraryLevelInfoList(facilityType);
            for (int i = 0; i < eastInfos.Count; i++)
            {
                if (i - 1 >= 0)
                {

                    int lastPracticePosCount = eastInfos[i - 1].GetCurCapacity();
                    int curPracticePosCount = eastInfos[i].GetCurCapacity();

                    int delta = curPracticePosCount - lastPracticePosCount;
                    if (delta == 0)
                    {
                        count++;
                        continue;
                    }
                    else if (delta == 1)
                        m_ReadingSlotList.Add(new KungfuLibraySlot(eastInfos[i], i + 1 - count, i + 1));
                }
                else
                    m_ReadingSlotList.Add(new KungfuLibraySlot(eastInfos[i], i + 1, i + 1));
            }
        }
    }
}