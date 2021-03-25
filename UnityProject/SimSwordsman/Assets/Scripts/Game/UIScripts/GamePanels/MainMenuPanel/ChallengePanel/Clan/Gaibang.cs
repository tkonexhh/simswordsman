using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class Gaibang : ClanBase
	{
        public RectTransform bg;
        public RectTransform m_ViewPortRT;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }
        public override void Update()
        {
            base.Update();
        }

        private bool m_IsInit = false;
        private void Init() 
        {
            if (m_IsInit) return;
            
            m_IsInit = true;

            m_ViewPortRT = transform.Find("Scroll View/Viewport").GetComponent<RectTransform>();

            bg = m_ViewPortRT.Find("CheckpointTra/Bg").GetComponent<RectTransform>();
        }

        public void UpdateScrollRectValue()
        {
            Init();

            if (m_CurLevel >= 0 && m_CurLevel < m_Buttons.Length) 
            {
                RectTransform targetBtnRT = m_Buttons[m_CurLevel].GetComponent<RectTransform>();

                float bgHeight = bg.rect.height;

                float canvasHeight = m_ViewPortRT.rect.height;

                Vector3 childTraLocalPos = targetBtnRT.anchoredPosition;

                childTraLocalPos.y = Mathf.Abs(childTraLocalPos.y + bgHeight * .5f) ;

                childTraLocalPos.y -= canvasHeight * .5f;

                float bili = Mathf.Clamp01(childTraLocalPos.y / (bgHeight - canvasHeight));

                m_ScrollRect.verticalNormalizedPosition = bili;
            }            
        }
    }	
}