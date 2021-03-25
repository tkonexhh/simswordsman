using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class TacticalFunctionPanel : AbstractAnimPanel
	{
        [Header("Upper")]
        [SerializeField]
        private Transform m_Upper;
        [SerializeField]
        private GameObject m_FontPrefabs;

        [Header("Middle Upper")]
        [SerializeField]
        private Image m_TaskImg; 
        [SerializeField]
        private Image m_TaskDesc;

        [Header("Middle")]
        [SerializeField]
        private Transform m_Middle;
        [SerializeField]
        private GameObject m_TacticalFunctionDisciple;

        [Header("MiddleDown")]
        [SerializeField]
        private Transform m_RewardTra;
        [SerializeField]
        private GameObject m_Reward;

        [Header("Down")]
        [SerializeField]
        private Button m_Accept;
        [SerializeField]
        private Button m_Refuse;    
        [SerializeField]
        private Button m_BlackExit;

        protected override void OnUIInit()
        {
            base.OnUIInit();
            OpenDependPanel(EngineUI.MaskPanel, -1, null);
        }


        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
	
}