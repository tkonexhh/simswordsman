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
		[SerializeField]
		private Image m_DoubleSpeedBtnImg;
		[Header("StateLock")]
		[SerializeField]
		private Text m_StateLock;
		[SerializeField]
		private Color m_YellowColor;
		[SerializeField]
		private Color m_GrayColor;

		private SingleDeliverDetailData m_SingleDeliverDetailData;
		private DeliverConfig m_DeliverConfig;
		private DeliverLevelInfo m_DeliverLevelInfo;
		private DeliverState m_DeliverPanelType;
		private List<DeliverDisciple> m_DeliverDiscipleList = new List<DeliverDisciple>();
		private List<DeliverRes> m_DeliverResList = new List<DeliverRes>();
		private List<int> m_DeliverDBLsst = null;
		private const int DiscipleNumber = 4;
		private CountDownItemTest m_CountDownItemTest;

		private List<CharacterItem> m_AllDiscipleList;
		// Start is called before the first frame update
		private Dictionary<int, CharacterItem> m_SelectedDiscipleDic = new Dictionary<int, CharacterItem> ();
		private void Awake()
		{
			EventSystem.S.Register(EventID.OnOpenChallengChoosePanel, HandleAddListenerEvent);
			EventSystem.S.Register(EventID.OnSelectedConfirmEvent, HandleAddListenerEvent);
			EventSystem.S.Register(EventID.OnDeliverCarArrive, HandleAddListenerEvent);
		}

        private void DeliverCountDownItemUpdateCallBack(int remaintTimeSeconds)
        {
            if (remaintTimeSeconds<=0)
            {
				m_CountDown.text = CommonUIMethod.SplicingTime(0);
				m_CountDownSlider.value = 0;
				return;
			}
			m_CountDown.text = CommonUIMethod.SplicingTime(remaintTimeSeconds);
			m_CountDownSlider.value = (float)remaintTimeSeconds / m_SingleDeliverDetailData.GetTotalTimeSeconds();
			////m_SingleDeliverDetailData.GetTotalTimeSeconds();
		}

        private void HandleAddListenerEvent(int key, params object[] args)
		{
            switch (key)
            {
				case (int)EventID.OnOpenChallengChoosePanel:
					if ((int)args[0] == m_SingleDeliverDetailData.DeliverID)
						UIMgr.S.OpenPanel(UIID.ChallengeChooseDisciple, OpenCallback, PanelType.Deliver, m_SingleDeliverDetailData);
					break;
				case (int)EventID.OnDeliverCarArrive:
					if ((int)args[0] == m_SingleDeliverDetailData.DeliverID)
					{
						m_CountDownItemTest = CountDowntMgr.S.GetCountDownItemByID(m_SingleDeliverDetailData.GetCountDownID());
						RefreshPanelInfo();
					}
					break;
				case (int)EventID.OnSelectedConfirmEvent:
					if ((int)args[1] == m_SingleDeliverDetailData.DeliverID)
					{
						m_SelectedDiscipleDic = (Dictionary<int, CharacterItem>)args[0];
						List<int> characterList = new List<int>();
                        foreach (var item in m_SelectedDiscipleDic.Values)
							characterList.Add(item.id);
						GameDataMgr.S.GetClanData().DeliverData.AddDeliverDisciple(m_SingleDeliverDetailData.DeliverID, characterList);
						FillingDisciple();

						DeliverSystemMgr.S.StartDeliver(m_SingleDeliverDetailData.DeliverID);
						RefreshPanelInfo();
						if (m_CountDownItemTest == null)
						{
							m_CountDownItemTest = CountDowntMgr.S.GetCountDownItemByID(m_SingleDeliverDetailData.GetCountDownID());
							DeliverCountDownItemUpdateCallBack(m_SingleDeliverDetailData.GetRemainTimeSeconds());
							m_CountDownItemTest.RegisterUpdateCallBack(DeliverCountDownItemUpdateCallBack);
							m_CountDownItemTest.RegisterEndCallBack(DeliverCountDownItemEndCallBack);
						}
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
			m_DeliverDBLsst = GameDataMgr.S.GetClanData().DeliverData.GetDeliverDisciple(m_SingleDeliverDetailData.DeliverID);
			for (int i = 0; i < m_DeliverDBLsst.Count; i++)
				m_SelectedDiscipleDic.Add(m_DeliverDBLsst[i],MainGameMgr.S.CharacterMgr.GetCharacterItem(m_DeliverDBLsst[i]));

			ChallengeChooseDisciple challengeChooseDisciple = obj as ChallengeChooseDisciple;
			challengeChooseDisciple.AddDiscipleDicDic(m_SelectedDiscipleDic);
		}

        private void OnDestroy()
        {
			EventSystem.S.UnRegister(EventID.OnOpenChallengChoosePanel, HandleAddListenerEvent);
			EventSystem.S.UnRegister(EventID.OnSelectedConfirmEvent, HandleAddListenerEvent);
			EventSystem.S.UnRegister(EventID.OnDeliverCarArrive, HandleAddListenerEvent);
			m_CountDownItemTest?.UnRegisterUpdateCallBack(DeliverCountDownItemUpdateCallBack);
			m_CountDownItemTest?.UnRegisterEndCallBack(DeliverCountDownItemEndCallBack);
		}

		public void QuickStartAddDisciple(CharacterQuality quality,int surplus, List<CharacterItem> characterItems)
		{
            if (characterItems.Count == DiscipleNumber)
				return;

			List<CharacterItem> normalList = MainGameMgr.S.CharacterMgr.GetCharacterForQuality(quality);
			CommonUIMethod.BubbleSortForType(normalList, CommonUIMethod.SortType.Level, CommonUIMethod.OrderType.FromSmallToBig);
			if (normalList.Count >= surplus)
			{
				int number = 0;
				for (int i = 0; i < normalList.Count; i++)
				{
                    if (number == surplus)
						break;
					if (normalList[i].IsFreeState())
					{
						number++;
						characterItems.Add(normalList[i]);
					}
				}
			}
			else
			{
                for (int i = 0; i < normalList.Count; i++)
					if (normalList[i].IsFreeState())
						characterItems.Add(normalList[i]);

				surplus = surplus - characterItems.Count;
			}
			if ((int)quality <= 3)
			{
				QuickStartAddDisciple(quality + 1, surplus, characterItems);
			}
		}

		void Start()
		{
            if (m_SingleDeliverDetailData.DaliverState == DeliverState.HasBeenGoOut)
            {
				DeliverCountDownItemUpdateCallBack(m_SingleDeliverDetailData.GetRemainTimeSeconds());
				m_CountDownItemTest = CountDowntMgr.S.GetCountDownItemByID(m_SingleDeliverDetailData.GetCountDownID());
				m_CountDownItemTest?.RegisterUpdateCallBack(DeliverCountDownItemUpdateCallBack);
				m_CountDownItemTest?.RegisterEndCallBack(DeliverCountDownItemEndCallBack);
			}

			m_QuickStart.onClick.AddListener(()=> {
				AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

				if (m_SelectedDiscipleDic.Count==0)
                {
					List<CharacterItem> characterItems = new List<CharacterItem>();
					QuickStartAddDisciple(CharacterQuality.Normal, DiscipleNumber, characterItems);

                    if (characterItems.Count != DiscipleNumber)
                    {
						FloatMessage.S.ShowMsg("人数不足");
						return;
                    }

					List<int> characterList = new List<int>();
					foreach (var item in characterItems)
						characterList.Add(item.id);
					GameDataMgr.S.GetClanData().DeliverData.AddDeliverDisciple(m_SingleDeliverDetailData.DeliverID, characterList);

					for (int i = 0; i < characterItems.Count; i++)
					{
						m_DeliverDiscipleList[i].OnFillDisciple(characterItems[i]);
					}

					DeliverSystemMgr.S.StartDeliver(m_SingleDeliverDetailData.DeliverID);
					RefreshPanelInfo();
					if (m_CountDownItemTest == null)
					{
						m_CountDownItemTest = CountDowntMgr.S.GetCountDownItemByID(m_SingleDeliverDetailData.GetCountDownID());
						DeliverCountDownItemUpdateCallBack(m_SingleDeliverDetailData.GetRemainTimeSeconds());
						m_CountDownItemTest.RegisterUpdateCallBack(DeliverCountDownItemUpdateCallBack);
						m_CountDownItemTest.RegisterEndCallBack(DeliverCountDownItemEndCallBack);
					}
				}
			});
			m_DoubleSpeedBtn.onClick.AddListener(()=> 
			{ 
                AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);

				AdsManager.S.PlayRewardAD("DeliverSpeedUp", LookDeliverSpeedUpADCallBack);
			});
		}

        private void LookDeliverSpeedUpADCallBack(bool obj)
        {
			DeliverSystemMgr.S.UpdateDeliverSpeedUpMultiple(m_SingleDeliverDetailData.DeliverID);

			RefreshDoubleSpeedBtnState();
		}

        private void RefreshDoubleSpeedBtnState() 
		{
			if (m_SingleDeliverDetailData != null) 
			{
				if (m_SingleDeliverDetailData.IsSpeedUp())
				{
					m_DoubleSpeedBtnImg.raycastTarget = false;
					m_DoubleSpeedBtnImg.color = m_GrayColor;
				}
				else {
					m_DoubleSpeedBtnImg.raycastTarget = true;
					m_DoubleSpeedBtnImg.color = m_YellowColor;
				}
			}
		}

        private void DeliverCountDownItemEndCallBack(int remaintTimeSeconds)
        {
			m_CountDown.text = CommonUIMethod.SplicingTime(0);
			m_CountDownSlider.value = 0;
		}

        public void OnInit(SingleDeliverDetailData item) 
		{
			m_SingleDeliverDetailData = item;
			m_DeliverConfig = TDDeliverTable.GetDeliverConfig(m_SingleDeliverDetailData.DeliverID);
			m_DeliverLevelInfo = TDFacilityDeliverTable.GetDeliverLevelInfoForTeamUnlock(m_DeliverConfig.level);
			m_DeliverDBLsst = GameDataMgr.S.GetClanData().DeliverData.GetDeliverDisciple(m_SingleDeliverDetailData.DeliverID);
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
					m_SingleDeliverDetailData.DaliverState = DeliverState.DidNotGoOut;
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
                case DeliverState.HasBeenGoOut:
					SetDiscipleBtnState(false);
					SwitchState(DeliverState.HasBeenGoOut);
					m_DeliverPhoto.sprite = SpriteHandler.S.GetSprite(AtlasDefine.DeliverAtlas, "Deliver_definephoto");
                    m_DeliverTitle.text = m_DeliverConfig.name;
                    break;
                case DeliverState.DidNotGoOut:
					SetDiscipleBtnState(true);
					SwitchState(DeliverState.DidNotGoOut);
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
                case DeliverState.HasBeenGoOut:
					m_StateStart.SetActive(false);
					m_StateStarting.SetActive(true);
					m_StateLock.gameObject.SetActive(false);
					RefreshDoubleSpeedBtnState();
					break;
                case DeliverState.DidNotGoOut:
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