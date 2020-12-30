using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class DisciplePanel : AbstractAnimPanel
    {
        [SerializeField]
        private Text m_DiscipleTitle;
        [SerializeField]
        private Text m_DiscipleCont;

        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Transform m_DiscipleContList;

        [SerializeField]
        private GameObject m_DiscipleItem;

        private List<CharacterItem> m_CharacterList = null;

        private Dictionary<CharacterItem, GameObject> m_DiscipleDic = new Dictionary<CharacterItem, GameObject>();

        protected override void OnUIInit()
        {
            base.OnUIInit();

            GetInformationForNeed();

            InitPanelInfo();

            BindAddListenerEvevnt();
        }
        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            RegisterEvents();
        }
        protected override void OnClose()
        {
            base.OnClose();
            UnregisterEvents();
        }

        /// <summary>
        /// 创建弟子
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="action"></param>
        /// <param name="characterItem"></param>
        private void CreateDisciple(Transform parent, CharacterItem characterItem)
        {
            if (m_DiscipleItem == null)
                return;
            GameObject disciple =  Instantiate(m_DiscipleItem, parent);
            ItemICom discipleItem = disciple.GetComponent<ItemICom>();

            m_DiscipleDic.Add(characterItem, disciple);
            discipleItem.OnInit(characterItem);
            discipleItem.SetButtonEvent(AddListenerBtn);
        }

        /// <summary>
        /// 弟子按钮Btn回调
        /// </summary>
        /// <param name="obj"></param>
        private void AddListenerBtn(object obj)
        {
            UIMgr.S.OpenPanel(UIID.DicipleDetailsPanel, obj);
        }

        private void InitPanelInfo()
        {
            m_DiscipleTitle.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_NAME);
            m_DiscipleCont.text = CommonUIMethod.GetStringForTableKey(Define.DISCIPLE_DESCRIBE);

            if (m_CharacterList != null)
                foreach (var item in m_CharacterList)
                    CreateDisciple(m_DiscipleContList, item);
        }

        private void GetInformationForNeed()
        {
            m_CharacterList = MainGameMgr.S.CharacterMgr.GetAllCharacterList();
        }

        private void BindAddListenerEvevnt()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
        }


        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnAddCoinNum, HandleEvent);
            EventSystem.S.Register(EventID.OnDiscipleReduce, HandleEvent);
        }

        private void UnregisterEvents()
        {
            EventSystem.S.UnRegister(EventID.OnAddCoinNum, HandleEvent);
            EventSystem.S.UnRegister(EventID.OnDiscipleReduce, HandleEvent);
        }

        private void HandleEvent(int key, params object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnAddCoinNum:
                    //RefreshPanelInfo();
                    break;
                case (int)EventID.OnDiscipleReduce:
                    int id = (int)param[0];
                    foreach (var item in m_DiscipleDic.Keys)
                    {
                        if (item.id==id)
                        {
                            GameObject obj = m_DiscipleDic[item];
                            m_DiscipleDic.Remove(item);
                            DestroyImmediate(obj);
                        }
                    }
                    break;
            }
        }
    }
}