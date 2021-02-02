using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class LivableRoomPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Image m_LivableRoomName;
        [SerializeField]
        private Text m_BriefIntroduction;

        [SerializeField]
        private Button m_CloseBtn;

        [SerializeField]
        private Transform m_LivableRoomTra;

        [SerializeField]
        private GameObject m_LivableItem;

        private FacilityType m_CurFacilityType;
        private List<CostItem> m_CostItems;
        private const int LivableRoomCount = 4;
        private int m_CurLevel;
        private LivableRoomLevelInfo m_LivableRoomLevelInfo = null;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }
        private bool m_IsLivableRoomEast;
        private List<FacilityType> m_LivableRoomList = new List<FacilityType>();

        private Dictionary<int, ItemICom> m_LivableRoomLevelInfoDic = new Dictionary<int, ItemICom>();

        private void RefreshPanelInfo()
        {
            switch (m_CurFacilityType)
            {
                case FacilityType.LivableRoomEast1:
                case FacilityType.LivableRoomEast2:
                case FacilityType.LivableRoomEast3:
                case FacilityType.LivableRoomEast4:
                    m_LivableRoomName.sprite = FindSprite("LivableRoomPanel_EastName");
                    m_IsLivableRoomEast = true;
                    break;
                case FacilityType.LivableRoomWest1:
                case FacilityType.LivableRoomWest2:
                case FacilityType.LivableRoomWest3:
                case FacilityType.LivableRoomWest4:
                    m_LivableRoomName.sprite = FindSprite("LivableRoomPanel_WeatName");
                    m_IsLivableRoomEast = false;
                    break;
            }
            if (m_IsLivableRoomEast)
            {
                m_LivableRoomList.Add(FacilityType.LivableRoomEast1);
                m_LivableRoomList.Add(FacilityType.LivableRoomEast2);
                m_LivableRoomList.Add(FacilityType.LivableRoomEast3);
                m_LivableRoomList.Add(FacilityType.LivableRoomEast4);
            }
            else
            {
                m_LivableRoomList.Add(FacilityType.LivableRoomWest1);
                m_LivableRoomList.Add(FacilityType.LivableRoomWest2);
                m_LivableRoomList.Add(FacilityType.LivableRoomWest3);
                m_LivableRoomList.Add(FacilityType.LivableRoomWest4);
            }
            FacilityConfigInfo facilityConfigInfo = MainGameMgr.S.FacilityMgr.GetFacilityConfigInfo(m_CurFacilityType);
            //m_BriefIntroduction.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_DESCRIBLE);
            m_BriefIntroduction.text = facilityConfigInfo.desc;

            for (int i = 0; i < m_LivableRoomList.Count; i++)
            {
                m_LivableRoomLevelInfoDic.Add(i, CreateLivableItem(m_LivableRoomList[i]));
            }
        }
            
        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            m_CurFacilityType = (FacilityType)args[0];
      
            RefreshPanelInfo();
     
        }

        private ItemICom CreateLivableItem(FacilityType facilityType)
        {
            m_CurLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(facilityType/*, m_SubID*/);
            m_LivableRoomLevelInfo = (LivableRoomLevelInfo)MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(facilityType, m_CurLevel + 1);
            m_CostItems = m_LivableRoomLevelInfo.GetUpgradeResCosts();
           
            ItemICom itemICom = Instantiate(m_LivableItem, m_LivableRoomTra).GetComponent<ItemICom>();
            itemICom.OnInit(this, null, facilityType);
            return itemICom;
        }
        
        private string GetIconName(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }
    }
}