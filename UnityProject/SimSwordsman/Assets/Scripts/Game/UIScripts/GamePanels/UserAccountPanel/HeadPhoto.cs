using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class HeadPhoto : MonoBehaviour
    {
        [SerializeField]
        private Button m_HeadBtn;
        [SerializeField]
        private Image m_Head;
        [SerializeField]
        private Image m_BlackHead;
        [SerializeField]
        private GameObject m_State;
        [SerializeField]
        private Image m_Mask;
        [SerializeField]
        private Image m_Lock;

        private AvatarConfig m_AvatarConfig;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        private bool isUnlock = false;
        public void OnInit(AvatarConfig avatarConfig)
        {
            m_Head.gameObject.SetActive(false);
            m_BlackHead.gameObject.SetActive(false);
            m_Mask.gameObject.SetActive(false);
            m_Lock.gameObject.SetActive(false);
            EventSystem.S.Register(EventID.OnRefreshHeadPhoto,HandAddListenerEvent);

            BindAddListenerEvent();

            m_AvatarConfig = avatarConfig;

            m_Head.sprite = SpriteHandler.S.GetSprite(AtlasDefine.EnmeyHeadIconsAtlas, "enemy_icon_" + m_AvatarConfig.headIcon);
            m_BlackHead.sprite = SpriteHandler.S.GetSprite(AtlasDefine.EnmeyHeadIconsAtlas, "enemy_icon_" + m_AvatarConfig.headIcon);

            if (GameDataMgr.S.GetPlayerData().headPhoto.Equals(m_AvatarConfig.headIcon))
            {
                m_SelelctedState = SelectedState.Selected;
                RefreshPanelInfo();
            }

            if (m_AvatarConfig.unlockingCondition.mainLevel == 0 && m_AvatarConfig.unlockingCondition.subLevel == 0)
            {
                isUnlock = true;
                m_Head.gameObject.SetActive(true);
                return;
            }

            int curLevel = MainGameMgr.S.ChapterMgr.GetLevelProgressNumber(m_AvatarConfig.unlockingCondition.mainLevel);
            isUnlock = curLevel >= m_AvatarConfig.unlockingCondition.subLevel ? true : false;
            if (isUnlock)
            {//解锁
                //m_Head.material.SetFloat("_IsGrey", 0);
                m_Head.gameObject.SetActive(true);
            }
            else
            {//未解锁
                //m_Head.material.SetFloat("_IsGrey", 1);
                m_BlackHead.gameObject.SetActive(true);
                m_Mask.gameObject.SetActive(true);
                m_Lock.gameObject.SetActive(true);
            }
          
        }

        private void OnDestroy()
        {
            EventSystem.S.UnRegister(EventID.OnRefreshHeadPhoto,HandAddListenerEvent);
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            if ((EventID)key == EventID.OnRefreshHeadPhoto)
            {
                if (isUnlock && m_SelelctedState ==  SelectedState.Selected)
                {
                    m_SelelctedState = SelectedState.NotSelected;
                    RefreshPanelInfo();
                }
            }
        }

        private void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_State.SetActive(true);
                    break;
                case SelectedState.NotSelected:
                    m_State.SetActive(false);
                    break;
                default:
                    break;
            }
        }
        private void BindAddListenerEvent()
        {
            m_HeadBtn.onClick.AddListener(() =>
            {
                if (isUnlock)
                {
                    if (m_SelelctedState == SelectedState.NotSelected)
                    {
                        EventSystem.S.Send(EventID.OnRefreshHeadPhoto);
                        EventSystem.S.Send(EventID.OnConfirmHeadPhoto, m_AvatarConfig.headIcon);
                        m_SelelctedState = SelectedState.Selected;
                    }
                    else
                    {
                        m_SelelctedState = SelectedState.NotSelected;
                        EventSystem.S.Send(EventID.OnConfirmHeadPhoto, string.Empty);
                    }
                    RefreshPanelInfo();
                }
                else
                {
                    FloatMessage.S.ShowMsg("完成"+ m_AvatarConfig.unlockingCondition.mainLevel+ "-"+ m_AvatarConfig.unlockingCondition.subLevel + "关卡挑战解锁");
                }
            });

        }
    }

}