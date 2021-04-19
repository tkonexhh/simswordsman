using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class ChallengePanelDisciple_Tower : ChallengePanelDisciple
    {
        [SerializeField] private GameObject m_ObjHp;
        [SerializeField] private Image m_ImgHp;
        [SerializeField] private GameObject m_ObjDead;

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
                m_ObjDead.SetActive(false);
            }
            else
            {
                m_ObjDead.SetActive(m_TowerCharacterDB.IsDead());
                SetHpRate((float)m_TowerCharacterDB.hpRate);
            }
        }

        protected override void BindAddListenerEvent()
        {
            m_ChoosePanelDisciple.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                if (m_TowerCharacterDB != null && m_TowerCharacterDB.IsDead())//已经死亡不能在选择
                {
                    return;
                }
                //TODO 超过每日20个也不能被选择

                IsSelected = !IsSelected;
                if (IsSelected && m_TowerCharacterDB == null && !MainGameMgr.S.TowerSystem.CanAddNewCharacter())
                {
                    FloatMessage.S.ShowMsg("伏魔塔人数已达到" + TowerDefine.MAX_CHARACT_NUM);
                    return;
                }
                EventSystem.S.Send(EventID.OnSelectedEvent, m_CharacterItem, IsSelected);
            });
        }

        void SetHpRate(float rate)
        {
            m_ImgHp.fillAmount = rate;
        }
    }

}