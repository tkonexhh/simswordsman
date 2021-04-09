using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class TowerPanelChallengeItem : UListItemView
    {
        [SerializeField] private Text m_TxtLevel;
        [SerializeField] private Text m_TxtEnemyPoolID;
        [SerializeField, Header("解锁状态")] private GameObject m_ObjUnlock;
        [SerializeField] private Button m_BtnFight;

        [SerializeField, Header("完成状态")] private GameObject m_ObjComplete;
        [SerializeField] private Button m_BtnReward;
        [SerializeField, Header("未解锁状态")] private GameObject m_ObjLocked;

        private int m_Level;
        private int m_EnemyPoolID;
        private TowerItemState m_State;

        private void Awake()
        {
            m_BtnFight.onClick.AddListener(OnClickFight);
            m_BtnReward.onClick.AddListener(OnClickReward);
        }

        public void Init(int level)
        {
            m_Level = level;
            m_EnemyPoolID = GameDataMgr.S.GetPlayerData().towerData.GetEnemyPoolIDByIndex(m_Level - 1);
            m_TxtLevel.text = m_Level.ToString();
            m_TxtEnemyPoolID.text = m_EnemyPoolID.ToString();

            int maxLvl = GameDataMgr.S.GetPlayerData().towerData.maxLevel;
            m_State = m_Level < maxLvl ? TowerItemState.Complete : (m_Level == maxLvl ? TowerItemState.Unlock : TowerItemState.Locked);
            RefeshUI();
        }

        private void RefeshUI()
        {
            m_BtnReward.gameObject.SetActive(false);
            m_ObjUnlock.SetActive(m_State == TowerItemState.Unlock);
            m_ObjLocked.SetActive(m_State == TowerItemState.Locked);
            m_ObjComplete.SetActive(m_State == TowerItemState.Complete);
            if (m_State == TowerItemState.Complete)
            {
                //是否奖励关，是否已经领取奖励了
                if (TDTowerConfigTable.GetData(m_Level).rwardtype.Equals("Fcoin"))
                {
                    if (!GameDataMgr.S.GetPlayerData().towerData.HasReward(m_Level))
                    {
                        m_BtnReward.gameObject.SetActive(true);
                    }
                    else
                    {

                    }
                }
            }
        }

        private void OnClickFight()
        {
            TowerPanelChallenge arg = new TowerPanelChallenge();
            arg.level = m_Level;
            arg.enemyPoolID = m_EnemyPoolID;
            UIMgr.S.OpenPanel(UIID.TowerSelectCharacterPanel, arg);
        }

        private void OnClickReward()
        {
            m_BtnReward.gameObject.SetActive(false);
            MainGameMgr.S.TowerSystem.GetLevelReward(m_Level);
        }
    }


    public struct TowerPanelChallenge
    {
        public int level;
        public int enemyPoolID;
    }

    public enum TowerItemState
    {
        Complete,
        Unlock,
        Locked,
    }



}