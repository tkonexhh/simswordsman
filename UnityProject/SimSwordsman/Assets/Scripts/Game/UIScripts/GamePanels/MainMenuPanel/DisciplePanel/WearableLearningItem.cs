using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class WearableLearningItem : MonoBehaviour,ItemICom
	{

        [SerializeField]
        private Text m_ArticlesName;
        [SerializeField]
        private Text m_Class;
        [SerializeField]
        private Text m_Number;

        [SerializeField]
        private Button M_SelectedBtn;

        //private EquipmentItem m_CurEquipmentItem = null;

        
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            //m_CurEquipmentItem = t as EquipmentItem;
            //m_ArticlesName.text = m_CurEquipmentItem.Name;
            //m_Class.text = CommonUIMethod.GetItemClass(m_CurEquipmentItem.ClassID);
            //m_Number.text = CommonUIMethod.GetItemNumber(m_CurEquipmentItem.Number);
        }

        public void SetButtonEvent(Action<object> action)
        {
            //M_SelectedBtn.onClick.AddListener(() =>
            //{
            //    action?.Invoke(m_CurEquipmentItem);
            //});
        }
	}
}