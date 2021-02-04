using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class SendSelectedDisciple : MonoBehaviour
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
        private GameObject m_SelectedImg;
        private CharacterItem m_CharacterItem;
        private PanelType m_PanelType;
		private SelectedState m_SelelctedState = SelectedState.NotSelected;
		public void OnInit(PanelType panelType)
		{
			m_PanelType = panelType;
			BindAddListenerEvent();
			RefreshPanelTypeInfo();
		}

        private void BindAddListenerEvent()
        {
            m_Btn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                switch (m_PanelType)
                {
                    case PanelType.Task:
                        break;
                    case PanelType.Challenge:
                        EventSystem.S.Send(EventID.OnSendDiscipleDicEvent);
                        break;
                    default:
                        break;
                }
            });
		}
        public void RefreshPanelInfo()
        {
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_DiscipleName.text = m_CharacterItem.name;
                    m_DiscipleHead.gameObject.SetActive(true);
                    m_LevelBg.gameObject.SetActive(true);
                    m_Plus.gameObject.SetActive(false);
                    m_Level.text = CommonUIMethod.GetGrade(m_CharacterItem.level);
                    break;
                case SelectedState.NotSelected:
                    if (m_PanelType == PanelType.Task)
                    {
                        DestroyImmediate(gameObject);
                        break;
                    }
                    m_DiscipleName.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_NOTARRANGED);
                    m_Plus.gameObject.SetActive(true);
                    m_DiscipleHead.gameObject.SetActive(false);
                    m_LevelBg.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public void RefreshSelectedDisciple(CharacterItem characterItem)
        {
            m_CharacterItem = characterItem;
            if (m_CharacterItem == null)
            {
                m_SelelctedState = SelectedState.NotSelected;
            }
            else
            {
                LoadClanPrefabs(GetLoadDiscipleName(m_CharacterItem));
                m_SelelctedState = SelectedState.Selected;
            }
            RefreshPanelInfo();
        }
        public void LoadClanPrefabs(string prefabsName)
        {
            AddressableAssetLoader<Sprite> loader = new AddressableAssetLoader<Sprite>();
            loader.LoadAssetAsync(prefabsName, (obj) =>
            {
                //Debug.Log(obj);
                m_DiscipleHead.sprite = obj;
            });
        }
        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }
        private void RefreshPanelTypeInfo()
		{
            switch (m_PanelType)
            {
                case PanelType.Task:
                    m_Btn.enabled = false;
                    break;
                case PanelType.Challenge:
                    RefreshPanelInfo();
                    break;
                default:
                    break;
            }
        }
	}
	
}