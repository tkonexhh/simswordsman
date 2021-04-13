using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class TowerSelectCharacterPanel : AbstractPanel
    {
        [SerializeField] private Button m_BtnClose;
        [SerializeField] private Button m_BtnAutoSelect;
        [SerializeField] private Button m_BtnFight;

        [SerializeField] private IUListView m_ListView;

        private List<CharacterItem> m_AllCharacterLst;

        protected override void OnUIInit()
        {
            m_BtnClose.onClick.AddListener(CloseSelfPanel);
            m_BtnAutoSelect.onClick.AddListener(OnClickAutoSelect);
            m_BtnFight.onClick.AddListener(OnClickFight);

            m_ListView.SetCellRenderer(OnCellRenderer);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            OpenDependPanel(EngineUI.MaskPanel, -1);
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            TowerPanelChallenge conf = (TowerPanelChallenge)args[0];
            int level = conf.level;
            m_AllCharacterLst = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
            m_ListView.SetDataCount(m_AllCharacterLst.Count);
        }


        private void OnClickAutoSelect()
        {

        }

        private void OnClickFight()
        {
            CloseSelfPanel();
            MainGameMgr.S.TowerSystem.StartLevel();
        }

        private void OnCellRenderer(Transform root, int index)
        {
            root.GetComponent<TowerSelectCharacterItem>().SetCharacter(m_AllCharacterLst[index]);
        }
    }


}