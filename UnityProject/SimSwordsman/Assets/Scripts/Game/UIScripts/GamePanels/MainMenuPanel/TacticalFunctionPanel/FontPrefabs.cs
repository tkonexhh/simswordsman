using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class FontPrefabs : MonoBehaviour
	{
		[SerializeField]
		private Text m_FontText;

		public void SetFontText(string text)
		{
			m_FontText.text = text;
		}
	}
}