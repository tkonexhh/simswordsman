using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class TowerRevivePanel : AbstractPanel
    {
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private Button m_BtnAD;
        [SerializeField] private Text m_TxtAtk;
        [SerializeField] private Text m_TxtCount;
        [SerializeField] private ChallengePanelDisciple m_Disciple;

        private int m_ID = -1;

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
            m_BtnAD.onClick.AddListener(OnClickAD);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);
            DataAnalysisMgr.S.CustomEvent(DotDefine.Tower_revive_happen);
            m_TxtCount.text = string.Format("剩余复活次数:{0}", Mathf.Max(0, TowerDefine.REVIVE_COUNT - GameDataMgr.S.GetPlayerData().recordData.towerRevive.dailyCount));

            //复活一名战斗力最高的已经死亡的角色
            var lst = GameDataMgr.S.GetPlayerData().towerData.towerCharacterLst;
            m_ID = -1;
            double maxAtk = -1;
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].IsDead() && !lst[i].revive)
                {
                    double atk = MainGameMgr.S.CharacterMgr.GetCharacterController(lst[i].id).CharacterModel.GetAtk();
                    if (atk >= maxAtk)
                    {
                        maxAtk = atk;
                        m_ID = lst[i].id;
                    }
                }
            }

            if (m_ID != -1)
            {
                var controller = MainGameMgr.S.CharacterMgr.GetCharacterController(m_ID);
                m_Disciple.Init(controller.CharacterModel.CharacterItem, this);
                m_TxtAtk.text = string.Format("功力: <color=#405787>{0}</color>", CommonUIMethod.GetTenThousandOrMillion((long)controller.CharacterModel.GetAtk()));
            }

        }

        private void OnClickAD()
        {
            AdsManager.S.PlayRewardAD("TowerRevive", (b) =>
            {
                CloseSelfPanel();


                if (m_ID != -1)
                {
                    GameDataMgr.S.GetPlayerData().recordData.AddTowerRevive();
                    GameDataMgr.S.GetPlayerData().towerData.ReviveTowerCharacter(m_ID);
                    DataAnalysisMgr.S.CustomEvent(DotDefine.Tower_revive_comfirm);
                }
            });
        }
    }

}