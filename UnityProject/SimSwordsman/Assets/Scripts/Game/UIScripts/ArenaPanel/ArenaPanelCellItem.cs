using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class ArenaPanelCellItem : IUListItemView
    {
        [SerializeField] private Image m_ImgBg;
        [SerializeField] private Text m_TxtLevel;
        [SerializeField] private Image m_ImgHeadIcon;
        [SerializeField] private Text m_TxtName;
        [SerializeField] private Text m_TxtATK;
        [SerializeField] private Button m_BtnFight;

        private int m_Index;
        private ArenaEnemyDB m_Data;

        private void Awake()
        {
            m_BtnFight.onClick.AddListener(OnClickFight);
        }

        public void SetItem(int index, ArenaEnemyDB data)
        {
            m_Index = index;
            m_Data = data;
            m_TxtLevel.text = index.ToString();
            m_TxtName.text = data.name;
            m_TxtATK.text = data.atk.ToString();
            int nowLevel = GameDataMgr.S.GetPlayerData().arenaData.nowLevel;
            bool canFight = index < nowLevel && index + ArenaDefine.ChallengeCount >= nowLevel;
            m_BtnFight.gameObject.SetActive(canFight);
            m_ImgBg.color = Color.white;
        }

        public void SetSelf(int level)
        {
            m_Data = null;
            m_TxtLevel.text = level.ToString();
            m_TxtName.text = GameDataMgr.S.GetClanData().clanName;
            m_TxtATK.text = 1000.ToString();
            m_ImgBg.color = Color.grey;
        }

        private void OnClickFight()
        {
            if (m_Data == null) return;

            ArenaCellToSend arg = new ArenaCellToSend();
            arg.index = m_Data.level;
            arg.recommendAtk = 100;
            arg.enemyData = m_Data;
            UIMgr.S.OpenPanel(UIID.SendDisciplesPanel, PanelType.Arena, arg);
        }
    }

    public class ArenaCellToSend
    {
        public int index;
        public long recommendAtk;
        public ArenaEnemyDB enemyData;
    }

}