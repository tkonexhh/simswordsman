using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class SignInPanel : AbstractAnimPanel
	{
        [SerializeField]
        private Text m_SignInTitle;
        [SerializeField]
        private Text SigiInCont;


        [SerializeField]
        private Button m_CloseBtn;
        [SerializeField]
        private Button m_SignInBtn;

        [SerializeField]
        private Transform SignInTrans;

        [SerializeField]
        private GameObject m_SignInItem;


        private List<SignInItem> signInItemList = null;
        // Start is called before the first frame update
        protected override void OnUIInit()
        {
            base.OnUIInit();

            IninPanelInfo();

            BindAddListenerEvent();
        }

        private void IninPanelInfo()
        {
            SignInItem[] signInItems =  SignInTrans.GetComponentsInChildren<SignInItem>();
            signInItemList = new List<SignInItem>(signInItems);
        }

        private void BindAddListenerEvent()
        {
            m_CloseBtn.onClick.AddListener(HideSelfWithAnim);

            m_SignInBtn.onClick.AddListener(()=>{
                if (signInItemList!=null)
                {
                    foreach (var item in signInItemList)
                    {
                        if (!item.isSignInState)
                        {
                            item.SetSignInState();
                            return;
                        }
                    }
                }
            });
        }

        protected override void OnPanelHideComplete()
        {
            base.OnPanelHideComplete();
            CloseSelfPanel();
        }
    }
	
}