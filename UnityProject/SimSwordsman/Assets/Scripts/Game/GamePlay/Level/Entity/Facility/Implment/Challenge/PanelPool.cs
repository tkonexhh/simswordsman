using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public enum UpgradePanelType
    {
        /// <summary>
        /// 武器强化
        /// </summary>
        WeaponEnhancement,
        /// <summary>
        /// 铠甲强化
        /// </summary>
        ArmorEnhancement,
        /// <summary>
        /// 装备武器
        /// </summary>
        EquipAmrs,
        /// <summary>
        /// 装备铠甲
        /// </summary>
        EquipAmror,
        /// <summary>
        /// 习得武功
        /// </summary>
        LearnMartialArts,
        /// <summary>
        /// 弟子升段
        /// </summary>
        DiscipleAscendingSection,
        /// <summary>
        /// 武功突破
        /// </summary>
        BreakthroughMartialArts,
    }


    public class PromotionBase
    {
        private UpgradePanelType m_EventID;
        private int m_ChaID;
        public PromotionBase(UpgradePanelType eventID, int chaID)
        {
            m_EventID = eventID;
            m_ChaID = chaID;
        }
    
        public UpgradePanelType GetEventID()
        {
            return m_EventID;
        }
        public int GetCharacterItem()
        {
            return m_ChaID;
        }
        public T ToSubType<T>() where T : PromotionBase
        {
            return this as T;
        }
    }

    public class DiscipleRiseStage : PromotionBase
    {
        private int m_Stage;

        public DiscipleRiseStage(UpgradePanelType eventID, int chaID, int stage) :base(eventID, chaID)
        {
            m_Stage = stage;
        }
        public int GetStage()
        {
            return m_Stage;
        }
    }

    public class LearnMartialArts : PromotionBase
    {
        public LearnMartialArts(UpgradePanelType eventID, int ChaID) : base(eventID, ChaID)
        {
        }
    }

    public class WugongBreakthrough : PromotionBase
    {
        private CharacterKongfuDBData m_CharacterKongfuDBData;

        public WugongBreakthrough(UpgradePanelType eventID, int ChaID, CharacterKongfuDBData characterKongfuDBData) : base(eventID, ChaID)
        {
            m_CharacterKongfuDBData = characterKongfuDBData;
        }
        public CharacterKongfuDBData GetWugongBreakthrough()
        {
            return m_CharacterKongfuDBData;
        }
    }





    public class PanelPool : TSingleton<PanelPool> 
    {
        private Queue<PromotionBase> m_Panels;

        /// <summary>
        /// 当前打开的面板是否关闭了
        /// </summary>

        public bool m_CurShowPanelIsOver;

        public bool CurShowPanelIsOver 
        {
            set 
            {
                m_CurShowPanelIsOver = value;
                if (!m_CurShowPanelIsOver)
                    DisplayPanel();
            }
            get { return m_CurShowPanelIsOver; }
        }

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
            m_Panels = new Queue<PromotionBase>();
        }

        public void AddPromotion(PromotionBase promotionPanel)
        {
            m_Panels.Enqueue(promotionPanel);
        }

        /// <summary>
        /// 开始显示
        /// </summary>
        public void DisplayPanel()
        {
            if (!m_CurShowPanelIsOver)
            {
                if (m_Panels.Count > 0)
                {
                    PromotionBase promotionModel = m_Panels.Dequeue();
                    UIMgr.S.OpenTopPanel(UIID.PromotionPanel, null, promotionModel);
                    CurShowPanelIsOver = true;
                }
            }
        }
    }
}