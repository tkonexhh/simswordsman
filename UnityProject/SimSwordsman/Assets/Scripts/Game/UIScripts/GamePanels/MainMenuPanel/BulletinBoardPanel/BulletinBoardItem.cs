using Qarth;
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
        private Image m_Res1KungfuName;
        [SerializeField]
        private Text m_Res1Value;    
        [SerializeField]
        private Image m_Res2Img;
        [SerializeField]
        private Image m_Res2KungfuName;
        [SerializeField]
        private Text m_Res2Value;
        [SerializeField]
        private Image m_Res3Img;
        [SerializeField]
        private Image m_Res3KungfuName;
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
        private Image m_PromptlyImg;
        [SerializeField]
        private Button m_Promptly;
        [SerializeField]
        private Text m_PromptlyValue;   
        [SerializeField]
        private GameObject m_Advertisement;
        [SerializeField]
        private Text m_Baozi;
        [SerializeField]
        private Text m_Time;
        [SerializeField]
        private Image m_Line;

        private bool IsOpen = false;

        private bool IsStartBattle = false;

        private const int CostBaozi = 5;
        private SimGameTask m_CurTaskInfo;
        private CommonTaskItemInfo m_CommonTaskItemInfo;
        private List<TaskReward> m_ItemReward;
        private List<CharacterController> m_CharacterController;
        private Vector2 m_OpenDelta;
        private List<BulletinBoardDisciple> m_BulletinBoardDiscipleList = new List<BulletinBoardDisciple>();
        private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem>();
        private BulletinBoardPanel m_BulletinBoardPanel;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_CurTaskInfo = t as SimGameTask;
            EventSystem.S.Register(EventID.OnBulletinSelectedConfirmEvent, HandAddListenerEvent);
            EventSystem.S.Register(EventID.OnBulletinSendDiscipleDicEvent, HandAddListenerEvent);
            //EventSystem.S.Register(EventID.OnArriveCollectResPos, HandAddListenerEvent);
            EventSystem.S.Register(EventID.OnStowPanelEvent, HandAddListenerEvent);
            m_CommonTaskItemInfo = m_CurTaskInfo.CommonTaskItemInfo;
            m_BulletinBoardPanel = (BulletinBoardPanel)obj[0];
            BindAddListenerEvent();
            GetInformationForNeed();
            RefreshFixedInfo();
            RefreshPanelInfo();
            RefreshDiscipleInfo();
            gameObject.name = "BulletinBoardItem" + m_CurTaskInfo.TaskId;
            RefreshTaskState();
        }

        private void OnDisable()
        {
            EventSystem.S.UnRegister(EventID.OnBulletinSelectedConfirmEvent, HandAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnBulletinSendDiscipleDicEvent, HandAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnArriveCollectResPos, HandAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnStowPanelEvent, HandAddListenerEvent);
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnBulletinSelectedConfirmEvent:
                    if (m_CommonTaskItemInfo.id != ((CommonTaskItemInfo)param[1]).id)
                        return;
                    m_SelectedDiscipleDic = (Dictionary<int, CharacterItem>)param[0];
                    int i = 0;
                    foreach (var item in m_SelectedDiscipleDic.Values)
                    {
                        m_BulletinBoardDiscipleList[i].RefreshSelectedDisciple(item);
                        i++;
                    }
                    for (int j = m_SelectedDiscipleDic.Values.Count; j < m_BulletinBoardDiscipleList.Count; j++)
                        m_BulletinBoardDiscipleList[j].RefreshSelectedDisciple(null);
                    break;
                case EventID.OnBulletinSendDiscipleDicEvent:
                    if (m_CommonTaskItemInfo.id != ((SimGameTask)param[0]).CommonTaskItemInfo.id)
                        return;
                    if ((SimGameTask)param[0]!=null)
                        UIMgr.S.OpenPanel(UIID.SendDisciplesPanel,  OpenCallback, PanelType.Task, (SimGameTask)param[0]);
                    else
                        UIMgr.S.OpenPanel(UIID.SendDisciplesPanel, OpenCallback, PanelType.Task, m_CurTaskInfo);
                    break;
                case EventID.OnArriveCollectResPos:
                    break;
                case EventID.OnStowPanelEvent:
                    if ((bool)param[0] && ((SimGameTask)param[1]).TaskId != m_CurTaskInfo.TaskId && IsOpen)
                    {
                        IsOpen = false;
                        RefreshPanelInfo();
                    }
                    break;
                default:
                    break;
            }
        }
        private void OpenCallback(AbstractPanel obj)
        {
            SendDisciplesPanel sendDisciplesPanel = obj as SendDisciplesPanel;
            sendDisciplesPanel.AddDiscipleDicDic(m_SelectedDiscipleDic);
        } 

        private void GetSprite(TaskReward taskReward,Image image, Image res1KungfuName)
        {
            switch (taskReward.rewardType)
            {
                case TaskRewardType.Item:
                    image.sprite = m_BulletinBoardPanel.FindSprite(TDItemConfigTable.GetIconName((int)taskReward.id));
                    break;
                case TaskRewardType.Armor:
                case TaskRewardType.Arms:
                    image.sprite = m_BulletinBoardPanel.FindSprite(TDEquipmentConfigTable.GetIconName((int)taskReward.id));
                    break;
                case TaskRewardType.Kongfu:
                    SetKungfuSprite(taskReward, image, res1KungfuName);
                    break;
                case TaskRewardType.Medicine:
                    image.sprite = m_BulletinBoardPanel.FindSprite(TDHerbConfigTable.GetHerbIconNameById((int)taskReward.id));
                    break;
                case TaskRewardType.Food:
                    image.sprite = m_BulletinBoardPanel.FindSprite("Baozi");
                    break;
                case TaskRewardType.Coin:
                    image.sprite = m_BulletinBoardPanel.FindSprite("Coin");
                    break;
            }
        }
        private KungfuQuality GetKungfuQuality(KungfuType kungfuType)
        {
            return TDKongfuConfigTable.GetKungfuConfigInfo(kungfuType).KungfuQuality;
        }
        private void SetKungfuSprite(TaskReward item, Image image, Image kungfuName)
        {
            kungfuName.gameObject.SetActive(true);
            switch (GetKungfuQuality((KungfuType)item.id))
            {
                case KungfuQuality.Normal:
                    image.sprite = m_BulletinBoardPanel.FindSprite("Introduction");
                    break;
                case KungfuQuality.Master:
                    image.sprite = m_BulletinBoardPanel.FindSprite("Advanced");
                    break;
                case KungfuQuality.Super:
                    image.sprite = m_BulletinBoardPanel.FindSprite("Excellent");
                    break;
                default:
                    break;
            }
            kungfuName.sprite = m_BulletinBoardPanel.FindSprite(TDKongfuConfigTable.GetIconName((KungfuType)item.id));
        }
        private string GetIconName(int herbType)
        {
            return TDHerbConfigTable.GetHerbIconNameById(herbType);
        }

        public string GetStrForItemID(int id)
        {
            return MainGameMgr.S.InventoryMgr.GetIconName(id);
        }

        private void GetInformationForNeed()
        {
            m_OpenDelta = m_Bottom.GetComponent<RectTransform>().sizeDelta;
            m_ItemReward = m_CommonTaskItemInfo.GetItemRewards();
            m_CharacterController = m_CurTaskInfo.GetRecordCharacterController();
        }

        private void BindAddListenerEvent()
        {
            m_FuncBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                IsOpen = !IsOpen;
                RefreshPanelInfo();
                if (IsOpen)
                     EventSystem.S.Send(EventID.OnStowPanelEvent, IsOpen, m_CurTaskInfo);

            });
            //前往
            m_GoToBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                DataAnalysisMgr.S.CustomEvent(DotDefine.c_task_enter, m_CurTaskInfo.TaskId.ToString());
                UIMgr.S.OpenPanel(UIID.SendDisciplesPanel, OpenCallback, PanelType.Task, m_CurTaskInfo);
            });
            //婉拒
            m_DeclinedBtn.onClick.AddListener(()=> {
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                DataAnalysisMgr.S.CustomEvent(DotDefine.c_task_refuse, m_CurTaskInfo.TaskId.ToString());
                UIMgr.S.OpenPanel(UIID.LogPanel, LogCallBack, "提示","您确定要放弃任务吗");
            });
            m_Promptly.onClick.AddListener(()=> {
            });
        }
        private void LogCallBack(AbstractPanel abstractPanel)
        {
            LogPanel logPanel= abstractPanel as LogPanel;
            logPanel.OnSuccessBtnEvent += OnSucessEvent;
        }

        private IEnumerator CountDown()
        {
            int second = 1;
            while (second>0)
            {
                yield return null;
                int executedTime = MainGameMgr.S.CommonTaskMgr.GetTaskExecutedTime(m_CommonTaskItemInfo.id);
                int totalTime = m_CommonTaskItemInfo.taskTime;

                second = totalTime - executedTime;
                if (totalTime - executedTime <= 0)
                {
                    if (m_CommonTaskItemInfo.taskType == SimGameTaskType.Battle)
                        IsStartBattle = true;
                    RefreshTaskState();
                }
            }
        }
        private void OnSucessEvent()
        {
            MainGameMgr.S.CommonTaskMgr.RemoveTask(m_CurTaskInfo.TaskId);
            DestroyImmediate(this.gameObject);
        }

        private void RefreshBtnInfo()
        {
            m_GoToBtn.gameObject.SetActive(false);
            m_DeclinedBtn.gameObject.SetActive(false);
            m_Promptly.gameObject.SetActive(true);
        }

        /// <summary>
        /// 刷新固定信息
        /// </summary>
        private void RefreshFixedInfo()
        {
            m_TaskName.text = m_CommonTaskItemInfo.title;
            m_RewardCompleted.text = "可能获得";
            m_DeclinedText.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_DECLINED);
            m_GoTo.text = CommonUIMethod.GetStringForTableKey(Define.COMMON_GOTO);
            m_TaskIntrodution.text = m_CommonTaskItemInfo.desc;
            m_Description.text = CommonUIMethod.TextIndent() + m_CommonTaskItemInfo.taskTxt;
            m_TaskPhoto.sprite = m_BulletinBoardPanel.FindSprite("enemy_icon_"+ m_CommonTaskItemInfo.iconRes);
            switch (m_ItemReward.Count)
            {
                case 1:
                    GetSprite(m_ItemReward[0], m_Res1Img, m_Res1KungfuName);
                    m_Res1Value.text = m_ItemReward[0].count1.ToString();
                    m_Res2Img.gameObject.SetActive(false);
                    m_Res2Value.text = Define.COMMON_DEFAULT_STR;
                    m_Res3Img.gameObject.SetActive(false);
                    m_Res3Value.text = Define.COMMON_DEFAULT_STR;
                    break;
                case 2:
                    GetSprite(m_ItemReward[0], m_Res1Img, m_Res1KungfuName);
                    m_Res1Value.text = m_ItemReward[0].count1.ToString();
                    GetSprite(m_ItemReward[1], m_Res2Img, m_Res2KungfuName);
                    m_Res2Value.text = m_ItemReward[1].count1.ToString();
                    m_Res3Img.gameObject.SetActive(false);
                    m_Res3Value.text = Define.COMMON_DEFAULT_STR;
                    break;
                case 3:
                    GetSprite(m_ItemReward[0], m_Res1Img, m_Res1KungfuName);
                    m_Res1Value.text = m_ItemReward[0].count1.ToString();
                    GetSprite(m_ItemReward[1], m_Res2Img, m_Res2KungfuName);
                    m_Res2Value.text = m_ItemReward[1].count1.ToString();
                    GetSprite(m_ItemReward[2], m_Res3Img, m_Res3KungfuName);
                    m_Res3Value.text = m_ItemReward[2].count1.ToString();
                    break;
            }
            if (m_CommonTaskItemInfo.id == 9001 || m_CommonTaskItemInfo.id == 9002)
                m_Baozi.text = "0";
            else
                m_Baozi.text = (m_CommonTaskItemInfo.GetCharacterAmount() * CostBaozi).ToString();
        }


        private void RefreshDiscipleInfo()
        {
            for (int i = 0; i < m_CommonTaskItemInfo.GetCharacterAmount(); i++)
                CreateDisciple();

            foreach (var item in m_CharacterController)
            {
                CharacterItem characterItem = MainGameMgr.S.CharacterMgr.GetCharacterItem(item.CharacterId);
                m_SelectedDiscipleDic.Add(item.CharacterId, characterItem);
            }
            int k = 0;
            foreach (var item in m_SelectedDiscipleDic.Values)
            {
                m_BulletinBoardDiscipleList[k].RefreshSelectedDisciple(item);
                k++;
            }
            for (int j = m_SelectedDiscipleDic.Values.Count; j < m_BulletinBoardDiscipleList.Count; j++)
                m_BulletinBoardDiscipleList[j].RefreshSelectedDisciple(null);
        }

        private void CreateDisciple()
        {
            GameObject obj = Instantiate(m_BulletinBoardDisciple, m_MiddleDown);
            BulletinBoardDisciple item = obj.GetComponent<BulletinBoardDisciple>();
            item.OnInit(m_CurTaskInfo,null, m_BulletinBoardPanel);
            m_BulletinBoardDiscipleList.Add(item);
        }
      
        /// <summary>
        /// 刷新打开关闭面板信息
        /// </summary>
        private void RefreshPanelInfo()
        {
            //默认关闭
            if (!IsOpen)
            {
                if (m_CurTaskInfo.GetCurTaskState()== TaskState.Unclaimed)
                    m_FuncBtnText.text = CommonUIMethod.GetStrForColor("#657D5D", Define.BULLETINBOARD_REWARD);
                else
                    m_FuncBtnText.text = CommonUIMethod.GetStringForTableKey(Define.BULLETINBOARD_VIEWDETAILS);
                m_Arrow.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
                m_Bottom.SetActive(false);
                RefreshContHeight(IsOpen);
            }
            else//打开
            {
                DataAnalysisMgr.S.CustomEvent(DotDefine.c_task_check,m_CurTaskInfo.TaskId.ToString());
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
                    //m_RedPoint.gameObject.SetActive(false);
                    m_Time.text = Define.COMMON_DEFAULT_STR;
                    m_Promptly.gameObject.SetActive(false);
                    break;
                case TaskState.Running:
                    m_Over.gameObject.SetActive(false);
                    foreach (var item in m_BulletinBoardDiscipleList)
                        item.SetBtnClick(false);
                    m_Time.text = Define.COMMON_DEFAULT_STR;
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