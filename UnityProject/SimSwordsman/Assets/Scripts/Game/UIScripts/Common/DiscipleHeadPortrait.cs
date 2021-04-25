using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
	public class DiscipleHeadPortrait : MonoBehaviour
	{
		[SerializeField]
		private Image m_Head;
		[SerializeField]
		private Image m_Body;
		[SerializeField]
		private Mask m_Mask;
		private CharacterItem m_CharacterItem;
		void Start()
        {
			
		}

        // Update is called once per frame
        void Update()
	    {
	        
	    }

		public void OnInit(bool head)
		{
            if (head)
            {
				m_Mask.enabled = true;
			}
		}
	}
	
}