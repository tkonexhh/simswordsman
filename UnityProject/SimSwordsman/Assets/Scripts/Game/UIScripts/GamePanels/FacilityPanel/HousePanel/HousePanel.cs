using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class HousePanel : AbstractAnimPanel
	{
        [SerializeField]
        private Text m_HouseCont;

        [SerializeField]
        private Button m_CloseBtn;

        [SerializeField]
        private Transform m_HouseTrans;

        [SerializeField]
        private GameObject m_HouseItem;

        protected override void OnUIInit()
        {
            base.OnUIInit();

            InitPanelInfo();

            BindAddListenerEvevt();
        }

        private void InitPanelInfo()
        {
            for (int i = 0; i < 10; i++)
            {
                CreateHouseItem();
            }
        }

        private void BindAddListenerEvevt()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }


        private void CreateHouseItem()
        {
            Instantiate(m_HouseItem, m_HouseTrans);
        }
    }
	
}