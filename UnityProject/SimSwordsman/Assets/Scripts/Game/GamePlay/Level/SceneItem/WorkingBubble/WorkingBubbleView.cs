using System.Collections;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using UnityEngine;


namespace GameWish.Game
{
	public class WorkingBubbleView : MonoBehaviour, IInputObserver
    {
        public ClickWorkingBubble BubbleCtrl;
        public Collider2D Collider;

        private void Awake()
        {
            InputMgr.S.AddTouchObserver(this);
        }

        #region 
        public void On_Drag(Gesture gesture, bool isTouchStartFromUI)
        {
           
        }

        public void On_LongTap(Gesture gesture)
        {
          
        }

        public void On_Swipe(Gesture gesture)
        {
           
        }

        public void On_TouchDown(Gesture gesture)
        {
            
        }

        public void On_TouchUp(Gesture gesture)
        {

        }

        #endregion

        public void On_TouchStart(Gesture gesture)
        {
            if (gesture.IsOverUIElement())
                return;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero, 1000, 1 << LayerMask.NameToLayer("Bubble"));
            if (hit.collider != null && hit.collider == Collider)
            {
                BubbleCtrl.OnClicked();
            }
        }
	}
}