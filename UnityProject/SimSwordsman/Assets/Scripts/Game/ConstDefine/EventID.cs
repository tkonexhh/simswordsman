﻿using UnityEngine;
using System.Collections;
using Qarth;

namespace GameWish.Game
{
    public enum EventID
    {
        OnLanguageTableSwitchFinish,
        OnRefreshMainMenuPanel,
        OnReduceFood,
        OnAddFood,
        OnClanNameChange,
        OnUpdateLoadProgress,
        OnShowPopAdUI,
        OnStartAdEffect,
        OnEndAdEffect,
        OnTimeRefresh,
        OnSignSuccess,
        OpenAbstractPanel,
        //UI
        OnCloseParentPanel,
        OnCheckVisitorBtn,

        OnFoodBuffTick,//伙房食物增益消失倒计时
        OnFoodBuffEnd,//伙房食物增益结束
        OnFoodBuffStart,//伙房食物增益开始
        OnFoodBuffInterval,
        /// <summary>
        /// 包子刷新事件
        /// </summary>
        OnFoodRefreshEvent,

        OnCountdownerStart,
        OnCountdownerTick,
        OnCountdownerEnd,

        OnCollectCountChange,//收集物
        OnChangeCollectLotusState1,
        OnChangeCollectLotusState2,
        OnUnlockWorkSystem,
        OnAddCanWorkFacility,//可以干活的建筑
        OnAddWorkingRewardFacility,//可以获得干活奖励的建筑

        // Facility
        OnStartUnlockFacility,
        OnStartUpgradeFacility,
        OnEndUpgradeFacility,

        OnAddItem,
        OnReduceItems,


        //LivableRoom
        /// <summary>
        /// 升级按钮刷新事件 
        /// </summary>
        OnUpgradeRefreshEvent,



        // MainTask
        OnTaskManualFinished,
        //OnTaskFinished,
        OnCommonTaskStart,
        OnCommonTaskFinish,
        OnTaskObjCollected,
        OnArriveCollectResPos,
        OnCharacterTaskRewardClicked,
        //PracticeField

        /// <summary>
        /// 刷新坑位解锁信息
        /// </summary>
        OnRefreshPracticeUnlock,
        /// <summary>
        /// 练功场倒计时刷新
        /// </summary>
        OnPracticeFieldTimeRefresh,

        #region 仓库
        /// <summary>
        /// 刷新仓库升级资源
        /// </summary>
        RefreshWarehouseRes,
        /// <summary>
        /// 仓库增加原材料
        /// </summary>
        OnAddRawMaterialEvent,
        #endregion

        #region 感叹号相关
        /// <summary>
        /// 发送头上有工作气泡的设施
        /// </summary>
        OnSendWorkingBubbleFacility,
        /// <summary>
        /// 公告榜任务
        /// </summary>
        OnSendBulletinBoardFacility,
        /// <summary>
        /// 可招募的时候惊叹号
        /// </summary>
        OnSendRecruitable,
        #endregion

        //KungfuLibrary
        /// <summary>
        /// 刷新功夫坑位的信息
        /// </summary>
        OnRefresKungfuSoltInfo,

        //PatrolRoom
        OnRefresPatrolSoltInfo,

        //Battle
        OnEnterBattle,
        OnExitBattle,
        OnRefreshBattleProgress,
        OnBattleFailed,
        OnBattleSuccessed,
        OnBattleMoveEnd,
        OnBattleAtkEnd,
        OnBattleAtkEvent,

        //RecruitmentOrderRefresh
        OnGoldMedalRefresh,         //金牌招募刷新事件
        OnSilverMedalRefresh,       //银牌招募刷新事件
        OnRefreshPanelInfo,         //刷新招募面板信息
        OnRefreshRecruitmentOrder,  //刷新广告招募次数事件
        OnRecruitmentOrderIncrease,  //刷新广告招募次数事件

        //ChallengePanel
        OnChanllengeSuccess,        //挑战成功事件
        OnUnlockNewChapter,         //挑战成功事件

        //Disciple
        /// <summary>
        /// 弟子减少事件
        /// </summary>
        OnDiscipleReduce,
        /// <summary>
        /// 弟子升段事件
        /// </summary>
        OnCharacterUpgrade,
        /// <summary>
        /// 弟子升级
        /// </summary>
        OnCharacterUpLevel,
        /// <summary>
        /// 功夫升级事件
        /// </summary>
        OnKongfuLibraryUpgrade,
        /// <summary>
        /// 刷新弟子面板
        /// </summary>
        OnRefreshDisciple,
        /// <summary>
        /// 装备选择
        /// </summary>
        OnSelectedEquipEvent,
        /// <summary>
        /// 装备选择成功
        /// </summary>
        OnSelectedEquipSuccess,
        /// <summary>
        /// 功夫选择
        /// </summary>
        OnSelectedKungfuEvent,
        /// <summary>
        /// 功夫选择成功
        /// </summary>
        OnSelectedKungfuSuccess,
        /// <summary>
        /// 获取到功夫秘籍
        /// </summary>
        OnGetKungFu,

