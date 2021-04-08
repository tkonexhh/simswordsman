using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class DeliverItem : MonoBehaviour
	{
		[SerializeField]
		private Image m_DeliverPhoto;
		[SerializeField]
		private Text m_DeliverTitle;
		[SerializeField]
		private Transform m_DeliverTra;
		[SerializeField]
		private GameObject m_DeliverDisciple;

		[Header("StateStart")]
		[SerializeField]
		private GameObject m_StateStart;
		[SerializeField]
		private Transform m_ResTra;
		[SerializeField]
		private GameObject m_ResItem;
		[SerializeField]
		private Button m_QuickStart;

		[Header("StateStarting")]
		[SerializeField]
		private GameObject m_StateStarting;
		[SerializeField]
		private Text m_CountDown;
		[SerializeField]
		private Slider m_CountDownSlider;
		[SerializeField]
		private Button m_DoubleSpeedBtn;
		[Header("StateLock")]
		[SerializeField]
		private Text m_StateLock;

		private SingleDeliverDetailData m_SingleDeliverDetailData;
		private DeliverConfig m_DeliverConfig;
		private DeliverLevelInfo m_DeliverLevelInfo;
		private DeliverState m_DeliverPanelType;
		private List<DeliverDisciple> m_DeliverDiscipleList = new List<DeliverDisciple>();
		private List<DeliverRes> m_DeliverResList = new List<DeliverRes>();
		private List<int> m_DeliverDBLsst = null;
		private const int DiscipleNumber = 4;
		// Start is called before the first frame update
		private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem> ();
		private void Awake()
		{
			EventSystem.S.Register(EventID.OnOpenChallengChoosePanel, HandleAddListenerEvent);
			EventSystem.S.Register(EventID.OnSelectedConfirmEvent, HandleAddListenerEvent);
		}


		private void HandleAddListenerEvent(int key, params object[] args)
		{
            switch (key)
            {
				case (int)EventID.OnOpenChallengChoosePanel:
					if ((int)args[0] == m_SingleDeliverDetailData.DaliverID)
						UIMgr.S.OpenPanel(UIID.ChallengeChooseDisciple, OpenCallback, PanelType.Deliver, m_SingleDeliverDetailData);
					break;
				case (int)EventID.OnSelectedConfirmEvent:
					if ((int)args[1] == m_SingleDeliverDetailData.DaliverID)
					{
						m_SelectedDiscipleDic = (Dictionary<int, CharacterItem>)args[0];
						List<int> characterList = new List<int>();
                        foreach (var item in m_SelectedDiscipleDic.Values)
							characterList.Add(item.id);
						GameDataMgr.S.GetClanData().DeliverData.AddDeliverDisciple(m_SingleDeliverDetailData.DaliverID, characterList);
						FillingDisciple();
					}
					break;
				default:
                    break;
            }
        }

        private void DiscipleList()
        {
            foreach (var item in m_DeliverDiscipleList)
            {
				item.ResetSelf();
			}
        }

        private void FillingDisciple()
        {
			List<CharacterItem> characterItem = new List<CharacterItem>();
			characterItem.AddRange(m_SelectedDiscipleDic.Values);
			//DiscipleList();
			for (int i = 0; i < characterItem.Count; i++)
            {
				m_DeliverDiscipleList[i].OnFillDisciple(characterItem[i]);
			}
        }

        private void OpenCallback(AbstractPanel obj)
        {
			m_SelectedDiscipleDic.Clear();
			m_DeliverDBLsst = GameDataMgr.S.GetClanData().DeliverData.GetDeliverDisciple(m_SingleDeliverDetailData.DaliverID);
			for (int i = 0; i < m_DeliverDBLsst.Count; i++)
				m_SelectedDiscipleDic.Add(m_DeliverDBLsst[i],MainGameMgr.S.CharacterMgr.GetCharacterItem(m_DeliverDBLsst[i]));

			ChallengeChooseDisciple challengeChooseDisciple = obj as ChallengeChooseDisciple;
			challengeChooseDisciple.AddDiscipleDicDic(m_SelectedDiscipleDic);
		}

        private void OnDestroy()
        {
			EventSystem.S.UnRegister(EventID.OnOpenChallengChoosePanel, HandleAddListenerEvent);
			EventSystem.S.UnRegister(EventID.OnSelectedConfirmEvent, HandleAddListenerEvent);
		}

		void Start()
		{
			m_QuickStart.onClick.AddListener(()=> { 
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
                if (m_SelectedDiscipleDic.Count!=4)
                {
					FloatMessage.S.ShowMsg("请选满人!");
					return;
                }
				m_SingleDeliverDetailData.DaliverState = DeliverState.HasBeenSetOut;
				RefreshPanelInfo();
			});
			m_DoubleSpeedBtn.onClick.AddListener(()=> { 
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
				m_SingleDeliverDetailData.DaliverState = DeliverState.DidNotSetOut;
				m_SelectedDiscipleDic.Clear();
				RefreshPanelInfo();
			});
		}

		public void OnInit(SingleDeliverDetailData item) 
		{
			m_SingleDeliverDetailData = item;
			m_DeliverConfig = TDDeliverTable.GetDeliverConfig(m_SingleDeliverDetailData.DaliverID);
			m_DeliverLevelInfo = TDFacilityDeliverTable.GetDeliverLevelInfoForTeamUnlock(m_DeliverConfig.level);
			m_DeliverDBLsst = GameDataMgr.S.GetClanData().DeliverData.GetDeliverDisciple(m_SingleDeliverDetailData.DaliverID);
			CheckDeliverState();

			for (int i = 0; i < DiscipleNumber; i++)
				CreateDeliverDisciple();
			RefreshPanelInfo();

			SwitchDeliverDiscileState(); 
			//m_DeliverTitle.text = m_DeliverConfig.name;
		}

        private void SwitchDeliverDiscileState()
        {
			for (int i = 0; i < m_SingleDeliverDetailData.CharacterIDList.Count; i++)
			{
				m_DeliverDiscipleList[i].SetDeliverDiscipleState(m_SingleDeliverDetailData.CharacterIDList[i]);
			}
		}

        private void CheckDeliverState()
        {
            if (m_SingleDeliverDetailData.DaliverState == DeliverState.Unlock)
            {
				int deliverLevel = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Deliver);
				if (deliverLevel >= m_DeliverLevelInfo.level)
				{
					m_SingleDeliverDetailData.DaliverState = DeliverState.DidNotSetOut;
				}
			}
        }

		private void SetDiscipleBtnState(bool state)
		{
            foreach (var item in m_DeliverDiscipleList)
            {
				item.SetBtnState(state);
			}
		}
        private void RefreshPanelInfo()
        {
            switch (m_SingleDeliverDetailData.DaliverState)
            {
                case DeliverState.Unlock:
					SetDiscipleBtnState(false);
					SwitchState(DeliverState.Unlock);
					m_StateLock.text = "镖局升至" + CommonUIMethod.GetStrForColor("#9C4B45", CommonUIMethod.GetGrade(m_DeliverLevelInfo.level)) + "后解锁";
					m_DeliverTitle.text = "???";
					m_DeliverPhoto.sprite = SpriteHandler.S.GetSprite(AtlasDefine.PanelCommonAtlas, "Lock3");
					break;
                case DeliverState.HasBeenSetOut:
					SetDiscipleBtnState(false);
					SwitchState(DeliverState.HasBeenSetOut);
					m_DeliverPhoto.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DeliverAtlas, "Deliver_definephoto");
                    m_DeliverTitle.text = m_DeliverConfig.name;
                    break;
                case DeliverState.DidNotSetOut:
					SetDiscipleBtnState(true);
					SwitchState(DeliverState.DidNotSetOut);
					m_DeliverPhoto.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DeliverAtlas, "Deliver_definephoto");
                    m_DeliverTitle.text = m_DeliverConfig.name;
					CreateDeliverRes();
					break;
                default:
                    break;
            }
			m_DeliverPhoto.SetNativeSize();
		}

        private void CreateDeliverRes()
        {
			for (int i = 0; i < m_DeliverResList.Count; i++)
				DestroyImmediate(m_DeliverResList[i].gameObject);
			m_DeliverResList.Clear();

			foreach (var item in m_SingleDeliverDetailData.RewadDataList)
			{
				DeliverRes deliverRes = Instantiate(m_ResItem, m_ResTra).GetComponent<DeliverRes>();
				deliverRes.OnInit(item);
				m_DeliverResList.Add(deliverRes);
			}
		}

		private void CreateDeliverDisciple()
		{
			DeliverDisciple deliverDisciple = Instantiate(m_DeliverDisciple, m_DeliverTra).GetComponent<DeliverDisciple>();
			deliverDisciple.OnInit(m_SingleDeliverDetailData);
			m_DeliverDiscipleList.Add(deliverDisciple);
		}

        //private void Create

        private void SwitchState(DeliverState deliverState)
		{
            switch (deliverState)
            {
                case DeliverState.Unlock:
					m_StateStart.SetActive(false);
					m_StateStarting.SetActive(false);
					m_StateLock.gameObject.SetActive(true);
					break;
                case DeliverState.HasBeenSetOut:
					m_StateStart.SetActive(false);
					m_StateStarting.SetActive(true);
					m_StateLock.gameObject.SetActive(false);
					break;
                case DeliverState.DidNotSetOut:
					foreach (var item in m_DeliverDiscipleList)
						item.SetDeliverDiscipleStateFree();
					m_StateStart.SetActive(true);
					m_StateStarting.SetActive(false);
					m_StateLock.gameObject.SetActive(false);
					break;
                default:
                    break;
            }
        }

        void Update()
	    {
	        
	    }
	}
	
}