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
        [SerializeField] private List<TowerEnemyIcon> m_EnemyIcons;
        [SerializeField, Header("解锁状态")] private GameObject m_ObjUnlock;
        [SerializeField] private Button m_BtnFight;

        [SerializeField, Header("完成状态")] private GameObject m_ObjComplete;
        [SerializeField, Header("未解锁状态")] private GameObject m_ObjLocked;


        private int m_Level;
        private TowerPanel m_Panel;
        private TowerItemState m_State;

        private void Awake()
        {
            m_BtnFight.onClick.AddListener(OnClickFight);
        }

        public void Init(TowerPanel panel, int level)
        {
            if (m_Panel == null)
                m_Panel = panel;
            m_Level = level;
            m_TxtLevel.text = "第" + m_Level.ToString() + "关";
            var enemyConfig = GameDataMgr.S.GetPlayerData().towerData.GetEnemyPoolIDByIndex(m_Level - 1);
            for (int i = 0; i < m_EnemyIcons.Count; i++)
            {
                m_EnemyIcons[i].SetEnemy(enemyConfig.enemyIDLst[i]);
            }
            int maxLvl = GameDataMgr.S.GetPlayerData().towerData.maxLevel;
            m_State = m_Level < maxLvl ? TowerItemState.Complete : (m_Level == maxLvl ? TowerItemState.Unlock : TowerItemState.Locked);
            RefeshUI();
        }

        private void RefeshUI()
        {
            m_ObjUnlock.SetActive(m_State == TowerItemState.Unlock);
            m_ObjLocked.SetActive(m_State == TowerItemState.Locked);
            m_ObjComplete.SetActive(m_State == TowerItemState.Complete);
            for (int i = 0; i < m_EnemyIcons.Count; i++)
            {
                m_EnemyIcons[i].SetGrey(m_State == TowerItemState.Complete, m_Panel.greyMat);
            }
        }

        private void OnClickFight()
        {
            TowerPanelChallengeToSelect arg = new TowerPanelChallengeToSelect();
            arg.level = m_Level;
            UIMgr.S.OpenPanel(UIID.SendDisciplesPanel, PanelType.Tower, arg);
            // UIMgr.S.OpenPanel(UIID.TowerSelectCharacterPanel, arg);
        }
    }


    public struct TowerPanelChallengeToSelect
    {
        public int level;
        public long recommendATK;
    }

    public enum TowerItemState
    {
        Complete,
        Unlock,
        Locked,
    }



}