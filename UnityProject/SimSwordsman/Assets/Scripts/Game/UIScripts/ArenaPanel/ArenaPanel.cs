using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;


namespace GameWish.Game
{
    public class ArenaPanel : AbstractPanel
    {
        [SerializeField] private Button m_BtnRule;
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private Button m_BtnShop;
        [SerializeField] private Text m_TxtCoin;
        [SerializeField] private Button m_BtnAddCount;
        [SerializeField] private Text m_TxtCount;
        [SerializeField] private IUListView m_ListView;
        [SerializeField] private ScrollRect m_ScrollRect;

        [Header("Close")]
        [SerializeField] private GameObject m_ArenaCloseBg;
        [SerializeField] private ArenaClose m_ArenaClose;

        [Header("My")]
        [SerializeField] private Text m_TxtMyRank;
        [SerializeField] private Text m_TxtMyName;
        [SerializeField] private Text m_TxtATK;


        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
            m_BtnShop.onClick.AddListener(OnClickShop);
            m_BtnAddCount.onClick.AddListener(OnClickAddCount);
            m_ListView.SetCellRenderer(OnCellRenderer);
            MainGameMgr.S.ArenaSystem.Enter();
        }


        protected override void OnOpen()
        {
            RegisterEvent(EventID.OnRefeshArenaCoin, (t, e) => { UpdateCoin(); });
            RegisterEvent(EventID.OnRefeshArenaChallengeCount, (t, e) => { UpdateCount(); });

            m_ListView.SetDataCount(GameDataMgr.S.GetPlayerData().arenaData.enemyLst.Count + 1);
            UpdateCoin();
            UpdateCount();
            UpdateMy();
            UpdateScroll();
            EnableArenaClose(!MainGameMgr.S.ArenaSystem.IsWithinTime());
            MainGameMgr.S.ArenaSystem.ShowRankReward();

        }

        private void EnableArenaClose(bool enable)
        {
            m_ArenaClose.gameObject.SetActive(enable);
            m_ArenaCloseBg.SetActive(enable);
        }

        private void UpdateScroll()
        {
            int level = GameDataMgr.S.GetPlayerData().arenaData.nowLevel;
            m_ScrollRect.verticalNormalizedPosition = 1.0f - (float)level / (float)(ArenaDefine.EnemyCount + 1);//.DoScrollVertical((float)level / (float)(ArenaDefine.EnemyCount + 1), 0.1f);
        }

        private void OnClickShop()
        {
            UIMgr.S.OpenPanel(UIID.ArenaShopPanel);
        }

        private void OnClickAddCount()
        {
            UIMgr.S.OpenPanel(UIID.ArenaAddCountPanel);
        }

        private void UpdateCoin()
        {
            m_TxtCoin.text = GameDataMgr.S.GetPlayerData().arenaData.coin.ToString();
        }

        private void UpdateCount()
        {
            m_TxtCount.text = string.Format("挑战次数: <color=#405788>{0}</color>", GameDataMgr.S.GetPlayerData().arenaData.challengeCount.ToString());
        }

        private void UpdateMy()
        {
            m_TxtMyRank.text = GameDataMgr.S.GetPlayerData().arenaData.nowLevel.ToString();
            m_TxtMyName.text = GameDataMgr.S.GetClanData().clanName;
            m_TxtATK.text = "功力:" + CommonUIMethod.GetTenThousandOrMillion(MainGameMgr.S.CharacterMgr.GetCharacterATK());
        }


        private void OnCellRenderer(Transform root, int index)
        {
            if (index + 1 == GameDataMgr.S.GetPlayerData().arenaData.nowLevel)
            {
                root.GetComponent<ArenaPanelCellItem>().SetSelf(GameDataMgr.S.GetPlayerData().arenaData.nowLevel);
            }
            else
            {
                int delta = index + 1 < GameDataMgr.S.GetPlayerData().arenaData.nowLevel ? 0 : 1;
                var data = GameDataMgr.S.GetPlayerData().arenaData.GetArenaEnemyDBByIndex(index - delta);
                root.GetComponent<ArenaPanelCellItem>().SetItem(index + 1, data);
            }

        }
    }

}