using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum SignInStatus
    {
        None,
        SignEnable,//��ǩ��
        SignDisable,//����ǩ��
        SignAlready//�Ѿ�ǩ��
    }
    public class SignInItem : MonoBehaviour
	{
        public delegate void SignClickDele(int id);
           
        public SignClickDele SignItemCallBack;//�����ť�ص�

        private Image m_NormalImage;
        private Image m_GlowImage;
        private Image m_ReadyImage;
        
        public Image m_IconImage;
        private Image m_GouImage;
        private Image m_KonfuImage;
        public Image m_KonfuNameImage;

        private Text m_CountText;
        private Text m_DayText;

        private Button m_Button;//�����ť

        private int m_ID;
        public RewardBase RewardCfg;

        public SignInStatus Status;

        public SignInItem(int id, Transform trans, RewardBase signConfig)
        {
            FindChildObject(trans);

            m_ID = id;
            m_DayText.text = GetWordByNum(id + 1);
            RewardCfg = signConfig;
            
            SetCount(signConfig.Count);

            m_Button.onClick.AddListener(() =>
            {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

                SignItemCallBack?.Invoke(m_ID);
            });

            if (RewardCfg.RewardItem != RewardItemType.Kungfu)
            {
                m_KonfuImage.gameObject.SetActive(false);
            }
            else
            {
                m_IconImage.gameObject.SetActive(false);
            }
        }
        string GetWordByNum(int num)
        {
            switch (num)
            {
                case 2:
                    return "�ڶ���";
                case 3:
                    return "������";
                case 4:
                    return "������";
                case 5:
                    return "������";
                case 6:
                    return "������";
                case 7:
                    return "������";
                case 1:
                default:
                    return "��һ��";
            }
        }

        public void ClickSignBtn()
        {
            SignItemCallBack?.Invoke(m_ID);
        }

        /// <summary>
        /// ���������������
        /// </summary>
        /// <param name="trans"></param>
        private void FindChildObject(Transform trans)
        {
            m_NormalImage = trans.Find("NormalBG").GetComponent<Image>();
            m_GlowImage = trans.Find("GlowBG").GetComponent<Image>();
            m_ReadyImage = trans.Find("AlreadyBG").GetComponent<Image>();
            m_IconImage = trans.Find("Icon").GetComponent<Image>();

            m_GouImage = trans.Find("Select").GetComponent<Image>();
            m_Button = trans.Find("Button").GetComponent<Button>();

            m_CountText = trans.Find("Count").GetComponent<Text>();
            m_DayText = trans.Find("Day").GetComponent<Text>();

            m_KonfuImage = trans.Find("KongfuBg").GetComponent<Image>();
            m_KonfuNameImage = m_KonfuImage.transform.Find("KongfuName").GetComponent<Image>();
        }

        /// <summary>
        /// ����SignItem���·���������
        /// </summary>
        /// <param name="count"></param>
        public void SetCount(int count)
        {
            m_CountText.text = "��" + count.ToString();
        }

        /// <summary>
        /// �ı�SignItem����ʾ״̬
        /// </summary>
        /// <param name="status"></param>
        public void SetSignItemStatus(SignInStatus status)
        {
            switch (status)
            {
                case SignInStatus.SignEnable:
                    m_NormalImage.gameObject.SetActive(true);
                    m_GlowImage.gameObject.SetActive(true);
                    m_ReadyImage.gameObject.SetActive(true);
                    m_GouImage.gameObject.SetActive(false);
                    m_DayText.gameObject.SetActive(true);
                    m_IconImage.color = new Color(m_IconImage.color.r, m_IconImage.color.g, m_IconImage.color.b, 1);
                    if (RewardCfg.RewardItem == RewardItemType.Kungfu)
                    {
                        m_KonfuImage.color = new Color(m_KonfuImage.color.r, m_KonfuImage.color.g, m_KonfuImage.color.b, 1);
                        m_KonfuNameImage.color = new Color(m_KonfuNameImage.color.r, m_KonfuNameImage.color.g, m_KonfuNameImage.color.b, 1);
                    }
                    break;
                case SignInStatus.SignDisable:
                    m_NormalImage.gameObject.SetActive(true);
                    m_GlowImage.gameObject.SetActive(false);
                    m_ReadyImage.gameObject.SetActive(false);
                    m_GouImage.gameObject.SetActive(false);
                    m_DayText.gameObject.SetActive(true);
                    m_IconImage.color = new Color(m_IconImage.color.r, m_IconImage.color.g, m_IconImage.color.b, 0.4f);
                    if (RewardCfg.RewardItem == RewardItemType.Kungfu)
                    {
                        m_KonfuImage.color = new Color(m_KonfuImage.color.r, m_KonfuImage.color.g, m_KonfuImage.color.b, 0.4f);
                        m_KonfuNameImage.color = new Color(m_KonfuNameImage.color.r, m_KonfuNameImage.color.g, m_KonfuNameImage.color.b, 0.4f);
                    }
                    break;
                case SignInStatus.SignAlready:
                    m_NormalImage.gameObject.SetActive(true);
                    m_GlowImage.gameObject.SetActive(false);
                    m_ReadyImage.gameObject.SetActive(false);
                    m_GouImage.gameObject.SetActive(true);
                    m_DayText.gameObject.SetActive(true);
                    m_IconImage.color = new Color(m_IconImage.color.r, m_IconImage.color.g, m_IconImage.color.b, 0.4f);
                    if (RewardCfg.RewardItem == RewardItemType.Kungfu)
                    {
                        m_KonfuImage.color = new Color(m_KonfuImage.color.r, m_KonfuImage.color.g, m_KonfuImage.color.b, 0.4f);
                        m_KonfuNameImage.color = new Color(m_KonfuNameImage.color.r, m_KonfuNameImage.color.g, m_KonfuNameImage.color.b, 0.4f);
                    }
                    break;
                case SignInStatus.None:
                    break;
                default:
                    break;
            }
            Status = status;
        }


    }
	
}