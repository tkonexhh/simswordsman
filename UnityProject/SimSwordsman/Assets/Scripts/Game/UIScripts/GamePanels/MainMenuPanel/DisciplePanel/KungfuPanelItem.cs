using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum KungfuLockState
    {
        /// <summary>
        /// 已学习
        /// </summary>
        Learned,
        /// <summary>
        /// 未学习
        /// </summary>
        NotLearning,
        /// <summary>
        /// 未解锁
        /// </summary>
        NotUnlocked,
    }
    public class KungfuPanelItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Image m_NotLearnBg;
        [SerializeField]
        private Image m_SelectBg;
        [SerializeField]
        private Image m_LearnBg;
        [SerializeField]
        private Text m_KungfuName;
        [SerializeField]
        private Button m_KungfuBtn;
        [SerializeField]
        private Text m_ClassValue;
        [SerializeField]
        private Text m_KungfuAddition;
        [SerializeField]
        private Text m_RestrictionsValue;

        private int m_CurIndex;
        private CharacterKongfu m_CharacterKongfu = null;
        private CharacterItem m_CurDisciple = null;
        private KungfuLockState m_KungfuLockState;
        private List<Sprite> m_KungfuSprite = new List<Sprite>();
        private int m_UnLockLevel = -1;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_KungfuLockState = (KungfuLockState)obj[0];
            m_CharacterKongfu = t as CharacterKongfu;
            m_KungfuSprite.AddRange((List<Sprite>)obj[1]);
            m_CurDisciple = (CharacterItem)obj[3];
            m_UnLockLevel = (int)obj[2];
            m_CurIndex = (int)obj[4];
            m_KungfuBtn.onClick.AddListener(() => {
                UIMgr.S.OpenPanel(UIID.LearnKungfuPanel, m_CurDisciple, m_CurIndex);
            });
            RefreshPanelInfo();
        }

        /// <summary>
        /// 刷新学习的功夫
        /// </summary>
        /// <param name="index"></param>
        public void RefeshKungfuInfo(CharacterKongfuData item, List<Sprite> sprite)
        {
            m_CharacterKongfu = item.CharacterKongfu;
            m_KungfuLockState = item.KungfuLockState;
            m_KungfuSprite.AddRange(sprite);
            RefreshPanelInfo();
        }
        private string GetIconName(KongfuType kungfuType)
        {
            return TDKongfuConfigTable.GetIconName(kungfuType);
        }
        private KungfuQuality GetKungfuQuality(KongfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }
        public void RefreshPanelInfo()
        {
            switch (m_KungfuLockState)
            {
                case KungfuLockState.Learned:
                    m_KungfuBtn.enabled = false;
                    m_KungfuName.text = m_CharacterKongfu.name;
                    m_ClassValue.text = GetKungfuClass(m_CharacterKongfu.dbData.level);
                    m_KungfuAddition.text = GetKungfuAddition(m_CharacterKongfu.atkScale);
                    m_RestrictionsValue.text = Define.COMMON_DEFAULT_STR;
                    m_LearnBg.gameObject.SetActive(true);
                    m_SelectBg.gameObject.SetActive(true);
                    m_NotLearnBg.gameObject.SetActive(false);
                    switch (GetKungfuQuality(m_CharacterKongfu.GetKongfuType()))
                    {
                        case KungfuQuality.Normal:
                            m_LearnBg.sprite = GetSprite("Introduction");
                            break;
                        case KungfuQuality.Super:
                            m_LearnBg.sprite = GetSprite("Advanced");
                            break;
                        case KungfuQuality.Master:
                            m_LearnBg.sprite = GetSprite("Excellent");
                            break;
                        default:
                            break;
                    }
                    m_SelectBg.sprite = GetSprite(GetIconName(m_CharacterKongfu.GetKongfuType()));

                    break;
                case KungfuLockState.NotLearning:
                    m_KungfuBtn.enabled = true;
                    m_KungfuName.text = CommonUIMethod.GetStringForTableKey(Define.KUNGFU_STATE_NOTLEARNED);
                    m_ClassValue.text = Define.COMMON_DEFAULT_STR;
                    m_KungfuAddition.text = Define.COMMON_DEFAULT_STR;
                    m_RestrictionsValue.text = CommonUIMethod.GetStringForTableKey(Define.KUNGFU_STATE_LEARNABLE);
                    m_NotLearnBg.sprite = GetSprite("NotStudy");
                    m_LearnBg.gameObject.SetActive(false);
                    m_SelectBg.gameObject.SetActive(false);
                    m_NotLearnBg.gameObject.SetActive(true);
                    break;
                case KungfuLockState.NotUnlocked:
                    m_KungfuBtn.enabled = false;
                    m_KungfuName.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTUNLOCKED);
                    m_ClassValue.text = Define.COMMON_DEFAULT_STR;
                    m_KungfuAddition.text = Define.COMMON_DEFAULT_STR;
                    m_RestrictionsValue.text = GetKungfuUnLock(m_UnLockLevel);
                    m_NotLearnBg.sprite = GetSprite("Lock1");
                    m_SelectBg.gameObject.SetActive(false);
                    m_LearnBg.gameObject.SetActive(false);
                    m_NotLearnBg.gameObject.SetActive(true);

                    break;
                default:
                    break;
            }
        }
        private Sprite GetSprite(string name)
        {
            return m_KungfuSprite.Where(i => i.name.Equals(name)).FirstOrDefault();
        }

        private string GetKungfuUnLock(int level)
        {
            if (m_UnLockLevel!=-1)
            {
                return CommonUIMethod.GetStrForColor("#8C343C", m_UnLockLevel.ToString()) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_GRADE) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNLOCKED);
            }
            return string.Empty;
        }

        private string GetKungfuClass(int level)
        {
            return CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_ONLY) + CommonUIMethod.GetTextNumber(level) + CommonUIMethod.GetStringForTableKey(Define.COMMON_UNIT_LAYER);
        }

        private string GetKungfuAddition(float atkScale)
        {
            return CommonUIMethod.GetStringForTableKey(Define.KUNGFU_TITLE) + CommonUIMethod.GetStrForColor("#8C343C", CommonUIMethod.GetBonus(atkScale));
        }
        

        public void SetButtonEvent(Action<object> action)
        {
        }
    }

}