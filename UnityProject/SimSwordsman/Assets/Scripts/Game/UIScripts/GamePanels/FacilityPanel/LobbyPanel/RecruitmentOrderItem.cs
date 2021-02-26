using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class RecruitmentOrderItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Image m_Icon;
        [SerializeField]
        private Text m_RecruitmentOrderTitle;
        [SerializeField]
        private Image m_RecruitmentOrderTitleBg;
        [SerializeField]
        private Text m_RecruitmentOrderCont;
        [SerializeField]
        private Text m_RecruitValue;
        [SerializeField]
        private Text m_RecruitmentBtnValue;
        [SerializeField]
        private Button m_RecruitmentBtn;
        [SerializeField]
        private Image m_RecruitmentImg;

        private const int _12Hours = 12;
        private const int _24Hours = 24;
        private const int _48Hours = 48;
        //private const string SilverAdvertiseCount = "3";
        private const string AdvertiseCount = "1";

        private RecruitType m_CurRecruitType;
        private Sprite m_CurSprite;
        private AbstractAnimPanel m_Panel;
        private RecruitDiscipleMgr m_RecruitDiscipleMgr = null;
        private bool m_IsFirstRecruitment = false;
        private Dictionary<RecruitType, ClickType> m_RecruitDic = new Dictionary<RecruitType, ClickType>();
        private int m_Hours;

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            m_Panel = t as AbstractAnimPanel;

            EventSystem.S.Register(EventID.OnRefreshPanelInfo, HandlingListeningEvents);
            EventSystem.S.Register(EventID.OnRefreshRecruitmentOrder, HandlingListeningEvents);

            m_RecruitDiscipleMgr = MainGameMgr.S.RecruitDisciplerMgr;
            m_CurRecruitType = (RecruitType)obj[0];
            m_CurSprite = (Sprite)obj[1];
            m_RecruitDic[RecruitType.SilverMedal] = ClickType.Free;
            m_RecruitDic[RecruitType.GoldMedal] = ClickType.Free;

            m_Hours = GetDeltaTime(GameDataMgr.S.GetPlayerData().GetLobbyBuildTime());
            InitFixedInfo();

            //RefreshPanelInfo();
            switch (m_CurRecruitType)
            {
                case RecruitType.GoldMedal:
                    //int _48Count = m_Hours / _48Hours;
                    RefreshFreeRecruit();
                    gameObject.name = "RecruitmentOrderItem2";
                    break;
                case RecruitType.SilverMedal:
                    //int _24Count = m_Hours / _24Hours;
                    RefreshFreeRecruit();
                    gameObject.name = "RecruitmentOrderItem1";
                    break;
                default:
                    break;
            }

            BindAddListenerEvent();
        }

        private void RefreshPanelInfo()
        {
            m_IsFirstRecruitment = m_RecruitDiscipleMgr.GetIsFirstMedal(m_CurRecruitType);

            switch (m_RecruitDic[m_CurRecruitType])
            {
                case ClickType.None:
                    break;
                case ClickType.Free:
                    m_RecruitmentImg.gameObject.SetActive(false);
                    m_RecruitValue.text = Define.COMMON_DEFAULT_STR;
                    m_RecruitmentBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_FREE);
                    break;
                case ClickType.RecruitmentOrder:
                    m_RecruitmentImg.gameObject.SetActive(false);
                    m_RecruitValue.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_CURCOUNT) + MainGameMgr.S.RecruitDisciplerMgr.GetCurRecruitCount(m_CurRecruitType).ToString();
                    m_RecruitmentBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_RECRUIT);
                    break;
                case ClickType.LookAdvertisement:
                    m_RecruitmentImg.gameObject.SetActive(true);
                    RefreshAdvertiseInfo();
                    m_RecruitmentBtnValue.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_RECRUIT);
                    break;
                default:
                    break;
            }
        }

        private void RefreshAdvertiseInfo()
        {
            switch (m_CurRecruitType)
            {
                case RecruitType.GoldMedal:
                    m_RecruitValue.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_TIMESTODAY)
                    + m_RecruitDiscipleMgr.GetAdvertisementCount(m_CurRecruitType).ToString() + Define.SLASH
                    + AdvertiseCount;
                    break;
                case RecruitType.SilverMedal:
                    m_RecruitValue.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_TIMESTODAY)
                    + m_RecruitDiscipleMgr.GetAdvertisementCount(m_CurRecruitType).ToString() + Define.SLASH
                    + AdvertiseCount;
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 检测招募数据
        /// </summary>
        /// <param name="lobbyTime"></param>
        private void RefreshFreeRecruit()
        {
            int FreeCount = GameDataMgr.S.GetPlayerData().GetRecruitTimeType(m_CurRecruitType, RecruitTimeType.Free);
            if (FreeCount > 0)
            {
                m_RecruitDic[m_CurRecruitType] = ClickType.Free;
                RefreshPanelInfo();
                return;
            }
            int curRecruitCount = MainGameMgr.S.RecruitDisciplerMgr.GetCurRecruitCount(m_CurRecruitType);
            if (curRecruitCount > 0)
            {
                m_RecruitDic[m_CurRecruitType] = ClickType.RecruitmentOrder;
                RefreshPanelInfo();
                return;
            }

            m_RecruitDic[m_CurRecruitType] = ClickType.LookAdvertisement;
            int silverAdverCount = GameDataMgr.S.GetPlayerData().GetRecruitTimeType(m_CurRecruitType, RecruitTimeType.Advertisement);

            int count;
            if (m_CurRecruitType == RecruitType.GoldMedal)
                count = m_Hours / _48Hours;
            else
                count = m_Hours / _24Hours;
            if (count > silverAdverCount)
            {
                MainGameMgr.S.RecruitDisciplerMgr.ResetAdvertisementCount(m_CurRecruitType);
            }
            RefreshPanelInfo();
        }

        /// <summary>
        /// 获取当前时间到目标时间的时间数
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetDeltaTime(string time)
        {
            DateTime dateTime;
            DateTime.TryParse(time, out dateTime);
            if (dateTime != null)
            {
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks) - new TimeSpan(dateTime.Ticks);
                return (int)timeSpan.TotalHours;
            }
            return 0;
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
        /// 判断招募人数是否已满
        /// </summary>
        /// <returns></returns>
        private bool JudgeRecruitNumberOfPelpleFull()
        {
            int CurLevelMaxNumber = 0;
            int CurLevelNumber = MainGameMgr.S.CharacterMgr.GetAllCharacterList().Count;
            //for (int i = (int)FacilityType.LivableRoomEast1; i <= (int)FacilityType.LivableRoomWest4; i++)
            //    CurLevelMaxNumber+=TDFacilityLivableRoomTable.GetCapability(i, TDFacilityLivableRoomTable.GetMaxLevel(i));

            for (int i = (int)FacilityType.LivableRoomEast1; i <= (int)FacilityType.LivableRoomWest4; i++)
            {
                FacilityController facility = MainGameMgr.S.FacilityMgr.GetFacilityController((FacilityType)i);
                if (facility.GetState() == FacilityState.Unlocked)
                    CurLevelMaxNumber += TDFacilityLivableRoomTable.GetCapability(i, MainGameMgr.S.FacilityMgr.GetFacilityCurLevel((FacilityType)i));
            }

            if (CurLevelNumber >= CurLevelMaxNumber)
                return true;
            return false;
        }
    
        /// <summary>
        /// 招募点击事件
        /// </summary>
        private void OnClickRecruitBtn(RecruitType type)
        {
            AudioMgr.S.PlaySound(Define.SOUND_UI_BTN);
            if (JudgeRecruitNumberOfPelpleFull())
            {
                FloatMessage.S.ShowMsg("当前屋舍不足，请升级或建造新的屋舍");
                return;
            }
            switch (m_RecruitDic[type])
            {
                case ClickType.Free:
                    UIMgr.S.OpenPanel(UIID.GetDisciplePanel, GetRandomDisciples(type), ClickType.Free, type);
                    //UIMgr.S.OpenPanel(UIID.RecruitmentPanel, type, ClickType.Free);
                    break;
                case ClickType.RecruitmentOrder:
                    GetRandomDisciples(type);
                    UIMgr.S.OpenPanel(UIID.GetDisciplePanel, GetRandomDisciples(type), ClickType.RecruitmentOrder, type);
                    //UIMgr.S.OpenPanel(UIID.RecruitmentPanel, type, ClickType.RecruitmentOrder);
                    break;
                case ClickType.LookAdvertisement:   
                    int advertisementCount = MainGameMgr.S.RecruitDisciplerMgr.GetAdvertisementCount(type);
                    if (advertisementCount <= 0)
                        FloatMessage.S.ShowMsg("招募次数用尽");
                    //UIMgr.S.OpenPanel(UIID.LogPanel, "招募标题", "招募次数用尽");
                    else
                        UIMgr.S.OpenPanel(UIID.GetDisciplePanel, GetRandomDisciples(type), ClickType.LookAdvertisement, type);
                    //UIMgr.S.OpenPanel(UIID.RecruitmentPanel, type, ClickType.LookAdvertisement);
                    break;
                default:
                    break;
            }
        }

        private CharacterItem GetRandomDisciples(RecruitType recruitType)
        {
            switch (recruitType)
            {
                case RecruitType.GoldMedal:
                    CharacterItem chaGold = MainGameMgr.S.RecruitDisciplerMgr.GetRecruitForRecruitType(RecruitType.GoldMedal);
                    if (chaGold != null)
                        return chaGold;
                    return null;
                case RecruitType.SilverMedal:
                    CharacterItem chaSilver = MainGameMgr.S.RecruitDisciplerMgr.GetRecruitForRecruitType(RecruitType.SilverMedal);
                    if (chaSilver != null)
                        return chaSilver;
                    return null;
                default:
                    return null;
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
                    if (m_CurRecruitType == (RecruitType)param[0])
                        SetRecruitCount((ClickType)param[1]);
                    //m_RecruitDic[m_CurRecruitType] = (ClickType)param[0];
                    //RefreshPanelInfo();
                    break;
                case EventID.OnRefreshRecruitmentOrder:
                    //RefreshRecruitmentOrder((RecruitType)param[0]);
                    break;
                default:
                    break;
            }
        }

        private void SetRecruitCount(ClickType clickType)
        {
            int count;
            if (m_CurRecruitType == RecruitType.GoldMedal)
                count = m_Hours / _48Hours;
            else
                count = m_Hours / _24Hours;
            switch (clickType)
            {
                case ClickType.Free:
                    GameDataMgr.S.GetPlayerData().SetRecruitTimeType(m_CurRecruitType, RecruitTimeType.Free, 0);
                    break;
                case ClickType.LookAdvertisement:
                    GameDataMgr.S.GetPlayerData().SetRecruitTimeType(m_CurRecruitType, RecruitTimeType.Advertisement, count);
                    break;
                default:
                    break;
            }
            RefreshFreeRecruit();
        }

        private void InitFixedInfo()
        {
            m_Icon.sprite = m_CurSprite;
            switch (m_CurRecruitType)
            {
                case RecruitType.GoldMedal:
                    m_RecruitmentOrderTitle.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_GOLDRECRUITMENT);
                    m_RecruitmentOrderCont.text = RecruitmentOrderCont(RecruitType.GoldMedal);
                    m_RecruitmentOrderTitleBg.sprite = m_Panel.FindSprite("LobbyPanel_BgFont2");
                    break;
                case RecruitType.SilverMedal:
                    m_RecruitmentOrderTitle.text = CommonUIMethod.GetStringForTableKey(Define.FACILITY_LOBBY_SILVERRECRUITMENT);
                    m_RecruitmentOrderCont.text = RecruitmentOrderCont(RecruitType.SilverMedal);
                    m_RecruitmentOrderTitleBg.sprite = m_Panel.FindSprite("LobbyPanel_BgFont3");
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
        }


        private void OnDisable()
        {
            EventSystem.S.UnRegister(EventID.OnRefreshPanelInfo, HandlingListeningEvents);
            EventSystem.S.UnRegister(EventID.OnRefreshRecruitmentOrder, HandlingListeningEvents);
        }
    }

}