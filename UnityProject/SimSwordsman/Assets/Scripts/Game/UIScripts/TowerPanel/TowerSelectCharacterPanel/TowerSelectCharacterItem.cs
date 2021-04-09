using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class TowerSelectCharacterItem : IUListItemView
    {
        [SerializeField] private Text m_TxtName;
        [SerializeField] private Text m_TxtLvl;
        [SerializeField] private Image m_ImgHead;
        [SerializeField] private Button m_BtnBg;


        private CharacterItem m_CharacterItem;

        private void Awake()
        {
            m_BtnBg.onClick.AddListener(OnClickBg);
        }

        public void SetCharacter(CharacterItem character)
        {
            m_CharacterItem = character;
            RefeshUI();
        }

        private void RefeshUI()
        {
            m_TxtLvl.text = m_CharacterItem.level.ToString();
            m_TxtName.text = m_CharacterItem.name;
            m_ImgHead.sprite = SpriteHandler.S.GetSprite("CharacterHeadIconsAtlas", GetLoadDiscipleName(m_CharacterItem));
            m_ImgHead.SetNativeSize();
        }

        private string GetLoadDiscipleName(CharacterItem characterItem)
        {
            return "head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId + "_" + characterItem.headId;
        }

        private void OnClickBg()
        {

        }
    }

}