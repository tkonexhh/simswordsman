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
        OnUpdateLoadProgress,
        OnShowPopAdUI,
        OnStartAdEffect,
        OnEndAdEffect,
        OnTimeRefresh,
        OnGuidePanelOpen,
        GuideEventTrigger,
        OnSignSuccess,

        #region 新手引导
        OnFirstGuide,
        GuideDelayStart,


        #endregion

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
        OnAddCanWorkFacility,//可以干活的建筑
        OnAddWorkingRewardFacility,//可以获得干活奖励的建筑

        // Facility
        OnStartUnlockFacility,
        OnStartUpgradeFacility,
        OnEndUpgradeFacility,

        OnReduceItems,


        // MainTask
        OnTaskManualFinished,
        //OnTaskFinished,

        //PracticeField
        /// <summary>
        /// 选择弟子
        /// </summary>
        OnSelectDisciple,
        OnDisciplePracticeOver,
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
    }

}
