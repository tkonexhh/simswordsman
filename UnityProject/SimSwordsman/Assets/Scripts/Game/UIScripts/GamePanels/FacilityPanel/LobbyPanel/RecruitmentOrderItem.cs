using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class RecruitmentOrderItem : MonoBehaviour,ItemICom
	{
		[SerializeField]
		private Image m_Icon;
		[SerializeField]
		private Text m_RecruitmentOrderTitle;
		[SerializeField]
		private Text m_RecruitmentOrderCont;
		[SerializeField]
		private Text m_RecruitValue;
		[SerializeField]
		private Text m_RecruitmentBtnValue;	
		[SerializeField]
		private Button m_RecruitmentBtn;

		private RecruitType m_CurRecruitType;
		private Sprite m_CurSprite;
		private RecruitDiscipleMgr m_RecruitDiscipleMgr = null;
		private bool m_IsFirstRecruitment = false;
		private Dictionary<RecruitType, ClickType> m_RecruitDic = new Dictionary<RecruitType, ClickType>();

		public void OnInit<T>(T t, Action action = null, params object[] obj)
        {

			EventSystem.S.Register(EventID.OnRefreshPanelInfo, HandlingListeningEvents);
			EventSystem.S.Register(EventID.OnRefreshRecruitmentOrder, HandlingListeningEvents);

			m_RecruitDiscipleMgr = MainGameMgr.S.RecruitDisciplerMgr;
			m_CurRecruitType = (RecruitType)obj[0];
			m_CurSprite = (Sprite)obj[1];

			m_RecruitDic.Add(RecruitType.SilverMedal, ClickType.First);
			m_RecruitDic.Add(RecruitType.GoldMedal, ClickType.First);


			InitFixedInfo();

			RefreshPanelInfo();

			BindAddListenerEvent();
		}

        private void RefreshPanelInfo()
        {
			m_IsFirstRecruitment = m_RecruitDiscipleMgr.GetIsFirstMedal(m_CurRecruitType);

			if (m_IsFirstRecruitment)
			{
				m_RecruitValue.text = Define.COMMON_DEFAULT_STR;
				m_RecruitmentBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_FREE);
			}
			else
			{
				int silverMedalCount = m_RecruitDiscipleMgr.GetCurRecruitCount(m_CurRecruitType);
				if (silverMedalCount>0)
                {
					m_RecruitDic[m_CurRecruitType] = ClickType.RecruitmentOrder;
					m_RecruitValue.text = silverMedalCount.ToString();
					m_RecruitmentBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_RECRUIT);
				}
                else
                {
					m_RecruitDic[m_CurRecruitType] = ClickType.LookAdvertisement;
					m_RecruitValue.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_TIMESTODAY);
					m_RecruitmentBtnValue.text = "看广告";
					//Todo 未完成
				}
			}
		}
		 
        private void BindAddListenerEvent()
        {
            switch (m_CurRecruitType)
            {
                case RecruitType.GoldMedal:
					m_RecruitmentBtn.onClick.AddListener(() => { OnClickRecruitBtn(RecruitType.GoldMedal); });
					break;
                case RecruitType.SilverMedal:
					m_RecruitmentBtn.onClick.AddListener(() => { OnClickRecruitBtn(RecruitType.SilverMedal); });
					break;
                default:
                    break;
            }
        }

		/// <summary>
		/// 招募点击事件
		/// </summary>
		private void OnClickRecruitBtn(RecruitType type)
		{
			switch (m_RecruitDic[type])
			{
				case ClickType.First:
					UIMgr.S.OpenPanel(UIID.RecruitmentPanel, type, ClickType.First);
					m_RecruitDic[type] = ClickType.RecruitmentOrder;
					break;
				case ClickType.RecruitmentOrder:
					UIMgr.S.OpenPanel(UIID.RecruitmentPanel, type, ClickType.RecruitmentOrder);
					break;
				case ClickType.LookAdvertisement:
					UIMgr.S.OpenPanel(UIID.RecruitmentPanel, type, ClickType.LookAdvertisement);
					break;
				case ClickType.Over:
					UIMgr.S.OpenPanel(UIID.LogPanel, "招募标题", "招募次数用尽");
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// 事件监听回调
		/// </summary>
		/// <param name="key"></param>
		/// <param name="param"></param>
		private void HandlingListeningEvents(int key, object[] param)
		{
			switch ((EventID)key)
			{
				case EventID.OnRefreshPanelInfo:
					RefreshPanelInfo();
					break;
				case EventID.OnRefreshRecruitmentOrder:
					RefreshRecruitmentOrder((RecruitType)param[0]);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// 刷新招募令数量
		/// </summary>
		/// <param name="recruitType"></param>
		private void RefreshRecruitmentOrder(RecruitType recruitType)
		{
			switch (recruitType)
			{
				case RecruitType.GoldMedal:
					//RecruitmentGoldCrder--;
					break;
				case RecruitType.SilverMedal:
					//RecruitmentSilverCrder--;
					break;
				default:
					break;
			}
		}

		private void InitFixedInfo()
        {
			m_Icon.sprite = m_CurSprite;
			switch (m_CurRecruitType)
            {
                case RecruitType.GoldMedal:
					m_RecruitmentOrderTitle.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_GOLDRECRUITMENT);
					m_RecruitmentOrderCont.text = RecruitmentOrderCont(RecruitType.GoldMedal);
					break;
                case RecruitType.SilverMedal:
					m_RecruitmentOrderTitle.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_SILVERRECRUITMENT);
					m_RecruitmentOrderCont.text = RecruitmentOrderCont(RecruitType.SilverMedal);
					break;
                default:
                    break;
            }

			
		}

		private string RecruitmentOrderCont(RecruitType Medal)
		{
            switch (Medal)
            {
                case RecruitType.GoldMedal:
					return CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_POSSIBLERECRUITMENT) +
						CommonUIMethod.GetStrForColor("#69471E", Define.FACILITY_LOBBY_ELITE) + Define.COMMA +
						CommonUIMethod.GetStrForColor("#69471E", Define.FACILITY_LOBBY_GENEIUS);
				case RecruitType.SilverMedal:
					return CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_POSSIBLERECRUITMENT) +
						CommonUIMethod.GetStrForColor("#384B76", Define.FACILITY_LOBBY_APPRENTICE) + Define.COMMA +
						CommonUIMethod.GetStrForColor("#69471E", Define.FACILITY_LOBBY_ELITE); ;
            }
			return "";
        }


		public void SetButtonEvent(Action<object> action)
        {
            throw new NotImplementedException();
        }


        private void OnDisable()
        {
			EventSystem.S.UnRegister(EventID.OnRefreshPanelInfo, HandlingListeningEvents);
			EventSystem.S.UnRegister(EventID.OnRefreshRecruitmentOrder, HandlingListeningEvents);
		}
    }
	
}