using System.Collections;
using System.Collections.Generic;
using HedgehogTeam.EasyTouch;
using UnityEngine;


namespace GameWish.Game
{
	public class CollectItemView : MonoBehaviour, IInputObserver,IClickedHandler
    {
        public ClickCollectableItem ClickCollectable;
        public Collider2D Collider;

        private void Awake()
        {
            InputMgr.S.AddTouchObserver(this);
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
            return 1;
        }

        public bool On_TouchStart(Gesture gesture)
        {
            if (gesture.IsOverUIElement())
                return false;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero, 1000, 1 << LayerMask.NameToLayer("Bubble"));
            if (hit.collider != null && hit.collider == Collider)
            {
                ClickCollectable.OnClicked();

                return true;
            }

            return false;
        }

        public void OnClicked()
        {
            ClickCollectable.OnClicked();
        }
        #endregion



    }
}