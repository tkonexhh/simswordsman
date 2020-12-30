using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class VisitorSystem : TSingleton<VisitorSystem>
	{
        /// <summary>
        /// ���ŵȼ������ǣ�
        /// </summary>
        int m_UnlockLevel = 2;
        /// <summary>
        /// ���˳��ֵ���ʱ
        /// </summary>
        int m_AppearVisitorCountdown = 90;
        /// <summary>
        /// ������ʧ����ʱ
        /// </summary>
        int m_DisAppearVisitorCountdown = 180;
        /// <summary>
        /// �����������
        /// </summary>
        int m_MaxVisitorCount = 2;

        public List<Visitor> CurrentVisitor = new List<Visitor>();
        

        public void Init()
        {
            if (!GameDataMgr.S.GetPlayerData().UnlockVisitor)
            {
                EventSystem.S.Register(EventID.OnEndUpgradeFacility, HandleEvent);
            }
            else
            {
                StartAppearVisitorCountdown();
            }
        }

        private void HandleEvent(int key, object[] param)
        {
            FacilityType facilityType = (FacilityType)param[0];
            if (facilityType == FacilityType.Lobby && MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(facilityType) == m_UnlockLevel)
            {
                GameDataMgr.S.GetPlayerData().UnlockVisitor = true;
                StartAppearVisitorCountdown();
                EventSystem.S.UnRegister(EventID.OnEndUpgradeFacility, HandleEvent);
            }
        }
        int m_StartAppearVisitorCDID = -1;
        public void StartAppearVisitorCountdown()
        {
            if (m_StartAppearVisitorCDID != -1)
                return;
            if (CurrentVisitor.Count < m_MaxVisitorCount)
            {
                m_StartAppearVisitorCDID = Timer.S.Post2Really(count =>
                {
                    CreateVisitor();
                    m_StartAppearVisitorCDID = -1;
                    StartAppearVisitorCountdown();
                }, m_AppearVisitorCountdown);
            }
        }

        void CreateVisitor()
        {
            if (CurrentVisitor.Count < m_MaxVisitorCount)
            {
                Debug.LogError("��������");

                Visitor visitor = new Visitor();
                visitor.VisitorCfgID = RandomHelper.Range(0, TDVisitorConfigTable.dataList.Count);
                visitor.Reward = GetRandomReward(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby));
                visitor.CountDown(m_DisAppearVisitorCountdown, ()=> 
                {
                    CheckVisitorList();
                    StartAppearVisitorCountdown();
                });

                CurrentVisitor.Add(visitor);
                //����ҳ����ʾ��ť
                CheckMainPanelBtn();
            }
        }


        public void CheckVisitorList()
        {
            bool isChange = false;
            for (int i = CurrentVisitor.Count - 1; i >= 0; i--)
            {
                if (!CurrentVisitor[i].IsShow)
                {
                    CurrentVisitor.RemoveAt(i);
                    isChange = true;
                }
            }
            if (isChange)
                CheckMainPanelBtn();
        }

        public void CheckMainPanelBtn()
        {
            EventSystem.S.Send(EventID.OnCheckVisitorBtn, CurrentVisitor.Count);
        }

        /// <summary>
        /// ���һ��������Ʒ
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        RewardBase GetRandomReward(int level)
        {
            return RewardMgr.S.GetRewardBase(TDVisitorRewardConfigTable.dataList[2].reward);
        }

	}
    public class Visitor
    {
        public int VisitorCfgID;
        public RewardBase Reward;

        public bool IsShow = true;
        public void Disappear()
        {
            IsShow = false;
            Timer.S.Cancel(m_CountdownID);
        }

        int m_CountdownID;
        public void CountDown(int time, Action onComplete)
        {
            m_CountdownID = Timer.S.Post2Really(count =>
            {
                Disappear();
                onComplete();
            }, time);
        }
    }
}