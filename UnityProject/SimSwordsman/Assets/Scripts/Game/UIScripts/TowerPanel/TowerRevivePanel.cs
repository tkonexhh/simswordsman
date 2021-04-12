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

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
            m_BtnAD.onClick.AddListener(OnClickAD);
        }

        protected override void OnOpen()
        {
            OpenDependPanel(EngineUI.MaskPanel, -1);
        }

        private void OnClickAD()
        {
            AdsManager.S.PlayRewardAD("TowerRevive", (b) =>
            {
                CloseSelfPanel();
                //复活一名战斗力最高的已经死亡的角色
                var lst = GameDataMgr.S.GetPlayerData().towerData.towerCharacterLst;
                int id = -1;
                double maxAtk = -1;
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].IsDead() && !lst[i].revive)
                    {
                        double atk = MainGameMgr.S.CharacterMgr.GetCharacterController(lst[i].id).CharacterModel.GetAtk();
                        if (atk >= maxAtk)
                        {
                            maxAtk = atk;
                            id = lst[i].id;
                        }
                    }
                }

                if (id != -1)
                    GameDataMgr.S.GetPlayerData().towerData.ReviveTowerCharacter(id);

            });
        }
    }

}