using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class BulletinBoardDisciple : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Button m_Btn;
        [SerializeField]
        private Image m_LevelBg;
        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Image m_DiscipleHead;
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Image m_Plus;
        [SerializeField]
        private Image m_DiscipleLevelBg;
        [SerializeField]
        private Image m_Line;
        private CharacterItem m_CharacterItem;
        private SimGameTask m_CurTaskInfo;
        private CommonTaskItemInfo m_CommonTaskItemInfo;
        private AddressableAssetLoader<Sprite> m_Loader;
        private BulletinBoardPanel m_BulletinBoardPanel;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_CurTaskInfo = t as SimGameTask;
            m_BulletinBoardPanel = (BulletinBoardPanel)obj[0];
            m_CommonTaskItemInfo = m_CurTaskInfo.CommonTaskItemInfo;
            m_Btn.onClick.AddListener(() => {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                EventSystem.S.Send(EventID.OnBulletinSendDiscipleDicEvent, m_CurTaskInfo);
            });
        }
        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }
        public void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_DiscipleName.text = m_CharacterItem.name;
                    m_DiscipleHead.sprite = m_BulletinBoardPanel.FindSprite(GetLoadDiscipleName(m_CharacterItem));
                    m_DiscipleHead.gameObject.SetActive(true);
                    m_LevelBg.gameObject.SetActive(true);
                    m_Plus.gameObject.SetActive(false);
                    m_Level.text = m_CharacterItem.level.ToString();
                    RefreshDiscipleColor();
                    break;
                case SelectedState.NotSelected:
                    m_DiscipleName.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NOTARRANGED);
                    m_Plus.gameObject.SetActive(true);
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_LevelBg.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        private void RefreshDiscipleColor()
        {
            switch (m_CharacterItem.quality)
            {
                case CharacterQuality.Normal:
                    m_DiscipleLevelBg.sprite = m_BulletinBoardPanel.FindSprite("Disciple_FontBg_Blue");
                    m_Line.sprite = m_BulletinBoardPanel.FindSprite("Disciple_Line_Bule");
                    break;
                case CharacterQuality.Good:
                    m_DiscipleLevelBg.sprite = m_BulletinBoardPanel.FindSprite("Disciple_FontBg_Yellow");
                    m_Line.sprite = m_BulletinBoardPanel.FindSprite("Disciple_Line_Yellow");
                    break;
                case CharacterQuality.Perfect:
                    m_DiscipleLevelBg.sprite = m_BulletinBoardPanel.FindSprite("Disciple_FontBg_Red");
                    m_Line.sprite = m_BulletinBoardPanel.FindSprite("Disciple_Line_Red");
                    break;
                default:
                    break;
            }
        }

        public void LoadClanPrefabs(string prefabsName)
        {
            //m_DiscipleHead.sprite = obj;
            //m_Loader = new AddressableAssetLoader<Sprite>();
            //m_Loader.LoadAssetAsync(prefabsName, (obj) =>
            //{
            //    //Debug.Log(obj);
            //    m_DiscipleHead.sprite = obj;
            //});
        }
        public void SetBtnClick(bool IsClick)
        {
            m_Btn.enabled = IsClick;
        }
        private void OnDestroy()
        {
            if (m_Loader != null)
            {
                m_Loader.Release();
            }

        }
        private void OnDisable()
        {
        }

        public void RefreshSelectedDisciple(CharacterItem characterItem)
        {
            m_CharacterItem = characterItem;
            if (m_CharacterItem == null)
                m_SelelctedState = SelectedState.NotSelected;
            else
            {
                LoadClanPrefabs(GetLoadDiscipleName(m_CharacterItem));
                m_SelelctedState = SelectedState.Selected;
            }
            RefreshPanelInfo();
        }
        public void SetButtonEvent(Action<object> action)
        {
        }
	}
}