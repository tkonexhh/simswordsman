using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class ImgFontPre : MonoBehaviour
	{
		[SerializeField]
		private Text m_Cont;

		public void SetFontCont(string font)
		{
			m_Cont.text = font;
		}
	 
	}
	
}