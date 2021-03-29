using HedgehogTeam.EasyTouch;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class ItemTipeMono : MonoBehaviour, IInputObserver, IClickedHandler
    {
        public CollectedObjType m_ItemTipsType;
        private PolygonCollider2D m_Collider;
          
        private void Awake()
        {
            InputMgr.S.AddTouchObserver(this);

            m_Collider = GetComponent<PolygonCollider2D>();
        }
        #region IInputObserver
        public bool BlockInput()
        {
            return false;
        }

        public int GetSortingLayer()
        {
            return 99;
        }

        public void OnClicked()
        {
            WorkConfigItem workConfigItem = TDWorkTable.GetWorkConfigItem(m_ItemTipsType);
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            if (workConfigItem.unlockHomeLevel >= lobbyLevel)//½âËø
            {
                WorldUIPanel.S.ShowWorkText(transform, workConfigItem.unlockDesc.name, workConfigItem.unlockDesc.desc, SpriteHandler.S.GetSprite(AtlasDefine.ItemIconItemIcon,GetIconName()));
            }
            else//Î´½âËø
            {
                WorldUIPanel.S.ShowWorkText(transform, workConfigItem.functionDesc.name, workConfigItem.functionDesc.desc, SpriteHandler.S.GetSprite(AtlasDefine.ItemIconItemIcon, GetIconName()));
            }
        }

        private string GetIconName()
        {
            switch (m_ItemTipsType)
            {
                case CollectedObjType.None:
                    break;
                case CollectedObjType.Fish:
                    break;
                case CollectedObjType.Chicken:
                    break;
                case CollectedObjType.Bear:
                    break;
                case CollectedObjType.Boar:
                    break;
                case CollectedObjType.Snake:
                    break;
                case CollectedObjType.Deer:
                    break;
                case CollectedObjType.WuWood:
                    break;
                case CollectedObjType.SilverWood:
                    break;
                case CollectedObjType.QingRock:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.QingRock);
                case CollectedObjType.CloudRock:
                    break;
                case CollectedObjType.Vine:
                    break;
                case CollectedObjType.Iron:
                    break;
                case CollectedObjType.Ganoderma:
                    break;
                case CollectedObjType.Well:
                    break;
                case CollectedObjType.Wolf:
                    break;
                case CollectedObjType.RoyalJelly:
                    break;
                case CollectedObjType.LotusRoot:
                    break;
                case CollectedObjType.Lotus:
                    break;
                case CollectedObjType.LotusLeaf:
                    break;
                default:
                    break;
            }
            return "";
        }

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

        public bool On_TouchStart(Gesture gesture)
        {
            if (gesture.IsOverUIElement())
                return false;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(gesture.position), Vector2.zero, 1000 /*1 << LayerMask.NameToLayer("Bubble")*/);
            if (hit.collider != null && hit.collider == m_Collider)
            {
                OnClicked();
                return true;
            }

            return false;
        }

        public bool On_TouchUp(Gesture gesture)
        {
            return false;
        }
        #endregion

     
	}
	
}