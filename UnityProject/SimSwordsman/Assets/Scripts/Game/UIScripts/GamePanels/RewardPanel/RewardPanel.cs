using Qarth;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace GameWish.Game
{
    public class RewardPanel : AbstractAnimPanel
    {
        [SerializeField]
        private Transform m_RewardTran;
        [SerializeField]
        private Button m_CloseBtn; 
        [SerializeField]
        private GameObject m_LootFirework;

        [SerializeField]
        private GameObject m_ItemObj;

        public Action OnBtnCloseEvent = null;
        List<RewardPanelItem> m_Items = new List<RewardPanelItem>();

        protected override void OnUIInit()
        {
            base.OnUIInit();
            BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            if (args != null)
            {
                List<RewardBase> rewards = (List<RewardBase>)args[0];
                InitItems(rewards);
            }

            OpenDependPanel(EngineUI.MaskPanel, -1, null);
        }

        void InitItems(List<RewardBase> rewards)
        {
            for (int i = 0; i < rewards.Count; i++)
            {
                if (m_Items.Count < i + 1)
                {
                    GameObject obj = Instantiate(m_ItemObj, m_RewardTran);
                    RewardPanelItem item = obj.GetComponent<RewardPanelItem>();
                    m_Items.Add(item);
                }
                m_Items[i].gameObject.SetActive(true);
                m_Items[i].Init(this, rewards[i]);
            }


            if (m_Items.Count > rewards.Count)
            {
                for (int i = rewards.Count - 1; i < m_Items.Count - rewards.Count; i++)
                {
                    m_Items[i].gameObject.SetActive(false);
                }
            }

            Instantiate(m_LootFirework, transform).transform.localPosition = Vector3.zero;
        }

        private void BindAddListenerEvent()
        {
            //ÒôÐ§
            foreach (var item in transform.GetComponentsInChildren<Button>(true))
            {
                item.onClick.AddListener(() => AudioMgr.S.PlaySound(Define.SOUND_UI_BTN));
            }
            m_CloseBtn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                OnBtnCloseEvent?.Invoke();
                HideSelfWithAnim();
            });
        }



        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}