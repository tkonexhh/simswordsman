using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class ArenaRankRewardPanel : AbstractPanel
    {
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private Text m_TxtRank;
        [SerializeField] private Text m_RewardNum;

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
        }


        protected override void OnPanelOpen(params object[] args)
        {
            int level = (int)args[0];
            int reward = (int)args[1];

            m_TxtRank.text = level.ToString();
            m_RewardNum.text = reward.ToString();//
        }
    }

}