        /// <summary>
        /// 收起公告榜其他面板
        /// </summary>
        OnStowPanelEvent,
        #region 公用
        /// <summary>
        /// 选择
        /// </summary>
        OnSelectedEvent,
        /// <summary>
        /// 选择确定事情
        /// </summary>
        OnSelectedConfirmEvent,
        OnBulletinSelectedConfirmEvent,
        /// <summary>
        /// 打开面板发送弟子事件
        /// </summary>
        OnSendDiscipleDicEvent,
        OnBulletinSendDiscipleDicEvent,
        /// <summary>
        /// 删除
        /// </summary>
        DeleteDisciple,
        OnSendHerbEvent,
        #endregion


        #region 新手引导
        OnGuideDialog1 = 10101,
        OnGuideTakeName = 10201,
        OnGuideDialog2 = 10301,
        OnGuideBuildFacilityTrigger1 = 10401,
        OnGuideDialog3 = 10501,
        OnGuideClickLobbyTrigger1 = 10601,
        OnGuideDialog4 = 10701,
        OnGuideClickTaskTrigger1 = 10801,

        OnGuideReceiveTaskRewardClickBtnTrigger1 = 10901,
        OnGuideDialog5 = 11001,
        OnGuideBuildFacilityTrigger2 = 11101,
        OnGuideDialog6 = 11201,
        OnGuideClickLobbyTrigger2 = 11301,
        OnGuideDialog7 = 11401,
        OnGuideClickTaskTrigger2 = 11501,
        OnGuideReceiveTaskRewardClickBtnTrigger2 = 11601,
        OnGuideUnlockKitchen = 11701,
        OnGuideBuildKitchen = 11801,
        OnGuideDialog8 = 11901,

        OnGuideUnlockWarehouse = 20000,
        OnGuideUnlockBaicaohu,
        OnGuideUnlockPracticeField,
        OnGuideUnlockKungfuLibrary,
        OnGuideUnlockForgeHouse,
        OnCuideKitchenFirstLvUp,
        OnGuideUnlockCollectSystem,

        OnGuideReceiveTaskRewardBtnTrigger1,
        OnGuideReceiveTaskRewardTrigger1,
        OnGuideReceiveTaskRewardBtnTrigger2,
        OnGuideReceiveTaskRewardTrigger2,

        OnGuideBuildFacilityPanelTrigger1,
        OnGuideClickRecruitTrigger1,
        OnGuideClickTaskDetailsTrigger1,
        OnGuideSelectCharacterBtnTrigger1,
        OnGuideSelectCharacterPanelTrigger1,
        OnGuideSelectCharacterSureTrigger1,
        OnGuideSendCharacterOnTaskTrigger1,
        OnGuideBuildFacilityPanelTrigger2,
        OnGuideClickRecruitTrigger2,
        OnGuideClickTaskDetailsTrigger2,
        OnGuideSelectCharacterTrigger2,

        OnGuideSelectCharacterPanelTrigger2_1,
        OnGuideSelectCharacterPanelTrigger2_2,
        OnGuideSelectCharacterSureTrigger2,

        OnGuideSendCharacterOnTaskTrigger2,
        OnGuideBuildKitchenPanel,
        OnGuideSendCharacterWorkTrigger,
        OnGuideBuildWarehouse,
        OnGuideBuildWarehousePanel,

        OnGuidePanelOpen,
        GuideDelayStart,
        InGuideProgress,
        OnGuideFirstGetCharacter,
        OnGuideSecondGetCharacter,

        OnCollectStoneTaskTrigger,
        OnCollectStoneProgressTaskTrigger,
        BuildPracticeFieldEastTrigger,//建造练功房
        OnCloseFightingPanel,//关闭战斗面板
        OnVisitorBtnNormalTipTrigger,//访客系统
        OnClickVisitorBtnTrigger,//点击访客系统
        OnClickVisitorPanelAcceptBtnTrigger,//点击访客面板收下按钮
        OnKungFuTrigger_IntroduceTrigger,//武功秘籍trigger
        OnKungFuTrigger_ClickOpenDisciplePanelTrigger,//点击打开弟子界面
        OnKungFuTrigger_ChoiceTargetDiscipleTrigger,//选择目标弟子
        OnKungFuTrigger_ClickStudyKungFuTrigger,//点击学习武功秘籍
        OnKungFuTrigger_ChoiceKungFuTrigger,//选择武功秘籍 
        OnKungFuTrigger_ConfirmChoiceKungFuTrigger,//确认选择武功秘籍
        #endregion


        OnAddArmor,//获取新装备护甲
        OnAddArms,//获取信装备武器
    }

}
