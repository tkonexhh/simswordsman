using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Qarth;

namespace GameWish.Game
{
	public class CharacterWorkRewardPop : MonoBehaviour
	{
        [SerializeField]
        private Text m_RewardNumText = null;
        [SerializeField]
        private Image m_RewardIcon = null;

        //[SerializeField]
        //private GameObject m_BaicohuIcon = null;
        //[SerializeField]
        //private GameObject m_FishIcon = null;
        //[SerializeField]
        //private GameObject m_FlowerIcon = null;
        //[SerializeField]
        //private GameObject m_ForgeHouseIcon = null;
        //[SerializeField]
        //private GameObject m_KitchenIcon = null;
        //[SerializeField]
        //private GameObject m_LobbyIcon = null;
        //[SerializeField]
        //private GameObject m_RockIcon = null;
        //[SerializeField]
        //private GameObject m_TreeIcon = null;
        //[SerializeField]
        //private GameObject m_WareHouseIcon = null;
        //[SerializeField]
        //private GameObject m_WellIcon = null;

        private float m_ExistTime = 1f;

        private ResLoader m_ResLoader = null;

        private string m_IconResName;

        private void Awake()
        {
            HidelAll();

            if (m_ResLoader == null)
            {
                m_ResLoader = ResLoader.Allocate("WorkRewardPopResLoader");
            }
        }

        private void OnDisable()
        {
            m_ResLoader?.ReleaseRes(m_IconResName);
        }

        private void HidelAll()
        {
            //m_BaicohuIcon.SetActive(false);
            //m_BaicohuIcon.SetActive(false);
            //m_FishIcon.SetActive(false);
            //m_FlowerIcon.SetActive(false);
            //m_ForgeHouseIcon.SetActive(false);
            //m_KitchenIcon.SetActive(false);
            //m_LobbyIcon.SetActive(false);
            //m_RockIcon.SetActive(false);
            //m_TreeIcon.SetActive(false);
            //m_WareHouseIcon.SetActive(false);
            //m_WellIcon.SetActive(false);
        }

        public void OnGetFacilityWorkReward(FacilityType facilityType, int count)
        {
            m_IconResName = "bubble_coin";
            //GameObject go = GetIconByFacilityType(facilityType);
            //if (go != null)
            //{
            //    go.SetActive(true);
            //}
            Sprite sprite = SpriteLoader.S.GetSpriteByName(m_IconResName);
            m_RewardIcon.sprite = sprite;
            m_RewardNumText.text = "+" + count.ToString();

            StartCoroutine(AutoDestroyCor());
        }

        public void OnGetCollectObjWorkReward(RawMaterial collectedObjType, int count)
        {
            m_IconResName = TDItemConfigTable.GetIconName((int)collectedObjType);
            //GameObject go = GetIconByCollectedObjType( CollectedObjType.Iron);
            //if (go != null)
            //{
            //    go.SetActive(true);
            //}
            Sprite sprite = SpriteLoader.S.GetSpriteByName(m_IconResName);
            m_RewardIcon.sprite = sprite;
            m_RewardNumText.text = "+" + count.ToString();

            StartCoroutine(AutoDestroyCor());
        }

        //private GameObject GetIconByFacilityType(FacilityType facilityType)
        //{
        //    GameObject icon = null;

        //    switch (facilityType)
        //    {
        //        case FacilityType.Baicaohu:
        //            icon = m_BaicohuIcon;
        //            break;
        //        case FacilityType.ForgeHouse:
        //            icon = m_ForgeHouseIcon;
        //            break;
        //        case FacilityType.Kitchen:
        //            icon = m_KitchenIcon;
        //            break;
        //        case FacilityType.Lobby:
        //            icon = m_LobbyIcon;
        //            break;
        //        case FacilityType.Warehouse:
        //            icon = m_WareHouseIcon;
        //            break;
        //        default:
        //            icon = m_LobbyIcon;
        //            break;
        //    }

        //    return icon;
        //}

        //private GameObject GetIconByCollectedObjType(CollectedObjType collectedObjType)
        //{
        //    GameObject icon = null;

        //    switch (collectedObjType)
        //    {
        //        case CollectedObjType.Fish:
        //            icon = m_FishIcon;
        //            break;
        //        case CollectedObjType.WuWood:
        //        case CollectedObjType.SilverWood:
        //            icon = m_TreeIcon;
        //            break;
        //        case CollectedObjType.CloudRock:
        //        case CollectedObjType.QingRock:
        //        case CollectedObjType.Iron:
        //            icon = m_RockIcon;
        //            break;
        //        case CollectedObjType.Vine:
        //        case CollectedObjType.Ganoderma:
        //            icon = m_FlowerIcon;
        //            break;
        //        default:
        //            icon = m_LobbyIcon;
        //            break;
        //    }

        //    return icon;
        //}

        WaitForSeconds m_Wait = null;

        private IEnumerator AutoDestroyCor()
        {
            if (m_Wait == null)
            {
                m_Wait = new WaitForSeconds(m_ExistTime);
            }

            transform.DOMoveY(2, m_ExistTime).SetRelative();

            yield return m_Wait;

            Qarth.GameObjectPoolMgr.S.Recycle(gameObject);
        }
    }
	
}