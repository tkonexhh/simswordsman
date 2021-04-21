using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum DeliverDiscipleState
    {
        None,
        Lock,
        Free,
        Someone
    }

    public class DeliverDisciple : MonoBehaviour
    {
        [SerializeField]
        private Button m_Btn;
        [SerializeField]
        private GameObject m_Plus;
        [SerializeField]
        private Image m_Qulity;
        [SerializeField]
        private GameObject m_UnLock;

        private GameObject m_DiscipleHeadObj;

        private CharacterItem m_CharacterItem;
        private SingleDeliverDetailData m_SingleDeliverDetailData;
        private DeliverDiscipleState deliverDiscipleState1 = DeliverDiscipleState.Lock;
        void Start()
        {
            m_Btn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                EventSystem.S.Send(EventID.OnOpenChallengChoosePanel, m_SingleDeliverDetailData.DeliverID);
            });
        }

        public void SetBtnState(bool state)
        {
            m_Btn.enabled = state;
        }

        public void ResetSelf()
        {
            deliverDiscipleState1 = DeliverDiscipleState.Free;
            RefreshDeliverDisciple();
        }

        public void OnInit(SingleDeliverDetailData singleDeliverDetailData)
        {
            m_SingleDeliverDetailData = singleDeliverDetailData;
            deliverDiscipleState1 = DeliverDiscipleState.Lock;
            RefreshDeliverDisciple();
        }

        public void OnFillDisciple(CharacterItem characterItem)
        {
            deliverDiscipleState1 = DeliverDiscipleState.Someone;
            m_CharacterItem = characterItem;
            RefreshDeliverDisciple();
        }

        public void SetDeliverDiscipleState(int characterID = -1)
        {
            if (characterID == -1)
            {
                deliverDiscipleState1 = DeliverDiscipleState.Free;
            }
            else
            {
                deliverDiscipleState1 = DeliverDiscipleState.Someone;
                m_CharacterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(characterID);
            }
            RefreshDeliverDisciple();
        }

        public void SetDeliverDiscipleStateFree()
        {
            deliverDiscipleState1 = DeliverDiscipleState.Free;
            RefreshDeliverDisciple();
        }
        private void RefreshDeliverDisciple()
        {
            switch (deliverDiscipleState1)
            {
                case DeliverDiscipleState.Lock:
                    m_UnLock.SetActive(true);
                    m_Plus.SetActive(false);
                    break;
                case DeliverDiscipleState.Free:
                    m_Plus.SetActive(true);
                    m_UnLock.SetActive(false);
                    if (m_DiscipleHeadObj != null)
                        m_DiscipleHeadObj.gameObject.SetActive(false);
                    m_Qulity.gameObject.SetActive(false);
                    break;
                case DeliverDiscipleState.Someone:
                    if (m_DiscipleHeadObj == null)
                    {
                        DiscipleHeadPortrait discipleHeadPortrait = Instantiate(DiscipleHeadPortraitMgr.S.GetDiscipleHeadPortrait(m_CharacterItem), transform).GetComponent<DiscipleHeadPortrait>();
                        discipleHeadPortrait.OnInit(true);
                        m_DiscipleHeadObj = discipleHeadPortrait.gameObject;
                        discipleHeadPortrait.transform.localPosition = new Vector3(8.8f, 0.4f, 0);
                        discipleHeadPortrait.transform.localScale = new Vector3(0.35f, 0.35f, 1);
                    }
                    //m_Head.sprite = SpriteHandler.S.GetSprite(AtlasDefine.CharacterHeadIconsAtlas,GetDiscipelHeadName());
                    SetQualitySprite();
                    if (m_DiscipleHeadObj != null)
                        m_DiscipleHeadObj.gameObject.SetActive(true);
                    m_Qulity.gameObject.SetActive(true);
                    m_Plus.SetActive(false);
                    m_UnLock.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        private void SetQualitySprite()
        {
            switch (m_CharacterItem.quality)
            {
                case CharacterQuality.Normal:
                    m_Qulity.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DeliverAtlas, "Deliver_civilian");
                    break;
                case CharacterQuality.Good:
                    m_Qulity.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DeliverAtlas, "Deliver_elite");
                    break;
                case CharacterQuality.Perfect:
                    m_Qulity.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DeliverAtlas, "Deliver_god");
                    break;
                default:
                    break;
            }
        }

        private string GetDiscipelHeadName()
        {
            return "head_" + m_CharacterItem.quality.ToString().ToLower() + "_" + m_CharacterItem.bodyId + "_" + m_CharacterItem.headId;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }

}