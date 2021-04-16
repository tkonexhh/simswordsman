using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class SendSelectedDisciple_Tower : SendSelectedDisciple
    {
        [SerializeField] private GameObject m_ObjHp;
        [SerializeField] private Image m_ImgHp;

        protected override void OnInit()
        {

        }

        protected override void OnRefreshPanelInfo()
        {
            if (this.m_CharacterItem == null)
            {
                m_ObjHp.SetActive(false);
                return;
            }

            m_ObjHp.SetActive(true);

            int id = this.m_CharacterItem.id;
            var towerCharacterDB = GameDataMgr.S.GetPlayerData().towerData.GetTowerCharacterByID(id);
            if (towerCharacterDB == null)
            {
                SetHpRate(1);
            }
            else
            {
                SetHpRate((float)towerCharacterDB.hpRate);
            }
        }

        void SetHpRate(float rate)
        {
            m_ImgHp.fillAmount = rate;
        }
    }

}