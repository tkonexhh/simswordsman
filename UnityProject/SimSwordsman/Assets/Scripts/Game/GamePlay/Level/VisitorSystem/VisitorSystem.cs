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
        static int AppearVisitorCountdown = 50;
        private float m_AppearVisitorTimer = 0;
        private bool m_CanApperaVisitor = false;
        /// <summary>
        /// ������ʧ����ʱ
        /// </summary>

        /// <summary>
        /// �����������
        /// </summary>
        static int MaxVisitorCount = 1;

        static int MaxVisitorCountDaily = 12;

        public List<Visitor> CurrentVisitor = new List<Visitor>();


        public void Init()
        {
            EventSystem.S.Register(EventID.OnCreateVisitor, OnCreateVisitorCallBack);

            if (!GameDataMgr.S.GetPlayerData().UnlockVisitor)
            {
                EventSystem.S.Register(EventID.OnEndUpgradeFacility, HandleEvent);
            }
            else
            {
                m_CanApperaVisitor = true;
            }
        }

        public void Update()
        {
            if (m_CanApperaVisitor)
            {
                //��������
                m_AppearVisitorTimer += Time.deltaTime;
                if (m_AppearVisitorTimer >= AppearVisitorCountdown)
                {
                    CreateVisitor();
                }
            }

            CurrentVisitor.ForEach(visitor => { visitor.Update(); });
            CheckVisitorList();
        }

        private void OnCreateVisitorCallBack(int key, object[] param)
        {
            CreateVisitor();
            UnlockVisitor();
        }

        private void HandleEvent(int key, object[] param)
        {
            FacilityType facilityType = (FacilityType)param[0];
            if (facilityType == FacilityType.Lobby && MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(facilityType) >= m_UnlockLevel)
            {
                UnlockVisitor();
            }
        }

        private void UnlockVisitor()
        {
            GameDataMgr.S.GetPlayerData().UnlockVisitor = true;
            GameDataMgr.S.GetPlayerData().SetDataDirty();

            m_CanApperaVisitor = true;
            EventSystem.S.UnRegister(EventID.OnEndUpgradeFacility, HandleEvent);
        }

        void CreateVisitor()
        {
            // Debug.LogError("CreateVisitor");
            if (GameDataMgr.S.GetPlayerData().visitorCount >= MaxVisitorCountDaily)
                return;

            if (CurrentVisitor.Count >= MaxVisitorCount)
                return;
            m_AppearVisitorTimer = 0;
            // Debug.LogError("CreateVisitor_Finally");
            Visitor visitor = new Visitor();
            visitor.VisitorCfgID = RandomHelper.Range(1, TDVisitorConfigTable.dataList.Count + 1);
            visitor.Reward = GetRandomReward(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby));
            CurrentVisitor.Add(visitor);
            CheckMainPanelBtn(CurrentVisitor.Count);

        }

        void CheckVisitorList()
        {
            for (int i = CurrentVisitor.Count - 1; i >= 0; i--)
            {
                if (CurrentVisitor[i].state == 2)
                {
                    CurrentVisitor.RemoveAt(i);
                    CheckMainPanelBtn(CurrentVisitor.Count);
                }
            }
        }

        void CheckMainPanelBtn(int count)
        {
            Timer.S.Post2Really((x) =>
            {
                EventSystem.S.Send(EventID.OnCheckVisitorBtn, count);
            }, .5f, 1);
        }

        /// <summary>
        /// ���һ��������Ʒ
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        RewardBase GetRandomReward(int level)
        {
            List<int> idList = null;
            if (!TDVisitorRewardConfigTable.rewardIDByMainLevelDic.TryGetValue(level, out idList))
                idList = TDVisitorRewardConfigTable.rewardIDByMainLevelDic[1];

            //��Ȩƽ��
            int all = 0;
            foreach (var item in idList)
            {
                all += TDVisitorRewardConfigTable.dataList[item].weight;
            }

            int value = RandomHelper.Range(0, all + 1);
            int resultindex = 1;
            int temp = 0;
            for (int i = 0; i < idList.Count; i++)
            {
                temp += TDVisitorRewardConfigTable.dataList[idList[i]].weight;
                if (value <= temp)
                {
                    resultindex = i;
                    break;
                }
            }
            //Debug.LogError(TDVisitorRewardConfigTable.dataList[idList[resultindex]].reward);
            var reward = RewardMgr.S.GetRewardBase(TDVisitorRewardConfigTable.dataList[idList[resultindex]].reward);
            if (reward.RewardItem == RewardItemType.Item)
            {
                // Debug.LogError(reward.KeyID);
                if (reward.KeyID == (int)RawMaterial.QingRock || reward.KeyID == (int)RawMaterial.WuWood)
                {
                    int nowQingRock = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType(RawMaterial.QingRock);
                    int nowWuWood = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType(RawMaterial.WuWood);
                    reward = new ItemReward(nowQingRock < nowWuWood ? (int)RawMaterial.QingRock : (int)RawMaterial.WuWood, reward.Count);
                }
                else if (reward.KeyID == (int)RawMaterial.CloudRock || reward.KeyID == (int)RawMaterial.SilverWood)
                {
                    int nowCloudRock = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType(RawMaterial.CloudRock);
                    int nowSliverWood = MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType(RawMaterial.SilverWood);
                    reward = new ItemReward(nowCloudRock < nowSliverWood ? (int)RawMaterial.CloudRock : (int)RawMaterial.SilverWood, reward.Count);
                }
            }
            return reward;
        }

        public void ShowInPanel(Visitor visitor)
        {
            visitor.ShowInPanel();
            CheckVisitorList();
            m_AppearVisitorTimer = 0;
        }

        public void Disappear(Visitor visitor)
        {
            visitor.Disappear();
            CheckVisitorList();
            m_AppearVisitorTimer = 0;
        }

    }
    public class Visitor
    {
        static int DisAppearVisitorCountdown = 180;
        private float m_DisappearTimer = 0;
        private bool m_IsCounting = true;
        public int VisitorCfgID;
        // public string IconName;
        public RewardBase Reward;

        public Visitor()
        {
            state = 0;
            m_DisappearTimer = 0;
            m_IsCounting = true;
        }

        public void Update()
        {

            if (m_IsCounting)
            {
                m_DisappearTimer += Time.deltaTime;
                if (m_DisappearTimer >= DisAppearVisitorCountdown)
                {
                    state = 2;
                    m_IsCounting = false;
                    m_DisappearTimer = 0;
                    DataAnalysisMgr.S.CustomEvent(DotDefine.visitor_overtime, Reward.KeyID.ToString());
                    //Debug.LogError("Remove Visitor");
                }
            }
        }

        /// <summary>
        /// ״̬ 0��δ��� 1��������ȡ���� 2���ر���ʧ
        /// </summary>
        public int state { get; private set; }
        public void Disappear()
        {
            state = 2;
            // m_IsCounting = true;
            // Timer.S.Cancel(m_CountdownID);
        }
        public void ShowInPanel()
        {
            state = 1;
            m_IsCounting = false;
            // Timer.S.Cancel(m_CountdownID);
        }
    }
}