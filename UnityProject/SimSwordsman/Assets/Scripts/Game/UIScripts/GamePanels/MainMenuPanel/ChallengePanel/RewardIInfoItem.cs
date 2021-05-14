using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class RewardIInfoItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Text m_ExpCont;
        [SerializeField]
        private Transform m_DiscipleTra;
        [SerializeField]
        private Image m_SliderImg;
        [SerializeField]
        private Text m_LastLevel;    
        [SerializeField]
        private Text m_NextLevel;

        private bool m_IsSuccess;

        private LevelConfigInfo m_LevelConfigInfo = null;
        // private TowerLevelConfig m_TowerLevelConfig = null;
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
                case PanelType.Tower:
                case PanelType.Arena:
                    // m_TowerLevelConfig = (TowerLevelConfig)obj[1];
                    m_CharacterController = t as CharacterController;
                    break;
                default:
                    break;
            }
            m_IsSuccess = (bool)obj[2];
            m_ParentPanel = (CombatSettlementPanel)obj[3];
            m_CurCharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(m_CharacterController.CharacterId);
            RefreshPanelInfo();
            DiscipleHeadPortraitMgr.S.CreateDiscipleHeadIcon(m_CurCharacterItem, m_DiscipleTra, new Vector3(-132, -8, 0), new Vector3(0.2f, 0.2f, 1));
            //m_DisciplePhoto.sprite = m_ParentPanel.FindSprite(GetLoadDiscipleName(m_CurCharacterItem)); ;
        }
        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }
        public void SetButtonEvent(Action<object> action)
        {
        }

        public IEnumerator ExperienceGrowthBar(float start, float targit, Action action = null)
        {
            m_SliderImg.fillAmount = start;
            while (true)
            {
                yield return new WaitForSeconds(0.05f);
                m_SliderImg.fillAmount += 0.08f;
                if (m_SliderImg.fillAmount >= targit)
                {
                    action?.Invoke();
                    break;
                }
            }
        }

        private void RefreshPanelInfo()
        {
            m_DiscipleName.text = m_CurCharacterItem.name;
            m_LastLevel.text = CommonUIMethod.GetGrade(m_CurCharacterItem.lastLevel);
            m_NextLevel.text = CommonUIMethod.GetGrade(m_CurCharacterItem.level);
            if (m_IsSuccess)
            {
                if (m_PanelType == PanelType.Tower || m_PanelType == PanelType.Arena)
                {
                    m_SliderImg.fillAmount = ((float)m_CurCharacterItem.curExp / m_CharacterController.GetExpLevelUpNeed());
                    m_ExpCont.text = Define.PLUS + "0";
                    return;
                }
                int deltaLevle = m_CurCharacterItem.level - m_CurCharacterItem.lastLevel;
                float start = ((float)m_CurCharacterItem.lastExp / TDCharacterStageConfigTable.GetExpLevelUpNeed(m_CurCharacterItem));
                float ratio = ((float)m_CurCharacterItem.curExp / TDCharacterStageConfigTable.GetExpLevelUpNeed(m_CurCharacterItem));
                if (deltaLevle == 0)
                {
                    StartCoroutine(ExperienceGrowthBar(start, ratio));
                }
                else if (deltaLevle == 1)
                {

                    StartCoroutine(ExperienceGrowthBar(start, 1, () =>
                    {
                        StartCoroutine(ExperienceGrowthBar(0, ratio));
                    }));
                }
                else if (deltaLevle > 1)
                {

                    StartCoroutine(ExperienceGrowthBar(start, 1, () =>
                    {
                        for (int i = 0; i < deltaLevle - 1; i++)
                        {
                            StartCoroutine(ExperienceGrowthBar(0, 1, () =>
                            {
                                StartCoroutine(ExperienceGrowthBar(0, ratio));
                            }));
                        }
                    }));
                }
            }
            else
            {
                m_SliderImg.fillAmount = ((float)m_CurCharacterItem.curExp / m_CharacterController.GetExpLevelUpNeed());
            }


            if (!m_IsSuccess)
            {
                m_ExpCont.text = Define.PLUS + "0";
                return;
            }

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
                case PanelType.Tower:
                case PanelType.Arena:
                    //int exp = (int)FoodBuffSystem.S.Exp(m_TowerLevelConfig.rewardExp);
                    m_ExpCont.text = Define.PLUS + "0";
                    break;

                default:
                    break;
            }
        }
    }
}