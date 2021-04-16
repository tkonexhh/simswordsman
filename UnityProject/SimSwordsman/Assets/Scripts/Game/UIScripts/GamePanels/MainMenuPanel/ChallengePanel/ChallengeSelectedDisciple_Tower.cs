using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class ChallengeSelectedDisciple_Tower : ChallengeSelectedDisciple
    {
        [SerializeField] private GameObject m_ObjHp;
        [SerializeField] private Image m_ImgHp;

        private TowerCharacterDB m_TowerCharacterDB;

        protected override void OnInit()
        {
            if (this.m_CharacterItem != null)
            {
                int id = this.m_CharacterItem.id;
                m_TowerCharacterDB = GameDataMgr.S.GetPlayerData().towerData.GetTowerCharacterByID(id);
            }
        }

        protected override void OnRefreshPanelInfo()
        {
            if (this.m_CharacterItem == null)
            {
                m_ObjHp.SetActive(false);
                return;
            }

            m_ObjHp.SetActive(true);

            if (m_TowerCharacterDB == null)
            {
                SetHpRate(1);
            }
            else
            {
                SetHpRate((float)m_TowerCharacterDB.hpRate);
            }
        }

        void SetHpRate(float rate)
        {
            m_ImgHp.fillAmount = rate;
        }
    }

}