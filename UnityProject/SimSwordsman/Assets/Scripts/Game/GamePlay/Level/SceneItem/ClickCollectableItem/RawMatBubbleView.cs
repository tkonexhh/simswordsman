using System.Collections;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using UnityEngine;


namespace GameWish.Game
{
	public class RawMatBubbleView : MonoBehaviour, IInputObserver
    {
        public RawMatItem rawMatItem;
        private Collider2D m_Collider;

        private void Awake()
        {
            InputMgr.S.AddTouchObserver(this);

            m_Collider = GetComponent<Collider2D>();
        }

        #region 
        public bool On_Drag(Gesture gesture, bool isTouchStartFromUI)
        {
            return false;
        }

        public bool On_LongTap(Gesture gesture)
        {
            return false;
        }

        public bool On_Swipe(Gesture gesture)
        {
            return false;
        }

        public bool On_TouchDown(Gesture gesture)
        {
            return false;
        }

        public bool On_TouchUp(Gesture gesture)
        {
            return false;
        }

        public bool BlockInput()
        {
            return true;
        }

        public int GetSortingLayer()
        {
            return 100;
        }
        #endregion

        public bool On_TouchStart(Gesture gesture)
        {
            if (gesture.IsOverUIElement())
                return false;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero, 1000, 1 << LayerMask.NameToLayer("Bubble"));
            if (hit.collider != null && hit.collider == m_Collider)
            {
                rawMatItem.OnClicked();

                return true;
            }

            return false;
        }
	}
}