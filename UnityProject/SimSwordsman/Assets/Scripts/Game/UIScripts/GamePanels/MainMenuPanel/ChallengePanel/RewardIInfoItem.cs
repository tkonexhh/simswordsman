using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class RewardIInfoItem : MonoBehaviour,ItemICom
	{
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Text m_ExpCont;
        [SerializeField]
        private Image m_DisciplePhoto;
        [SerializeField]
        private Slider m_ExpProportion;

        private bool m_IsSuccess;

        private LevelConfigInfo m_LevelConfigInfo = null;
        private CharacterItem m_CurCharacterItem = null;
        private CharacterController m_CharacterController = null;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_LevelConfigInfo = (LevelConfigInfo)obj[0];
            m_IsSuccess = (bool)obj[1];
            m_CharacterController = t as CharacterController;
            m_CurCharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(m_CharacterController.CharacterId);

            RefreshPanelInfo();
        }


        private void RefreshPanelInfo()
        {

            m_DiscipleName.text = m_CurCharacterItem.name;

            m_ExpProportion.value = ((float)m_CharacterController.GetCurExp()/ m_CharacterController.GetExpLevelUpNeed());

            m_ExpCont.text = (m_ExpProportion.value * 100).ToString() + "%";
        }

        private void OnDestroy()
        {

        }

        public void SetButtonEvent(Action<object> action)
        {
        }
	}
}