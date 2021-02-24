using Qarth;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace GameWish.Game
{
   
    public class SectNamePanel : AbstractAnimPanel
	{
	    [SerializeField]
	    private InputField m_ClanName; 
		[SerializeField]
		private Button m_RandomBtn;
		[SerializeField]
		private Button m_AcceptBtn;

        [SerializeField]
        private Button m_CloseBtn;

        Action m_CloseAction;

        protected override void OnUIInit()
	    {
            base.OnUIInit();
           
			BindAddListenerEvent();
        }

        protected override void OnPanelOpen(params object[] args)
        {
            base.OnPanelOpen(args);

            if (args.Length > 0)
            {
                m_CloseAction = (Action)args[0];
                m_CloseBtn.gameObject.SetActive(false);
                RandomName();
            }
            else
            {
                m_CloseBtn.gameObject.SetActive(true);
                m_ClanName.text = GameDataMgr.S.GetGameData().clanData.GetClanName();
            }
            OpenDependPanel(EngineUI.MaskPanel,-1,null);
        }

        /// <summary>
        /// 检测敏感词汇
        /// </summary>
        bool IllegalCheckName(string value)
        {
            FilterSensitiveWords.Initialize();
            string out1;
            string s = value;
            return FilterSensitiveWords.IsContainSensitiveWords(ref s, out out1);
        }

        void RandomName()
        {
            int index1 = RandomHelper.Range(0, TDSectNameTable.m_FamilyNameList.Count);
            string s1 = TDSectNameTable.m_FamilyNameList[index1];

            int index2 = RandomHelper.Range(0, TDSectNameTable.m_FirstList.Count);
            string s2 = TDSectNameTable.m_FirstList[index2];

            m_ClanName.text = s1 + s2;
        }
        
        private void BindAddListenerEvent()
        {
            m_RandomBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                RandomName();
            });
            m_AcceptBtn.onClick.AddListener(()=> 
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                if (IllegalCheckName(m_ClanName.text))
                {
                    FloatMessage.S.ShowMsg("名称不合法");
                    //UIMgr.S.OpenTopPanel(UIID.LogPanel, null, "提示", "名称不合法！");
                }
                else if(m_ClanName.text.Length > 8)
                {
                    FloatMessage.S.ShowMsg("超过字数限制");
                    //UIMgr.S.OpenTopPanel(UIID.LogPanel, null, "提示", "超过字数限制！");
                }
                else
                {
                    GameDataMgr.S.GetGameData().clanData.SetClanName(m_ClanName.text);
                    m_CloseAction?.Invoke();
                    HideSelfWithAnim();
                }
            });
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}