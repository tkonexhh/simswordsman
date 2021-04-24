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

        [SerializeField] private ScrollRect m_ScrollRect;
        [SerializeField] private TowerPanelChallengeItem[] m_LevelItems;

        //需要两种材质
        private Material m_GreyMat;

        public Material greyMat => m_GreyMat;

        protected override void OnUIInit()
        {
            m_BtnBack.onClick.AddListener(CloseSelfPanel);
            m_BtnShop.onClick.AddListener(OnClickShop);
            m_BtnRule.onClick.AddListener(OnClickRule);

            // m_ListView.SetCellRenderer(OnCellRenderer);
            m_GreyMat = new Material(Shader.Find("XHH/UI/GreyUI"));
            MainGameMgr.S.TowerSystem.EnterTower();
            for (int i = 0; i < m_LevelItems.Length; i++)
            {
                int level = TowerDefine.MAXLEVEL - i;
                m_LevelItems[i].Init(this, level);
                m_LevelItems[i].gameObject.name = "TowerLevelItem" + level;
            }

            PlayerPrefs.SetInt(Define.IsOpenTowerPanel, 1);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            RegisterEvent(EventID.OnRefeshTowerCoin, (i, e) => { UpdateCoin(); });
            UpdateUI();

            if (!DataRecord.S.GetBool(TowerDefine.SAVEKEY_NEWDAYSHOW, false))
            {
                DataRecord.S.SetBool(TowerDefine.SAVEKEY_NEWDAYSHOW, true);
                DataRecord.S.Save();
                // UIMgr.S.OpenPanel(UIID.TowerNewDayPanel);
            }

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

        private void UpdateUI()
        {
            int nowLevel = GameDataMgr.S.GetPlayerData().towerData.maxLevel;
            try
            {
                var target = m_LevelItems[nowLevel - 1].rectTransform();
                float cellHeight = target.rect.height;
                m_ScrollRect.content.anchoredPosition = new Vector2(0, cellHeight * (Mathf.Abs(TowerDefine.MAXLEVEL - nowLevel - 2)));
            }
            catch (System.IndexOutOfRangeException)
            {
                m_ScrollRect.verticalNormalizedPosition = 0;
            }

            UpdateCoin();
        }


        private void UpdateCoin()
        {
            m_TxtCoin.text = GameDataMgr.S.GetPlayerData().towerData.coin.ToString();
        }
    }

}