using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class KungfuLibraryDisciple : MonoBehaviour
    {
        [SerializeField]
        private Text m_Level;
        [SerializeField]
        private Text m_DiscipleName;
        [SerializeField]
        private Image m_DiscipleHead;
        [SerializeField]
        private Image m_State;
        [SerializeField]
        private Button m_Btn;
        [SerializeField]
        private Transform m_Pos;
        private AddressableAssetLoader<Sprite> m_Loader;
        private SelectedState m_SelelctedState = SelectedState.NotSelected;
        private CharacterItem m_CharacterItem;
        private KungfuChooseDisciplePanel m_KungfuChooseDisciplePanel;
        private bool isSelected = false;
        public void OnInit(CharacterItem characterItem, KungfuChooseDisciplePanel kungfuChooseDisciplePanel)
        {
            m_CharacterItem = characterItem;
            m_KungfuChooseDisciplePanel = kungfuChooseDisciplePanel;
            m_Btn.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                isSelected = !isSelected;
                if (isSelected)
                    m_SelelctedState = SelectedState.Selected;
                else
                    m_SelelctedState = SelectedState.NotSelected;
                RefreshPanelInfo();
                EventSystem.S.Send(EventID.OnSelectedEvent, isSelected, m_CharacterItem, m_Pos);
            });
            m_DiscipleHead.sprite = m_KungfuChooseDisciplePanel.FindSprite(GetLoadDiscipleName(m_CharacterItem));
            RefreshPanelInfo();
        }
        public void LoadClanPrefabs(string prefabsName)
        {
            m_Loader = new AddressableAssetLoader<Sprite>();
            m_Loader.LoadAssetAsync(prefabsName, (obj) =>
            {
                //Debug.Log(obj);
                m_DiscipleHead.sprite = obj;
            });
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
        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }
        private void RefreshPanelInfo()
        {
            m_Level.text = CommonUIMethod.GetGrade(m_CharacterItem.level);
            m_DiscipleName.text = m_CharacterItem.name;
            switch (m_SelelctedState)
            {
                case SelectedState.Selected:
                    m_State.gameObject.SetActive(true);
                    break;
                case SelectedState.NotSelected:
                    m_State.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public void IsSame(CharacterItem characterItem)
        {
            if (characterItem.id != m_CharacterItem.id)
            {
                m_SelelctedState = SelectedState.NotSelected;
                isSelected = false;
                RefreshPanelInfo();
            }
        }
        // Start is called before the first frame update
    }

}