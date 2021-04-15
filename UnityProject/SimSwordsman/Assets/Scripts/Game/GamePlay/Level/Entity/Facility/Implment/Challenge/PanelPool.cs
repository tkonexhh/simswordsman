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
        WeaponEnhancement = 1,
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
        /// <summary>
        /// 英雄试炼
        /// </summary>
        HeroTrial,
    }


    public class PromotionBase
    {
        private UpgradePanelType m_EventID;
        private int m_ChaID;
        protected float m_PreAtk;
        public PromotionBase(UpgradePanelType eventID, int chaID, float preAtk)
        {
            m_PreAtk = preAtk;
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
        public float GetPreAtk()
        {
            return m_PreAtk;
        }

    }

    public class DiscipleRiseStage : PromotionBase
    {
        private int m_Stage;

        public DiscipleRiseStage(int chaID, int stage, float preAtk) : base(UpgradePanelType.DiscipleAscendingSection, chaID, preAtk)
        {
            m_Stage = stage;
        }
        public int GetStage()
        {
            return m_Stage;
        }
    }
    public class WeaponEnhancement : PromotionBase
    {
        private CharacterArms m_ArmsItem;
        public WeaponEnhancement(int ChaID, float preAtk, CharacterArms armsItem) : base(UpgradePanelType.WeaponEnhancement, ChaID, preAtk)
        {
            m_ArmsItem = armsItem;
        }
        public CharacterArms GetArmsItem()
        {
            return m_ArmsItem;
        }
    }
    public class ArmorEnhancement : PromotionBase
    {
        private CharacterArmor m_ArmorItem;
        public ArmorEnhancement(int ChaID, float preAtk, CharacterArmor armorItem) : base(UpgradePanelType.ArmorEnhancement, ChaID, preAtk)
        {
            m_ArmorItem = armorItem;
        }
        public CharacterArmor GetArmorItem()
        {
            return m_ArmorItem;
        }
    }

    public class EquipAmrs : PromotionBase
    {
        private ArmsItem m_ArmsItem;
        public EquipAmrs(int ChaID, float preAtk, ArmsItem armsItem) : base(UpgradePanelType.EquipAmrs, ChaID, preAtk)
        {
            m_ArmsItem = armsItem;
        }

        public ArmsItem GetArmsItem()
        {
            return m_ArmsItem;
        }
    }
    public class EquipAmror : PromotionBase
    {
        private ArmorItem m_ArmorItem;
        public EquipAmror(int ChaID, float preAtk, ArmorItem armorItem) : base(UpgradePanelType.EquipAmror, ChaID, preAtk)
        {
            m_ArmorItem = armorItem;
        }

        public ArmorItem GetArmorItem()
        {
            return m_ArmorItem;
        }
    }

    public class LearnMartialArts : PromotionBase
    {
        private KungfuItem m_KungfuItem;
        public LearnMartialArts(int ChaID, float preAtk, KungfuItem kungfuItem) : base(UpgradePanelType.LearnMartialArts, ChaID, preAtk)
        {
            m_KungfuItem = kungfuItem;
        }

        public KungfuItem GetKungfuItem()
        {
            return m_KungfuItem;
        }
    }

    public class WugongBreakthrough : PromotionBase
    {
        private CharacterKongfuDBData m_CharacterKongfuDBData;

        public WugongBreakthrough(int ChaID, CharacterKongfuDBData characterKongfuDBData, float preAtk) : base(UpgradePanelType.BreakthroughMartialArts, ChaID, preAtk)
        {
            m_CharacterKongfuDBData = characterKongfuDBData;
        }
        public CharacterKongfuDBData GetWugongBreakthrough()
        {
            return m_CharacterKongfuDBData;
        }
    }

    public class HeroTrial : PromotionBase
    {
        private ClanType m_ClanType;
        public HeroTrial(int ChaID, ClanType clanType, float preAtk) : base(UpgradePanelType.HeroTrial, ChaID, preAtk)
        {
            m_ClanType = clanType;
        }
        public ClanType GetClanType()
        {
            return m_ClanType;
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