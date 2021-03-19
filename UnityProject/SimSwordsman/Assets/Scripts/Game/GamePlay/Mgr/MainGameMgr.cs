using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using System;

namespace GameWish.Game
{
    public class MainGameMgr : TMonoSingleton<MainGameMgr>, IMgr
    {
        private FacilityMgr m_FacilityMgr = null;
        private InventoryMgr m_InventoryMgr = null;
        private CharacterMgr m_CharacterMgr = null;
        private KongfuLibraryMgr m_KongfuLibraryMgr = null;
        private MainTaskMgr m_MainTaskMgr = null;
        private CommonTaskMgr m_CommonTaskMgr = null;
        private ChapterMgr m_ChapterMgr = null;
        private RecruitDiscipleMgr m_RecruitDisciplerMgr = null;
        private MainCamera m_MainCamera = null;
        private BattleFieldMgr m_BattleFieldMgr = null;
        private RawMatCollectSystem m_RawMatCollectSystem = null;
        //private MedicinalPowderMgr m_MedicinalPowderMgr = null;

        public FacilityMgr FacilityMgr { get => m_FacilityMgr; }
        public CharacterMgr CharacterMgr { get => m_CharacterMgr; }
        public InventoryMgr InventoryMgr { get => m_InventoryMgr; }
        public KongfuLibraryMgr KongfuLibraryMgr { get => m_KongfuLibraryMgr; }
        public MainTaskMgr MainTaskMgr { get => m_MainTaskMgr; }
        public CommonTaskMgr CommonTaskMgr { get => m_CommonTaskMgr; }
        public ChapterMgr ChapterMgr { get => m_ChapterMgr; }
        public RecruitDiscipleMgr RecruitDisciplerMgr { get => m_RecruitDisciplerMgr; }
        public MainCamera MainCamera { get => m_MainCamera; }
        public BattleFieldMgr BattleFieldMgr { get => m_BattleFieldMgr; }
        //public MedicinalPowderMgr MedicinalPowderMgr { get => m_MedicinalPowderMgr; }
        public RawMatCollectSystem RawMatCollectSystem { get => m_RawMatCollectSystem; }

        private bool m_IsInited = false;

        #region IMgr
        public void OnInit()
        {
            EventSystem.S.Register(EventID.OnShowMaskWithAlphaZeroPanel, OnShowMaskWithAlphaZeroPanelCallBack);
            m_CharacterMgr = gameObject.AddComponent<CharacterMgr>();
            m_CharacterMgr.OnInit();

            m_CharacterMgr.InitCharacterDataWrapper();

            m_FacilityMgr = gameObject.AddComponent<FacilityMgr>();
            m_FacilityMgr.OnInit();

            m_InventoryMgr = gameObject.AddComponent<InventoryMgr>();
            m_InventoryMgr.OnInit();

            m_KongfuLibraryMgr = gameObject.AddComponent<KongfuLibraryMgr>();
            m_KongfuLibraryMgr.OnInit();

            m_MainTaskMgr = gameObject.AddComponent<MainTaskMgr>(); // ��ʱû��
            //m_MainTaskMgr.OnInit();

            m_CommonTaskMgr = gameObject.AddComponent<CommonTaskMgr>();
            m_CommonTaskMgr.OnInit();

            m_ChapterMgr = gameObject.AddComponent<ChapterMgr>();
            m_ChapterMgr.OnInit();

            m_RecruitDisciplerMgr = gameObject.AddComponent<RecruitDiscipleMgr>();
            m_RecruitDisciplerMgr.OnInit();

            m_RawMatCollectSystem = gameObject.AddComponent<RawMatCollectSystem>();
            m_RawMatCollectSystem.OnInit();

            //m_MedicinalPowderMgr = gameObject.AddComponent<MedicinalPowderMgr>();
            //m_MedicinalPowderMgr.OnInit();

            m_MainCamera = FindObjectOfType<MainCamera>();
            m_MainCamera.OnInit();

            m_BattleFieldMgr = gameObject.AddComponent<BattleFieldMgr>();
            m_BattleFieldMgr.OnInit();

            m_CharacterMgr.InitData();

            m_CommonTaskMgr.InitTaskList();

            m_FacilityMgr.InitFacilityControllers();

            m_RawMatCollectSystem.InitState();

            m_IsInited = true;
            EventSystem.S.Send(EventID.OnRawMaterialChangeEvent);
        }
        private int m_TimerID = -1;
        //TODO:后续修改
        private void OnShowMaskWithAlphaZeroPanelCallBack(int key, object[] param)
        {
            Timer.S.Cancel(m_TimerID);
            UIMgr.S.ClosePanelAsUIID(UIID.MaskWithAlphaZeroPanel);
            UIMgr.S.OpenTopPanel(UIID.MaskWithAlphaZeroPanel, null);
            m_TimerID = Timer.S.Post2Really((x) =>
            {
                UIMgr.S.ClosePanelAsUIID(UIID.MaskWithAlphaZeroPanel);
                m_TimerID = -1;
            }, 0.4f, 1);
        }

        public void OnUpdate()
        {
            if (m_IsInited == false)
                return;

            m_BattleFieldMgr?.OnUpdate();
            CharacterMgr?.OnUpdate();
            m_RawMatCollectSystem?.OnUpdate();
            m_CommonTaskMgr?.OnUpdate();
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    GameDataMgr.S.GetMainTaskData().SetTaskFinished(1);
            //    EventSystem.S.Send(EventID.OnTaskManualFinished);
            //}
        }

        public void OnDestroyed()
        {

        }
        #endregion

        //private void Update()
        //{
        //    OnUpdate();
        //}
    }

}