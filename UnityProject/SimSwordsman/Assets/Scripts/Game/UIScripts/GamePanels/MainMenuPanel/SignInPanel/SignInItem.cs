using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum SignPanel_SignStatus
    {
        None,
        SignEnable,//可签到
        SignDisable,//不可签到
        SignAlready//已经签到
    }
    public class SignInItem : MonoBehaviour
	{
        public delegate void SignClickDele(int id);

        public SignClickDele SignItemCallBack;//点击按钮回调

        private Image m_NormalImage;
        private Image m_GlowImage;
        private Image m_AlreadyImage;
        private Image m_IconImage;

        private Text m_CountText;
        private Text m_DayText;

        private Button m_Button;//点击按钮

        private int m_ID;

        private RewardBase m_SignConfig;

        public RewardBase RewardCfg { get => m_SignConfig; }

        public SignInItem(int id, Transform trans, RewardBase signConfig)
        {
            FindChildObject(trans);

            m_ID = id;
            m_SignConfig = signConfig;

            //m_DayText.text = (id + 1).ToString() + TDLanguageTable.Get("SignPanel_1");
            SetCount(signConfig.m_Count);

            m_Button.onClick.AddListener(() =>
            {
                SignItemCallBack?.Invoke(m_ID);
            });

        }

        /// <summary>
        /// 设置Icon的图片
        /// </summary>
        /// <param name="sprite"></param>
        public void SetIconSprite(Sprite sprite)
        {
            m_IconImage.sprite = sprite;
            //m_IconImage.SetNativeSize();
        }

        /// <summary>
        /// 查找下面的子物体
        /// </summary>
        /// <param name="trans"></param>
        private void FindChildObject(Transform trans)
        {
            m_NormalImage = trans.Find("NormalBG").GetComponent<Image>();
            m_GlowImage = trans.Find("GlowBG").GetComponent<Image>();
            m_AlreadyImage = trans.Find("AlreadyBG").GetComponent<Image>();
            m_IconImage = trans.Find("Icon").GetComponent<Image>();

            m_Button = trans.Find("Button").GetComponent<Button>();

            m_CountText = trans.Find("Count").GetComponent<Text>();
            m_DayText = trans.Find("Day").GetComponent<Text>();
        }

        /// <summary>
        /// 设置SignItem右下方奖励数字
        /// </summary>
        /// <param name="count"></param>
        public void SetCount(int count)
        {
            m_CountText.text = count.ToString();
        }

        /// <summary>
        /// 改变SignItem的显示状态
        /// </summary>
        /// <param name="status"></param>
        public void SetSignItemStatus(SignPanel_SignStatus status)
        {
            switch (status)
            {
                case SignPanel_SignStatus.None:
                    break;
                case SignPanel_SignStatus.SignEnable:
                    m_NormalImage.gameObject.SetActive(true);
                    m_GlowImage.gameObject.SetActive(true);
                    m_AlreadyImage.gameObject.SetActive(false);
                    m_DayText.gameObject.SetActive(true);
                    break;
                case SignPanel_SignStatus.SignDisable:
                    m_NormalImage.gameObject.SetActive(true);
                    m_GlowImage.gameObject.SetActive(false);
                    m_AlreadyImage.gameObject.SetActive(false);
                    m_DayText.gameObject.SetActive(true);
                    break;
                case SignPanel_SignStatus.SignAlready:
                    m_NormalImage.gameObject.SetActive(true);
                    m_GlowImage.gameObject.SetActive(false);
                    m_AlreadyImage.gameObject.SetActive(true);
                    m_DayText.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }


    }
	
}