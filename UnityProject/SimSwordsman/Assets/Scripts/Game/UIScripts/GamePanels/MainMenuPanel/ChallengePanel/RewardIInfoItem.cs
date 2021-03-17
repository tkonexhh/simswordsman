using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class RewardIInfoItem : MonoBehaviour,ItemICom
	{
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Text m_ExpCont;
        [SerializeField]
        private Image m_DisciplePhoto;
        [SerializeField]
        private Slider m_ExpProportion;

        private bool m_IsSuccess;

        private LevelConfigInfo m_LevelConfigInfo = null;
        private CharacterItem m_CurCharacterItem = null;
        private CharacterController m_CharacterController = null;
        private PanelType m_PanelType;
        private SimGameTask m_CurTaskInfo = null;
        private CombatSettlementPanel m_ParentPanel;


        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_PanelType = (PanelType)obj[0];
            switch (m_PanelType)
            {
                case PanelType.Task:
                    m_CharacterController = t as CharacterController;
                    m_CurTaskInfo = (SimGameTask)obj[1];
                    break;
                case PanelType.Challenge:
                    m_LevelConfigInfo = (LevelConfigInfo)obj[1];
                    m_CharacterController = t as CharacterController;
                    break;
                default:
                    break;
            }
            m_IsSuccess = (bool)obj[2];
            m_ParentPanel = (CombatSettlementPanel)obj[3];
            m_CurCharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(m_CharacterController.CharacterId);
            RefreshPanelInfo();
            m_DisciplePhoto.sprite = m_ParentPanel.FindSprite(GetLoadDiscipleName(m_CurCharacterItem)); ;
        }
        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }
        public void SetButtonEvent(Action<object> action)
        {
        }
    
        private void RefreshPanelInfo()
        {

            m_DiscipleName.text = m_CurCharacterItem.name;
            m_ExpProportion.value = ((float)m_CharacterController.GetCurExp()/ m_CharacterController.GetExpLevelUpNeed());

            switch (m_PanelType)
            {
                case PanelType.Task:
                    int expTask = (int)FoodBuffSystem.S.Exp(m_CurTaskInfo.CommonTaskItemInfo.expReward);
                    m_ExpCont.text = Define.PLUS + expTask.ToString();
                    break;
                case PanelType.Challenge:
                    int expChallenge = (int)FoodBuffSystem.S.Exp(m_LevelConfigInfo.GetExpRoleReward());
                    m_ExpCont.text = Define.PLUS + expChallenge;
                    break;
                default:
                    break;
            }
        }
	}
}