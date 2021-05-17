using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class FailRewardIInfoItem : MonoBehaviour
	{
		[SerializeField]
		private Image m_Icon;
		[SerializeField]
		private Text m_NameTxt;
		[SerializeField]
		private GameObject m_Tra;
		[SerializeField]
		private Text m_Cont;
		private FailItemData m_FailItemData;
		// Start is called before the first frame update
		void Start()
	    {
	        
	    }
	
	    // Update is called once per frame
	    void Update()
	    {
	        
	    }

        internal void OnInit(FailItemData item)
        {
			m_FailItemData = item;
			m_Icon.sprite = SpriteHandler.S.GetSprite(AtlasDefine.CombatSettlementAtlas, m_FailItemData.icon);
			m_NameTxt.text = CommonUIMethod.GetStrForColor(m_FailItemData.color, m_FailItemData.name);

			foreach (var i in m_FailItemData.conts)
            {
				Text text =  Instantiate(m_Cont.gameObject, m_Tra.transform).GetComponent<Text>();
				text.gameObject.SetActive(true);
				text.text = CommonUIMethod.GetStrForColor(m_FailItemData.color, i);
			}
		}
    }
	
}