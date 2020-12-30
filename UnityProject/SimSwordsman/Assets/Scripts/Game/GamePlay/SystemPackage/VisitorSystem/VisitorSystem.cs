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
        /// 开放等级（主城）
        /// </summary>
        int m_UnlockLevel = 2;
        /// <summary>
        /// 客人出现倒计时
        /// </summary>
        int m_AppearVisitorCountdown = 90;
        /// <summary>
        /// 客人消失倒计时
        /// </summary>
        int m_DisAppearVisitorCountdown = 180;
        /// <summary>
        /// 客人最大数量
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
                Debug.LogError("创建客人");

                Visitor visitor = new Visitor();
                visitor.VisitorCfgID = RandomHelper.Range(1, TDVisitorConfigTable.dataList.Count + 1);
                visitor.Reward = GetRandomReward(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby));
                visitor.CountDown(m_DisAppearVisitorCountdown, ()=> 
                {
                    CheckVisitorList();
                    StartAppearVisitorCountdown();
                });

                CurrentVisitor.Add(visitor);
                CheckVisitorList();
            }
        }


        public void CheckVisitorList()
        {
            int count = 0;
            for (int i = CurrentVisitor.Count - 1; i >= 0; i--)
            {
                if (CurrentVisitor[i].state == 0)
                    count++;
                else if (CurrentVisitor[i].state == 2)
                    CurrentVisitor.RemoveAt(i);
            }
            CheckMainPanelBtn(count);
        }

        public void CheckMainPanelBtn(int count)
        {
            EventSystem.S.Send(EventID.OnCheckVisitorBtn, count);
        }

        /// <summary>
        /// 随机一个奖励物品
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        RewardBase GetRandomReward(int level)
        {
            return RewardMgr.S.GetRewardBase(TDVisitorRewardConfigTable.dataList[2].reward);
        }

        public void ShowInPanel(Visitor visitor)
        {
            visitor.ShowInPanel();
            CheckVisitorList();
        }

        public void Disappear(Visitor visitor)
        {
            visitor.Disappear();
            CheckVisitorList();
            StartAppearVisitorCountdown();
        }

	}
    public class Visitor
    {
        public int VisitorCfgID;
        public RewardBase Reward;

        public Visitor()
        {
            state = 0;
        }

        /// <summary>
        /// 状态 0：未点击 1：正在领取界面 2：关闭消失
        /// </summary>
        public int state { get; private set; }
        public void Disappear()
        {
            state = 2;
            Timer.S.Cancel(m_CountdownID);
        }
        public void ShowInPanel()
        {
            state = 1;
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