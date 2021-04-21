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
        [SerializeField] private GameObject m_ObjEnemyIconRoot;
        [SerializeField] private List<TowerEnemyIcon> m_EnemyIcons;
        [SerializeField, Header("解锁状态")] private GameObject m_ObjUnlock;
        [SerializeField] private Button m_BtnFight;
        [SerializeField] private RewardItemIcon m_RewardItemIcon;
        [SerializeField] private Text m_TxtRewardNum;

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
            m_TxtLevel.text = "第" + ChineseHelper.NumToChinese(m_Level) + "关";
            var towerLevelConfigDB = GameDataMgr.S.GetPlayerData().towerData.GetLevelConfigByIndex(m_Level - 1);
            var enemyConfig = towerLevelConfigDB.enemyConfig;
            var tableEnemyConfig = TDTowerEnemyConfigTable.GetData(enemyConfig.id).enemyHeadIcon;
            var icons = Helper.String2ListString(tableEnemyConfig, ";");
            for (int i = 0; i < m_EnemyIcons.Count; i++)
            {
                var sp = SpriteHandler.S.GetSprite(AtlasDefine.EnmeyHeadIconsAtlas, "enemy_icon_" + icons[i]);
                //TODO 直接从表里取头像
                m_EnemyIcons[i].SetEnemy(sp);
                // m_EnemyIcons[i].SetEnemy(SpriteHandler.S.GetSprite(AtlasDefine.EnmeyHeadIconsAtlas, "enemy_icon_qiubujun"));
            }
            int maxLvl = GameDataMgr.S.GetPlayerData().towerData.maxLevel;
            if (string.IsNullOrEmpty(towerLevelConfigDB.reward))
            {
                m_RewardItemIcon.gameObject.SetActive(false);
            }
            else
            {
                m_RewardItemIcon.gameObject.SetActive(true);
                var reward = RewardMgr.S.GetRewardBase(towerLevelConfigDB.reward);
                m_RewardItemIcon.SetReward(reward, panel);
                m_TxtRewardNum.text = "x" + reward.Count;
            }

            m_State = m_Level < maxLvl ? TowerItemState.Complete : (m_Level == maxLvl ? TowerItemState.Unlock : TowerItemState.Locked);
            RefeshUI();
        }

        private void RefeshUI()
        {
            m_ObjUnlock.SetActive(m_State == TowerItemState.Unlock);
            m_ObjLocked.SetActive(m_State == TowerItemState.Locked);
            m_ObjComplete.SetActive(m_State == TowerItemState.Complete);
            m_ObjEnemyIconRoot.SetActive(m_State != TowerItemState.Locked);
            if (m_State != TowerItemState.Unlock)
            {
                for (int i = 0; i < m_EnemyIcons.Count; i++)
                {
                    m_EnemyIcons[i].SetGrey(m_State == TowerItemState.Complete, m_Panel.greyMat);
                }
            }
        }

        private void OnClickFight()
        {
            TowerPanelChallengeToSelect arg = new TowerPanelChallengeToSelect();
            arg.level = m_Level;
            var allCharacter = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
            CommonUIMethod.BubbleSortForType(allCharacter, CommonUIMethod.SortType.AtkValue, CommonUIMethod.OrderType.FromBigToSmall);
            if (allCharacter.Count < 5)
            {
                float totalATk = 0;
                for (int i = 0; i < allCharacter.Count; i++)
                {
                    totalATk += allCharacter[i].atkValue;
                }
                totalATk /= (float)allCharacter.Count;
                arg.basicATK = totalATk;
            }
            else
            {
                float totalATk = 0;
                for (int i = 0; i < 5; i++)
                {
                    totalATk += allCharacter[i].atkValue;
                }
                totalATk /= 5;
                arg.basicATK = totalATk;
            }

            var towerConfig = TDTowerConfigTable.GetData(arg.level);
            if (towerConfig != null)
            {
                arg.basicATK *= towerConfig.atkNum;
            }
            arg.recommendATK = (long)(arg.basicATK * 5.5f);
            UIMgr.S.OpenPanel(UIID.SendDisciplesPanel, PanelType.Tower, arg);
            // UIMgr.S.OpenPanel(UIID.TowerSelectCharacterPanel, arg);
        }
    }


    public struct TowerPanelChallengeToSelect
    {
        public int level;
        public long recommendATK;
        public float basicATK;
    }

    public enum TowerItemState
    {
        Complete,
        Unlock,
        Locked,
    }



}