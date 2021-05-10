using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class ArenaAddCountPanel : AbstractPanel
    {
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private Button m_BtnAD;
        [SerializeField] private Text m_TxtNowCount;
        [SerializeField] private Text m_TxtTipCount;

        private ArenaData m_ArenaData;
        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
            m_BtnAD.onClick.AddListener(OnClickAD);
            // m_TxtTip.text=""
            m_ArenaData = GameDataMgr.S.GetPlayerData().arenaData;
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);

            m_TxtNowCount.text = m_ArenaData.challengeCount.ToString();
            m_TxtTipCount.text = string.Format("今日次数:{0}/{1}", m_ArenaData.adAddChallengeCount, ArenaDefine.Max_ADChallengeCount);
        }


        private void OnClickAD()
        {
            if (m_ArenaData.adAddChallengeCount <= 0)
            {
                FloatMessage.S.ShowMsg("今日次数已用完");
                return;
            }
            AdsManager.S.PlayRewardAD("TowerShopRefesh", (b) =>
            {
                AddCount();
            });
        }

        private void AddCount()
        {
            CloseSelfPanel();
            m_ArenaData.AddChallengeCount(ArenaDefine.AD_ChallengeCount);
            m_ArenaData.ReduceADChallengeCount();
        }
    }
}