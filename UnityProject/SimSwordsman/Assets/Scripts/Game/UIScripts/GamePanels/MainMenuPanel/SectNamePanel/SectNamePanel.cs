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
            }
            RandomName();
            OpenDependPanel(EngineUI.MaskPanel,-1,null);
        }

        /// <summary>
        /// ºÏ≤‚√Ù∏–¥ ª„ ¥˝ÕÍ…∆
        /// </summary>
        void OnNameChange(string value)
        {
            Log.e("’‚¿Ô”¶µ±ºÏ≤‚√Ù∏–¥ ª„");
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
            m_ClanName.onValueChanged.AddListener(OnNameChange);
            m_ClanName.onEndEdit.AddListener(OnNameChange);

            m_RandomBtn.onClick.AddListener(RandomName);
            m_AcceptBtn.onClick.AddListener(()=> 
            {
                GameDataMgr.S.GetGameData().clanData.SetClanName(m_ClanName.text);
                OnNameChange(m_ClanName.text);

                m_CloseAction?.Invoke();

                HideSelfWithAnim();
            });
		}


        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
            CloseDependPanel(EngineUI.MaskPanel);
        }
    }
}