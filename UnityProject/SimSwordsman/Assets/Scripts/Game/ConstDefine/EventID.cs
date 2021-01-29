using UnityEngine;
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
        OnUnlockWorkSystem,
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
        OnArriveCollectResPos,
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

        #region 公用
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
        #endregion

    }

}
