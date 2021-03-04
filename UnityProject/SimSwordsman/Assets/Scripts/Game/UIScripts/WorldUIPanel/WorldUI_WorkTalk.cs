using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class WorldUI_WorkTalk : WorldUIBindTransform
    {
        [SerializeField] private Text m_TxtTalk;


        public void SetText(string text)
        {
            m_TxtTalk.text = text;
        }
    }
}
