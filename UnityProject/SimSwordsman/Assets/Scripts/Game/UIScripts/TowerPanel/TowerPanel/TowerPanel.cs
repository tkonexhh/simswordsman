using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{
    public class TowerPanel : AbstractFullPanel
    {
        [SerializeField] private Button m_BtnShop;
        [SerializeField] private Button m_BtnBack;
        [SerializeField] private Button m_BtnRule;
        [SerializeField] private Text m_TxtCoin;

        [SerializeField] private IUListView m_ListView;

        //需要两种材质
        private Material m_GreyMat;

        public Material greyMat => m_GreyMat;
        // private Material m_NormalMat;



        protected override void OnUIInit()
        {
            m_BtnBack.onClick.AddListener(CloseSelfPanel);
            m_BtnShop.onClick.AddListener(OnClickShop);
            m_BtnRule.onClick.AddListener(OnClickRule);

            m_ListView.SetCellRenderer(OnCellRenderer);
            m_GreyMat = new Material(Shader.Find("XHH/UI/GreyUI"));
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            RegisterEvent(EventID.OnRefeshTowerCoin, (i, e) => { UpdateCoin(); });
            UpdateUI();
        }

        protected override void OnClose()
        {
            base.OnClose();
        }


        private void OnClickShop()
        {
            UIMgr.S.OpenPanel(UIID.TowerShopPanel);
        }

        private void OnClickRule()
        {
            UIMgr.S.OpenPanel(UIID.TowerRulePanel);
        }

        private void OnCellRenderer(Transform root, int index)
        {
            root.GetComponent<TowerPanelChallengeItem>().Init(this, TowerDefine.MAXLEVEL - index);
        }

        private void UpdateUI()
        {
            m_ListView.SetDataCount(TowerDefine.MAXLEVEL);
            UpdateCoin();
        }

        private void UpdateCoin()
        {
            m_TxtCoin.text = GameDataMgr.S.GetPlayerData().towerData.coin.ToString();
        }
    }

}