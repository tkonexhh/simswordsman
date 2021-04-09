using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace GameWish.Game
{
    public class RewardIInfoItem : MonoBehaviour, ItemICom
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
        private Coroutine m_Coroutine;

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
        /// <summary>
        /// 记录
        /// </summary>
        /// <param name="last"></param>
        /// <param name="next"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private IEnumerator InterpolationGrowth(float last, float next, Action action = null)
        {
            while (true)
            {
                yield return new WaitForSeconds(0.05f);
                last += 0.05f;
                //last = Mathf.Lerp(last, next, 4 * Time.deltaTime);
                m_ExpProportion.value = last;
                if (next - last < 0.1f)
                {
                    action?.Invoke();
                    break;
                }
            }
        }
        private void OnDestroy()
        {

        }

        private void RefreshPanelInfo()
        {
            m_DiscipleName.text = m_CurCharacterItem.name;
            m_ExpProportion.value = ((float)m_CurCharacterItem.lastExp / m_CharacterController.GetExpLevelUpNeed());

            //有升级的情况
            if (m_CurCharacterItem.level > m_CurCharacterItem.lastLevel)
            {
                int levelDelta = m_CurCharacterItem.level - m_CurCharacterItem.lastLevel;
                float lastRatio = ((float)m_CurCharacterItem.lastExp / m_CharacterController.GetExpLevelUpNeed());
                float curRatio = ((float)m_CurCharacterItem.curExp / m_CharacterController.GetExpLevelUpNeed());

                m_Coroutine = StartCoroutine(InterpolationGrowth(lastRatio, 1, () =>
                {
                    for (int i = 0; i < levelDelta - 1; i++)
                    {
                        m_Coroutine = StartCoroutine(InterpolationGrowth(0, 1, () =>
                        {
                            lastRatio = 0;
                            m_Coroutine = StartCoroutine(InterpolationGrowth(0, curRatio));
                        }));

                    }
                }));
            }
            //没有升级的情况
            else if (m_CurCharacterItem.level == m_CurCharacterItem.lastLevel)
            {
                float lastRatio = ((float)m_CurCharacterItem.lastExp / m_CharacterController.GetExpLevelUpNeed());
                float curRatio = ((float)m_CurCharacterItem.curExp / m_CharacterController.GetExpLevelUpNeed());
                m_Coroutine = StartCoroutine(InterpolationGrowth(lastRatio, curRatio));
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
                default:
                    break;
            }
        }
    }
}