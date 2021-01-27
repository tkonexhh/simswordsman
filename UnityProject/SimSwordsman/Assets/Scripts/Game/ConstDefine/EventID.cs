﻿using UnityEngine;
using System.Collections;
using Qarth;

namespace GameWish.Game
{
    public enum EventID
    {
        OnLanguageTableSwitchFinish,
        OnAddCoinNum,
        OnReduceCoinNum,
        OnAddFoodNum,
        OnReduceFoodNum,
        OnClanNameChange,
        OnUpdateLoadProgress,
        OnShowPopAdUI,
        OnStartAdEffect,
        OnEndAdEffect,
        OnTimeRefresh,
        OnSignSuccess,

        //UI
        OnCloseParentPanel,
        OnCheckVisitorBtn,

        OnFoodBuffTick,//伙房食物增益消失倒计时
        OnFoodBuffEnd,//伙房食物增益结束
        OnFoodBuffStart,//伙房食物增益开始
        OnFoodBuffInterval,

        OnCountdownerStart,
        OnCountdownerTick,
        OnCountdownerEnd,

        OnCollectCountChange,//收集物
        OnChangeCollectLotusState1,
        OnChangeCollectLotusState2,
        OnAddCanWorkFacility,//可以干活的建筑
        OnAddWorkingRewardFacility,//可以获得干活奖励的建筑

        // Facility
        OnStartUnlockFacility,
        OnStartUpgradeFacility,
        OnEndUpgradeFacility,

        OnAddItem,
        OnReduceItems,


        // MainTask
        OnTaskManualFinished,
        //OnTaskFinished,
        OnCommonTaskStart,
        OnCommonTaskFinish,

        //PracticeField

        /// <summary>
        /// 刷新坑位解锁信息
        /// </summary>
        OnRefreshPracticeUnlock,
        /// <summary>
        /// 练功场倒计时刷新
        /// </summary>
        OnPracticeFieldTimeRefresh,

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
        OnSelectedEquipSuccess,     //挑战成功事件

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
        /// 功夫升级事件
        /// </summary>
        OnKongfuLibraryUpgrade,
        /// <summary>
        /// 刷新弟子面板
        /// </summary>
        OnRefreshDisciple,

        #region 公告榜
        /// <summary>
        /// 选择弟子
        /// </summary>
        OnSelectedDiscipleEvent,
        /// <summary>
        /// 选择弟子确定事情
        /// </summary>
        OnSelectedConfirmEvent,
        /// <summary>
        /// 打开面板发送弟子事件
        /// </summary>
        OnSendDiscipleDicEvent,
        OnSendHerbEvent,
        #endregion


        #region 新手引导
        OnGuideDialog1 = 10001,
        OnGuideTakeName,
        OnGuideDialog2,
        OnGuideBuildFacilityTrigger1,
        OnGuideDialog3,
        OnGuideClickLobbyTrigger1,
        OnGuideClickTaskTrigger1 = 10008,

        OnGuideReceiveTaskRewardClickBtnTrigger1,
        OnGuideDialog5,
        OnGuideBuildFacilityTrigger2,
        OnGuideDialog6,
        OnGuideClickLobbyTrigger2,
        OnGuideClickTaskTrigger2 = 10015,
        OnGuideReceiveTaskRewardClickBtnTrigger2,
        OnGuideUnlockKitchen,
        OnGuideBuildKitchen,
        OnGuideDialog8,

        OnGuideUnlockWarehouse = 10100,
        OnGuideDialog4,
        OnGuideDialog7,
        OnGuideUnlockBaicaohu,
        OnGuideUnlockPracticeField,
        OnGuideUnlockKungfuLibrary,
        OnGuideUnlockForgeHouse,
        OnCuideKitchenFirstLvUp,
        OnGuideUnlockCollectSystem,

        OnGuideReceiveTaskRewardTrigger2,
        OnGuideReceiveTaskRewardTrigger1,

        OnGuideBuildFacilityPanelTrigger1,
        OnGuideClickRecruitTrigger1,
        OnGuideClickTaskDetailsTrigger1,
        OnGuideSelectCharacterTrigger1,
        OnGuideSendCharacterOnTaskTrigger1,
        OnGuideBuildFacilityPanelTrigger2,
        OnGuideClickRecruitTrigger2,
        OnGuideClickTaskDetailsTrigger2,
        OnGuideSelectCharacterTrigger2,
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
        #endregion

    }

}
