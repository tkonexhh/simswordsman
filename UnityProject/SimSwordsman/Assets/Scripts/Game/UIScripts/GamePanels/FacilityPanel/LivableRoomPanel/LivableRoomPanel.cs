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
        private Text m_LivableRoomName;
        [SerializeField]
        private Text m_BriefIntroduction;

        [SerializeField]
        private Button m_CloseBtn;

        [SerializeField]
        private Transform m_LivableRoomTra;

        [SerializeField]
        private GameObject m_LivableItem;

        private FacilityType m_CurFacilityType;
        private int m_SubID;

        private int m_LivableRoomCount = 4;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }

        private Dictionary<int, ItemICom> m_LivableRoomLevelInfoDic = new Dictionary<int, ItemICom>();

        private void RefreshPanelInfo()
        {
            switch (m_CurFacilityType)
            {
                case FacilityType.LivableRoomEast1:
                case FacilityType.LivableRoomEast2:
                case FacilityType.LivableRoomEast3:
                case FacilityType.LivableRoomEast4:
                    m_LivableRoomName.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOMEAST_NAME);
                    break;
                case FacilityType.LivableRoomWest1:
                case FacilityType.LivableRoomWest2:
                case FacilityType.LivableRoomWest3:
                case FacilityType.LivableRoomWest4:
                    m_LivableRoomName.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOMWEST_NAME);
                    break;
            }
            m_BriefIntroduction.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LIVABLEROOM_DESCRIBLE);





            for (int i = 1; i <= m_LivableRoomCount; i++)
            {
                m_LivableRoomLevelInfoDic.Add(i, CreateLivableItem(i));
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
            m_SubID = (int)args[1];
            RefreshPanelInfo();

        }

        private ItemICom CreateLivableItem(int subID)
        {
            ItemICom itemICom = Instantiate(m_LivableItem, m_LivableRoomTra).GetComponent<ItemICom>();
            itemICom.OnInit(this,null, m_CurFacilityType, subID);
            return itemICom;
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }
    }

}