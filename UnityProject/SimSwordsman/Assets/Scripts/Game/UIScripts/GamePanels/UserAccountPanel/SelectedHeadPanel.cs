using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class SelectedHeadPanel : AbstractAnimPanel
	{
        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_ArrangeBtn;
        [SerializeField]
        private Transform m_SelectedList;
        [SerializeField]
        private GameObject m_Head;

        private List<AvatarConfig> m_HeadList = new List<AvatarConfig>();
        private List<HeadPhoto> m_HeadPhotoList = new List<HeadPhoto>();

        private string m_HeadPhoto = string.Empty;
        protected override void OnUIInit()
        {
            base.OnUIInit();
            EventSystem.S.Register(EventID.OnConfirmHeadPhoto, HandAddListenerEvent);
            BindAddListenerEvent();
        }
        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);
            OpenDependPanel(EngineUI.MaskPanel, -1, null);

            BindAddListenerEvent();

            GetInformationForNeed();

            InitHead();
        }


        private void InitHead()
        {
            if (m_HeadList!=null)
            {
                for (int i = 0; i < m_HeadList.Count; i++)
                {
                    HeadPhoto headPhoto = Instantiate(m_Head, m_SelectedList).GetComponent<HeadPhoto>();
                    headPhoto.OnInit(m_HeadList[i]);
                    m_HeadPhotoList.Add(headPhoto);
                }
            }
        }

        private void GetInformationForNeed()
        {
            m_HeadList.AddRange(TDAvatarTable.AvatarConfigConfigDic.Values);
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                HideSelfWithAnim();
            });
            m_ArrangeBtn.onClick.AddListener(()=> {
                if (!string.IsNullOrEmpty(m_HeadPhoto))
                {
                    GameDataMgr.S.GetPlayerData().headPhoto = m_HeadPhoto;
                    EventSystem.S.Send(EventID.OnRefreshSettingHeadPhoto);
                }
                HideSelfWithAnim();
            });

        }
        private void HandAddListenerEvent(int key, object[] param)
        {
            if ((EventID)key == EventID.OnConfirmHeadPhoto)
            {
                m_HeadPhoto = (string)param[0];
            }
        }
        protected override void OnClose()
        {
            base.OnClose();

            CloseDependPanel(EngineUI.MaskPanel);

            EventSystem.S.UnRegister(EventID.OnConfirmHeadPhoto, HandAddListenerEvent);
        }



        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }
    }
	
}