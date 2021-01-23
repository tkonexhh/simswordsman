using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private Image m_SecretBookBg;
        [SerializeField]
        private Text m_SecretBookNameValue;
        [SerializeField]
        private Button m_KungfuPanelItemBtn;
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
        private Sprite m_KungfuSprite;
        private int m_UnLockLevel = -1;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_KungfuLockState = (KungfuLockState)obj[0];
            m_CharacterKongfu = t as CharacterKongfu;
            m_KungfuSprite = (Sprite)obj[1];
            m_CurDisciple = (CharacterItem)obj[3];
            m_UnLockLevel = (int)obj[2];
            m_CurIndex = (int)obj[4];
            m_KungfuPanelItemBtn.onClick.AddListener(() => {
                UIMgr.S.OpenPanel(UIID.LearnKungfuPanel, m_CurDisciple, m_CurIndex);
            });
            RefreshPanelInfo();
        }

        /// <summary>
        /// 刷新学习的功夫
        /// </summary>
        /// <param name="index"></param>
        public void RefeshKungfuInfo(CharacterKongfuData item, Sprite sprite)
        {
            m_CharacterKongfu = item.CharacterKongfu;
            m_KungfuLockState = item.KungfuLockState;
            m_KungfuSprite = sprite;
            RefreshPanelInfo();
        }

        public void RefreshPanelInfo()
        {
            m_SecretBookBg.sprite = m_KungfuSprite;

            switch (m_KungfuLockState)
            {
                case KungfuLockState.Learned:
                    m_KungfuPanelItemBtn.enabled = false;
                    m_SecretBookNameValue.text = m_CharacterKongfu.name;
                    m_ClassValue.text = GetKungfuClass(m_CharacterKongfu.dbData.level);
                    m_KungfuAddition.text = GetKungfuAddition(m_CharacterKongfu.atkScale);
                    m_RestrictionsValue.text = Define.COMMON_DEFAULT_STR;
                    break;
                case KungfuLockState.NotLearning:
                    m_KungfuPanelItemBtn.enabled = true;
                    m_SecretBookNameValue.text = CommonUIMethod.GetStringForTableKey(Define.KUNGFU_STATE_NOTLEARNED);
                    m_ClassValue.text = Define.COMMON_DEFAULT_STR;
                    m_KungfuAddition.text = Define.COMMON_DEFAULT_STR;
                    m_RestrictionsValue.text = CommonUIMethod.GetStringForTableKey(Define.KUNGFU_STATE_LEARNABLE);
                    break;
                case KungfuLockState.NotUnlocked:
                    m_KungfuPanelItemBtn.enabled = false;
                    m_SecretBookNameValue.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_NOTUNLOCKED);
                    m_ClassValue.text = Define.COMMON_DEFAULT_STR;
                    m_KungfuAddition.text = Define.COMMON_DEFAULT_STR;
                    m_RestrictionsValue.text = GetKungfuUnLock(m_UnLockLevel);
                    break;
                default:
                    break;
            }
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