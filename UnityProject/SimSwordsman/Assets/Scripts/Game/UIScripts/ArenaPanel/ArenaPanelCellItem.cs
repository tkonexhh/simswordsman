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
        [SerializeField] private Image m_ImgLevelBg;
        [SerializeField] private Text m_TxtLevel;
        [SerializeField] private Image m_ImgHeadBg;
        [SerializeField] private Image m_ImgNameBg;
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
            UpdateLevelBg(index);
            m_TxtName.text = data.name;
            m_TxtATK.text = CommonUIMethod.GetTenThousandOrMillion(data.atk);
            int nowLevel = GameDataMgr.S.GetPlayerData().arenaData.nowLevel;
            bool canFight = index < nowLevel && index + ArenaDefine.ChallengeRange >= nowLevel;
            m_BtnFight.gameObject.SetActive(canFight);
            m_ImgBg.sprite = SpriteHandler.S.GetSprite("ArenaPanelAtlas", "jingjichang_liebiaoBG");
            m_ImgHeadBg.sprite = SpriteHandler.S.GetSprite("ArenaPanelAtlas", "jingjichang_liebiaoBG_renwuPicBG");
            m_ImgNameBg.sprite = SpriteHandler.S.GetSprite("ArenaPanelAtlas", "jingjichang_liebiaoBG_titleBG");
        }

        public void SetSelf(int level)
        {
            m_Data = null;
            m_TxtLevel.text = level.ToString();
            UpdateLevelBg(level);
            m_TxtName.text = GameDataMgr.S.GetClanData().clanName;
            long totalAtk = MainGameMgr.S.CharacterMgr.GetCharacterATK();
            m_TxtATK.text = CommonUIMethod.GetTenThousandOrMillion(totalAtk);
            m_ImgBg.sprite = SpriteHandler.S.GetSprite("ArenaPanelAtlas", "jingjichang_liebiaoBGMy");
            m_ImgHeadBg.sprite = SpriteHandler.S.GetSprite("ArenaPanelAtlas", "jingjichang_liebiaoBG_renwuPicBGMy");
            m_ImgNameBg.sprite = SpriteHandler.S.GetSprite("ArenaPanelAtlas", "jingjichang_liebiaoBG_titleBGMy");
        }

        private void UpdateLevelBg(int level)
        {
            string spName = "jingjichang_qizi" + Mathf.Min(4, level);
            if (m_ImgLevelBg.sprite.name != spName)
                m_ImgLevelBg.sprite = SpriteHandler.S.GetSprite("ArenaPanelAtlas", spName);

            if (level >= 4)
            {
                m_TxtLevel.transform.localPosition = new Vector3(0, -7.5f, 0);
            }
            else
            {
                m_TxtLevel.transform.localPosition = new Vector3(0, -1.5f, 0);
            }
        }

        private void OnClickFight()
        {
            if (m_Data == null) return;
            if (GameDataMgr.S.GetPlayerData().arenaData.adAddChallengeCount <= 0)
            {
                FloatMessage.S.ShowMsg("挑战次数不足");
                return;
            }

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