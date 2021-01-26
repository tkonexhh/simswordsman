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
        [SerializeField]
        private Text m_Time;

        private bool IsOpen = false;

        private const int CostBaozi = 5;
        private List<Sprite> m_NeedSprites;
        private SimGameTask m_CurTaskInfo;
        private CommonTaskItemInfo m_CommonTaskItemInfo;
        private List<TaskReward> m_ItemReward;
        private List<CharacterController> m_CharacterController;
        private Vector2 m_OpenDelta;
        private List<BulletinBoardDisciple> m_BulletinBoardDiscipleList = new List<BulletinBoardDisciple>();
        private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem>();


        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_CurTaskInfo = t as SimGameTask;
            EventSystem.S.Register(EventID.OnSelectedConfirmEvent, HandAddListenerEvent);
            EventSystem.S.Register(EventID.OnSendDiscipleDicEvent, HandAddListenerEvent);
            m_CommonTaskItemInfo = m_CurTaskInfo.CommonTaskItemInfo;
            m_NeedSprites = (List<Sprite>)obj[0];
            BindAddListenerEvent();
            GetInformationForNeed();
            StartCoroutine(CountDown());
            RefreshFixedInfo();
            RefreshPanelInfo();
            RefreshTaskState();
            RefreshDiscipleInfo();
        }

        private void OnDisable()
        {
            EventSystem.S.UnRegister(EventID.OnSelectedConfirmEvent, HandAddListenerEvent);
            EventSystem.S.UnRegister(EventID.OnSendDiscipleDicEvent, HandAddListenerEvent);
        }

        private void HandAddListenerEvent(int key, object[] param)
        {
            switch ((EventID)key)
            {
                case EventID.OnSelectedConfirmEvent:
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
                case EventID.OnSendDiscipleDicEvent:
                    if (m_CommonTaskItemInfo.id != ((SimGameTask)param[0]).CommonTaskItemInfo.id)
                        return;
                    if ((SimGameTask)param[0]!=null)
                        UIMgr.S.OpenPanel(UIID.BulletinBoardChooseDisciple, OpenCallback, (SimGameTask)param[0]);
                    else
                        UIMgr.S.OpenPanel(UIID.BulletinBoardChooseDisciple, OpenCallback, m_CurTaskInfo);
                    break;
                default:
                    break;
            }
        }

        private void OpenCallback(AbstractPanel obj)
        {
            BulletinBoardChooseDisciple bulletinBoardChooseDisciple = obj as BulletinBoardChooseDisciple;
            bulletinBoardChooseDisciple.AddDiscipleDicDic(m_SelectedDiscipleDic);
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
            m_CharacterController = MainGameMgr.S.CharacterMgr.GetAllCharacterInTask(m_CommonTaskItemInfo.id);
        }

        private void BindAddListenerEvent()
        {
            m_FuncBtn.onClick.AddListener(()=> {
                IsOpen = !IsOpen;
                RefreshPanelInfo();
            });
            //前往
            m_GoToBtn.onClick.AddListener(()=> {
               int baoz = int.Parse(m_Baozi.text);
                if (baoz < GameDataMgr.S.GetPlayerData().GetFoodNum())
                {
                    FloatMessage.S.ShowMsg("食物不足，过会儿再来吧");
                    return;
                }
                else
                {
                    RefreshBtnInfo();
                    GameDataMgr.S.GetPlayerData().ReduceFoodNum(baoz);

                    List<CharacterController> selectedControllerList = new List<CharacterController>();
                    foreach (var item in m_SelectedDiscipleDic.Values)
                    {
                        CharacterController controller = MainGameMgr.S.CharacterMgr.GetCharacterController(item.id);
                        if (controller != null)
                        {
                            selectedControllerList.Add(controller);
                        }
                    }
                    m_CurTaskInfo.ExecuteTask(selectedControllerList);
                }
            });
            //婉拒
            m_DeclinedBtn.onClick.AddListener(()=> {
                UIMgr.S.OpenPanel(UIID.LogPanel, LogCallBack, "提示","您确定要放弃任务吗");
            });
            m_Promptly.onClick.AddListener(()=> {
                if (m_CurTaskInfo.GetCurTaskState() == TaskState.Unclaimed)
                {
                    MainGameMgr.S.CommonTaskMgr.ClaimReward(m_CurTaskInfo.TaskId);
                    DestroyImmediate(this.gameObject);
                }
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
                yield return new WaitForSeconds(1);
                int startTime = MainGameMgr.S.CommonTaskMgr.GetTaskExecutedTime(m_CommonTaskItemInfo.id);
                int deltaTime = m_CommonTaskItemInfo.taskTime;
                second = deltaTime - startTime;
                m_Time.text = SplicingTime(deltaTime - startTime);
            }
        }

        public string SplicingTime(int seconds)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(seconds));
            string str = "";

            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:" + ts.Seconds.ToString("00");
            }

            return str;
        }
        private void OnSucessEvent()
        {
            //MainGameMgr.S.CommonTaskMgr.RemoveTask(m_CurTaskInfo.TaskId);
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

            foreach (var item in m_CharacterController)
                m_SelectedDiscipleDic.Add(item.CharacterId,MainGameMgr.S.CharacterMgr.GetCharacterItem(item.CharacterId));
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
            item.OnInit(m_CurTaskInfo);
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
                    m_Over.gameObject.SetActive(false);
                    m_Promptly.gameObject.SetActive(false);
                    break;
                case TaskState.Unclaimed:
                    m_FuncBtnText.text = CommonUIMethod.GetStrForColor("#657D5D", Define.BULLETINBOARD_REWARD);
                    m_Over.gameObject.SetActive(false);
                    m_PromptlyValue.text = "领取奖励";
                    m_Promptly.gameObject.SetActive(true);
                    m_GoToBtn.gameObject.SetActive(false);
                    m_DeclinedBtn.gameObject.SetActive(false);
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