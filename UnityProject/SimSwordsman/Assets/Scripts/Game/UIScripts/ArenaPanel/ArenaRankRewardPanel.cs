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
        [SerializeField] private Text m_TxtTitle;
        [SerializeField] private Text m_RewardNum;

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
        }


        protected override void OnPanelOpen(params object[] args)
        {
            int level = (int)args[0];

            m_TxtTitle.text = string.Format("恭喜获得竞技场{0}名", level);
        }
    }

}