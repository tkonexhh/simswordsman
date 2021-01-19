using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
	public class NoMaskWorldClickPanel : AbstractPanel
    {
        [SerializeField] private RectTransform m_HintRect;
        [SerializeField] private Text m_HintContent;

	    protected override void OnUIInit()
        {
            base.OnUIInit();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            if(args.Length > 0)
            {
                Vector2 pos = new Vector2(float.Parse(args[0].ToString()), float.Parse(args[1].ToString()));
                m_HintRect.anchoredPosition = pos;
                m_HintContent.text = TDLanguageTable.Get(args[2].ToString());
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
        }
    }
	
}