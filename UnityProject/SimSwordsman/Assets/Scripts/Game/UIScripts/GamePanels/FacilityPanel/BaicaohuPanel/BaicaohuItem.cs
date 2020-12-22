using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class BaicaohuItem : MonoBehaviour,ItemICom
	{
        [SerializeField]
        private Text m_ForgeHouseNameTxt;
        [SerializeField]
        private Text m_ForgeHouseContTxt;
        [SerializeField]
        private Text m_ForgeHouseEffecTxt;
        [SerializeField]
        private Text m_RawMaterialsValue;
        [SerializeField]
        private Text m_DurationTxt;
        [SerializeField]
        private Image m_ForgeHouseImg;
        [SerializeField]
        private Button m_MakeBtn;




        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            throw new NotImplementedException();
        }

        public void SetButtonEvent(Action<object> action)
        {
            throw new NotImplementedException();
        }


	}
	
}