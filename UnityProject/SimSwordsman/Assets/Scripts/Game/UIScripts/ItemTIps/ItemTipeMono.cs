using HedgehogTeam.EasyTouch;
using Qarth;
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
            return 101;
        }

        public void OnClicked()
        {
            WorkConfigItem workConfigItem = TDWorkTable.GetWorkConfigItem(m_ItemTipsType);
            int lobbyLevel = MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
            if (workConfigItem==null)
            {
                PropConfigInfo propConfigInfo = TDItemConfigTable.GetPropConfigInfo(GetRawMaterial());
                if (propConfigInfo==null)
                    return;
                if (propConfigInfo.unlockHomeLevel <= lobbyLevel)//解锁
                {
                    Sprite propSprite = SpriteHandler.S.GetSprite(AtlasDefine.ItemIconAtlas, GetIconName());
                    if (propConfigInfo.functionDesc==null || propSprite==null)
                    {
                        Debug.LogError("propConfigInfo.functionDesc="+ propConfigInfo.functionDesc+"----"+ "propSprite="+ propSprite);
                        return;
                    }
                    WorldUIPanel.S.ShowWorkText(transform, propConfigInfo.functionDesc.name, propConfigInfo.functionDesc.desc, propSprite);
                }
                else//未解锁
                {
                    WorldUIPanel.S.ShowWorkText(transform, propConfigInfo.lockDesc.name, propConfigInfo.lockDesc.desc, SpriteHandler.S.GetSprite(AtlasDefine.ItemIconAtlas, GetIconName()));
                }
            }
            else
            {
                Sprite funcSprite = SpriteHandler.S.GetSprite(AtlasDefine.ItemIconAtlas, GetIconName());
                Sprite unlock = SpriteHandler.S.GetSprite(AtlasDefine.ItemIconAtlas, GetIconName());
                if (workConfigItem.functionDesc == null || funcSprite == null|| unlock == null|| workConfigItem.unlockDesc==null)
                {
                    Debug.LogError("workConfigItem.functionDesc=" + workConfigItem.functionDesc + "----" + "workSprite=" + funcSprite + "unlock=" + unlock + "---workConfigItem.unlockDesc="+ workConfigItem.unlockDesc);
                    return;
                }
                if (workConfigItem.unlockHomeLevel <= lobbyLevel)//解锁
                {
                    WorldUIPanel.S.ShowWorkText(transform, workConfigItem.functionDesc.name, workConfigItem.functionDesc.desc, funcSprite);
                }
                else//未解锁
                {
                    WorldUIPanel.S.ShowWorkText(transform, workConfigItem.unlockDesc.name, workConfigItem.unlockDesc.desc, unlock);
                }
            }
         
        }

        private string GetIconName()
        {
            switch (m_ItemTipsType)
            {
                case CollectedObjType.WuWood:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.WuWood);
                case CollectedObjType.SilverWood:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.SilverWood);
                case CollectedObjType.QingRock:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.QingRock);
                case CollectedObjType.CloudRock:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.CloudRock);
                case CollectedObjType.Vine:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.Vine);
                case CollectedObjType.Iron:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.Iron);
                case CollectedObjType.Ganoderma:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.Ganoderma);
                case CollectedObjType.RoyalJelly:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.Honey);
                case CollectedObjType.LotusRoot:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.LotusRoot);
                case CollectedObjType.Lotus:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.Lotus);
                case CollectedObjType.LotusLeaf:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.LotusLeaf);
                case CollectedObjType.Fish:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.Fish);      
                case CollectedObjType.Well:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.WellWater);  
                case CollectedObjType.Chicken:
                    return TDItemConfigTable.GetIconName((int)RawMaterial.Chicken);
            }
            Log.e("Type is not find "+ m_ItemTipsType.ToString());
            return TDItemConfigTable.GetIconName((int)RawMaterial.WuWood);
        }
        private int GetRawMaterial()
        {
            switch (m_ItemTipsType)
            {
                case CollectedObjType.WuWood:
                    return (int)RawMaterial.WuWood;
                case CollectedObjType.SilverWood:
                    return (int)RawMaterial.SilverWood;
                case CollectedObjType.QingRock:
                    return (int)RawMaterial.QingRock;
                case CollectedObjType.CloudRock:
                    return (int)RawMaterial.CloudRock;
                case CollectedObjType.Vine:
                    return (int)RawMaterial.Vine;
                case CollectedObjType.Iron:
                    return (int)RawMaterial.Iron;
                case CollectedObjType.Ganoderma:
                    return (int)RawMaterial.Ganoderma;
                case CollectedObjType.RoyalJelly:
                    return (int)RawMaterial.Honey;
                case CollectedObjType.LotusRoot:
                    return (int)RawMaterial.LotusRoot;
                case CollectedObjType.Lotus:
                    return (int)RawMaterial.Lotus;
                case CollectedObjType.LotusLeaf:
                    return (int)RawMaterial.LotusLeaf;
            }
            Log.e("Type is not find " + m_ItemTipsType.ToString());
            return 1016;
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