using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class BulletinBoardItem : MonoBehaviour,ItemICom
	{
        [SerializeField]
        private RectTransform m_Bg;

        [Header("Top")]
        [SerializeField]
        private Image m_TaskPhoto;
        [SerializeField]
        private Text m_TaskName;  
        [SerializeField]
        private Text m_TaskIntrodution; 
        [SerializeField]
        private Image m_Arrow;
        [SerializeField]
        private Button m_FuncBtn; 
        [SerializeField]
        private Text m_FuncBtnText; 
        [SerializeField]
        private Image m_Over;       
        [SerializeField]
        private Image m_RedPoint;

        [Header("Bottom")]
        [SerializeField]
        private GameObject m_Bottom;
        [SerializeField]
        private Text m_Description;
        [SerializeField]
        private Image m_Point;
        [SerializeField]
        private Text m_RewardCompleted;  
        [SerializeField]
        private Image m_Res1Img;     
        [SerializeField]
        private Text m_Res1Value;    
        [SerializeField]
        private Image m_Res2Img;     
        [SerializeField]
        private Text m_Res2Value;
        [SerializeField]
        private Image m_Res3Img;     
        [SerializeField]
        private Text m_Res3Value;  
        [SerializeField]
        private Text m_ChooseDisciple; 
        [SerializeField]
        private Transform m_MiddleDown;
        [SerializeField]
        private GameObject m_BulletinBoardDisciple;  
        [SerializeField]
        private Button m_DeclinedBtn;     
        [SerializeField]
        private Text m_DeclinedText;    
        [SerializeField]
        private Button m_GoToBtn;     
        [SerializeField]
        private Text m_GoTo;
        [SerializeField]
        private Button m_Promptly;
        [SerializeField]
        private Text m_PromptlyValue;
        [SerializeField]
        private Text m_Baozi;

        private bool IsOpen = false;

        private const int CostBaozi = 5;
        private List<Sprite> m_NeedSprites;
        private SimGameTask m_CurTaskInfo;
        private CommonTaskItemInfo m_CommonTaskItemInfo;
        private List<TaskReward> m_ItemReward;
        private Vector2 m_OpenDelta;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_CurTaskInfo = t as SimGameTask;
            m_CommonTaskItemInfo = m_CurTaskInfo.CommonTaskItemInfo;
            m_NeedSprites = (List<Sprite>)obj[0];
            BindAddListenerEvent();
            GetInformationForNeed();


            RefreshFixedInfo();
            RefreshPanelInfo();
            RefreshTaskState();
            RefreshDiscipleInfo();
        }

        private Sprite GetSprite(int id)
        {
            return m_NeedSprites.Where(i => i.name.Equals(GetStrForItemID(id))).FirstOrDefault();
        }
        public string GetStrForItemID(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
        }

        private void GetInformationForNeed()
        {
            m_OpenDelta = m_Bottom.GetComponent<RectTransform>().sizeDelta;
            m_ItemReward = m_CommonTaskItemInfo.GetItemRewards();
        }

        private void BindAddListenerEvent()
        {
            m_FuncBtn.onClick.AddListener(()=> {
                IsOpen = !IsOpen;
                RefreshPanelInfo();
            });
        }
        /// <summary>
        /// 刷新固定信息
        /// </summary>
        private void RefreshFixedInfo()
        {
            m_TaskName.text = m_CommonTaskItemInfo.title;
            m_RewardCompleted.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_REWARD);
            m_DeclinedText.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_DECLINED);
            m_GoTo.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_GOTO);
            m_TaskIntrodution.text = m_CommonTaskItemInfo.desc;
            m_Description.text = CommonUIMethod.TextIndent() + m_CommonTaskItemInfo.desc;

            switch (m_ItemReward.Count)
            {
                case 1:
                    m_Res1Img.sprite = GetSprite(m_ItemReward[0].id);
                    m_Res1Value.text = m_ItemReward[0].count1.ToString();
                    m_Res2Img.gameObject.SetActive(false);
                    m_Res2Value.text = Define.COMMON_DEFAULT_STR;
                    m_Res3Img.gameObject.SetActive(false);
                    m_Res3Value.text = Define.COMMON_DEFAULT_STR;
                    break;
                case 2:
                    m_Res1Img.sprite = GetSprite(m_ItemReward[0].id);
                    m_Res1Value.text = m_ItemReward[0].count1.ToString();
                    m_Res2Img.sprite = GetSprite(m_ItemReward[1].id);
                    m_Res2Value.text = m_ItemReward[1].count1.ToString();
                    m_Res3Img.gameObject.SetActive(false);
                    m_Res3Value.text = Define.COMMON_DEFAULT_STR;
                    break;
                case 3:
                    m_Res1Img.sprite = GetSprite(m_ItemReward[0].id);
                    m_Res1Value.text = m_ItemReward[0].count1.ToString();
                    m_Res2Img.sprite = GetSprite(m_ItemReward[1].id);
                    m_Res2Value.text = m_ItemReward[1].count1.ToString();
                    m_Res2Img.sprite = GetSprite(m_ItemReward[2].id);
                    m_Res2Value.text = m_ItemReward[2].count1.ToString();
                    break;
            }
            m_Baozi.text = (m_CommonTaskItemInfo.GetCharacterAmount() * CostBaozi).ToString();

        }

        private void RefreshDiscipleInfo()
        {
            for (int i = 0; i < m_CommonTaskItemInfo.GetCharacterAmount(); i++)
                CreateDisciple();
        }

        private void CreateDisciple()
        {
            GameObject obj = Instantiate(m_BulletinBoardDisciple, m_MiddleDown);
            ItemICom item = obj.GetComponent<ItemICom>();
            //item.OnInit();
        }
      
        /// <summary>
        /// 刷新打开关闭面板信息
        /// </summary>
        private void RefreshPanelInfo()
        {
            //默认关闭
            if (!IsOpen)
            {
                m_Arrow.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
                m_FuncBtnText.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_VIEWDETAILS);
                m_Bottom.SetActive(false);
                RefreshContHeight(IsOpen);
            }
            else//打开
            {
                m_Arrow.GetComponent<Transform>().localScale = new Vector3(1, -1, 1);
                m_FuncBtnText.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_PUTAWAY);
                m_Bottom.SetActive(true);
                RefreshContHeight(IsOpen);
            }
        }
        /// <summary>
        /// 刷新任务状态
        /// </summary>
        private void RefreshTaskState()
        {
            switch (m_CurTaskInfo.GetCurTaskState())
            {
                case TaskState.None:
                    break;
                case TaskState.NotStart:
                    m_Over.gameObject.SetActive(false);
                    m_RedPoint.gameObject.SetActive(false);
                    m_Promptly.gameObject.SetActive(false);
                    break;
                case TaskState.Running:
                    break;
                case TaskState.Unclaimed:
                    break;
                case TaskState.Finished:
                    break;
                default:
                    break;
            }
        }

        private void RefreshContHeight(bool isOpen)
        {
            if (!isOpen)
                m_Bg.sizeDelta -= new Vector2(0, m_OpenDelta.y);
            else
                m_Bg.sizeDelta += new Vector2(0, m_OpenDelta.y);
        }

        public void SetButtonEvent(Action<object> action)
        {
        }
	}
}