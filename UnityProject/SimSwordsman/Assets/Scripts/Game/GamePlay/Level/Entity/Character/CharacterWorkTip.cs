using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class CharacterWorkTip : MonoBehaviour
	{
        [SerializeField]
        private GameObject m_BaicohuIcon = null;
        [SerializeField]
        private GameObject m_FishIcon = null;
        [SerializeField]
        private GameObject m_FlowerIcon = null;
        [SerializeField]
        private GameObject m_ForgeHouseIcon = null;
        [SerializeField]
        private GameObject m_KitchenIcon = null;
        [SerializeField]
        private GameObject m_LobbyIcon = null;
        [SerializeField]
        private GameObject m_RockIcon = null;
        [SerializeField]
        private GameObject m_TreeIcon = null;
        [SerializeField]
        private GameObject m_WareHouseIcon = null;
        [SerializeField]
        private GameObject m_WellIcon = null;

        private void OnEnable()
        {
            HidelAll();
        }

        private void HidelAll()
        {
            m_BaicohuIcon.SetActive(false);
            m_BaicohuIcon.SetActive(false);
            m_FishIcon.SetActive(false);
            m_FlowerIcon.SetActive(false);
            m_ForgeHouseIcon.SetActive(false);
            m_KitchenIcon.SetActive(false);
            m_LobbyIcon.SetActive(false);
            m_RockIcon.SetActive(false);
            m_TreeIcon.SetActive(false);
            m_WareHouseIcon.SetActive(false);
            m_WellIcon.SetActive(false);
        }

        private void OnDisable()
        {
            HidelAll();
        }

        public void OnGotoFacilityWork(FacilityType facilityType)
        {
            GameObject go = GetIconByFacilityType(facilityType);
            if (go != null)
            {
                go.SetActive(true);
            }
        }

        public void OnGotoCollectObj(CollectedObjType collectedObjType)
        {
            GameObject go = GetIconByCollectedObjType(collectedObjType);
            if (go != null)
            {
                go.SetActive(true);
            }
        }

        private GameObject GetIconByFacilityType(FacilityType facilityType)
        {
            GameObject icon = null;

            switch (facilityType)
            {
                case FacilityType.Baicaohu:
                    icon = m_BaicohuIcon;
                    break;
                case FacilityType.ForgeHouse:
                    icon = m_ForgeHouseIcon;
                    break;
                case FacilityType.Kitchen:
                    icon = m_KitchenIcon;
                    break;
                case FacilityType.Lobby:
                    icon = m_LobbyIcon;
                    break;
                case FacilityType.Warehouse:
                    icon = m_WareHouseIcon;
                    break;
                default:
                    icon = m_LobbyIcon;
                    break;
            }

            return icon;
        }

        private GameObject GetIconByCollectedObjType(CollectedObjType collectedObjType)
        {
            GameObject icon = null;

            switch (collectedObjType)
            {
                case CollectedObjType.Fish:
                    icon = m_FishIcon;
                    break;
                case CollectedObjType.WuWood:
                case CollectedObjType.SilverWood:
                    icon = m_TreeIcon;
                    break;
                case CollectedObjType.CloudRock:
                case CollectedObjType.QingRock:
                case CollectedObjType.Iron:
                    icon = m_RockIcon;
                    break;
                case CollectedObjType.Vine:
                case CollectedObjType.Ganoderma:
                    icon = m_FlowerIcon;
                    break;
                default:
                    icon = m_LobbyIcon;
                    break;
            }

            return icon;
        }
    }
	
}