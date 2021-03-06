using UnityEngine;
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
        /// <summary>
        /// 关闭所有UI面板，不包含mainmenu
        /// </summary>
        OnCloseAllUIPanel,

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
        OnUpgradeFacility,

        OnAddItem,
        OnReduceItems,


        //LivableRoom
        /// <summary>
        /// 升级按钮刷新事件 
        /// </summary>
        OnUpgradeRefreshEvent,

        // MainTask
        OnUnlockCommonTaskSystem,
        OnCommonTaskRefreshed,
        OnTaskManualFinished,
        //OnTaskFinished,
        OnCommonTaskStart,
        OnCommonTaskFinish,
        OnTaskObjCollected,
        OnArriveCollectResPos,
        OnCharacterTaskRewardClicked,
        OnAddCharacter,
        OnAddCharacterPanelClosed,
        //PracticeField

        /// <summary>
        /// 刷新坑位解锁信息
        /// </summary>
        OnRefreshPracticeUnlock,
        /// <summary>
        /// 限制相机的触摸移动
        /// </summary>
        OnLimitCameraTouchMove,
        #region 仓库
        /// <summary>
        /// 刷新仓库升级资源
        /// </summary>
        RefreshWarehouseRes,
        /// <summary>
        /// 仓库增加原材料
        /// </summary>
        OnRawMaterialChangeEvent,
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

        #region 弟子红点相关
        OnSubPanelRedPoint,
        OnKungfuRedPoint,
        OnEquipRedPoint,
        OnMainMenuOrDiscipleRedPoint,
        #endregion

        #region Deliver
        /// <summary>
        /// 打开选择弟子界面
        /// </summary>
        OnOpenChallengChoosePanel,
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
        OnBattleAtkStart,
        OnBattleAtkEnd,
        OnBattleAtkEvent,
        OnOneRoundEnd,
        OnCharacterInFightGroupDead,
        /// <summary>
        /// 战斗描述刷新
        /// </summary>
        OnBattleSecondEvent,

        //RecruitmentOrderRefresh
        OnGoldMedalRefresh,         //金牌招募刷新事件
        OnSilverMedalRefresh,       //银牌招募刷新事件
        OnRefreshPanelInfo,         //刷新招募面板信息
        OnRefreshRecruitmentOrder,  //刷新广告招募次数事件
        OnRecruitmentOrderIncrease,  //刷新广告招募次数事件

        //ChallengePanel
        OnChanllengeSuccess,        //挑战成功事件
        OnUnlockNewChapter,         //挑战成功事件
        /// <summary>
        /// 任务接受奖励列表
        /// </summary>
        OnReciveRewardList,
        /// <summary>
        /// 添加挑战奖励
        /// </summary>
        OnChallengeReward,

        //Disciple
        /// <summary>
        /// 弟子减少事件
        /// </summary>
        OnDiscipleReduce,
        /// <summary>
        /// 刷新弟子装备信息
        /// </summary>
        OnRefreshEquipInfo,
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
        /// 主菜单可挑战信息显示
        /// </summary>
        OnMainMenuChallenging,
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
        #region TacticalFunction
        OnDiscipleButtonOnClick,
        OnDeleteTaskBtn,
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

        OnGuideReceiveTaskRewardClickBtnTrigger1 = 2220901,
        OnGuideDialog5 = 10901,
        OnGuideBuildFacilityTrigger2 = 11001,
        OnGuideDialog6 = 11101,
        OnGuideClickLobbyTrigger2 = 11201,
        OnGuideDialog7 = 11301,
        OnCollectStoneTaskTrigger = 11401,
        OnGuideUnlockKitchen = 11501,
        OnGuideBuildKitchen = 11601,
        #region 任务面板
        OnRandomFightTrigger_IntroduceTrigger = 11701,//任务面板介绍
        #endregion
        BuildPracticeFieldEastTrigger = 11801,//建造练功房
        OnVisitorBtnNormalTipTrigger = 11901,//访客系统
        OnGuideUnlockWarehouse = 12001,
        OnRandomFightTrigger_ClickTaskBtnTrigger2,
        OnGuideReceiveTaskRewardClickBtnTrigger2,

        OnGuideDialog8,

        #region 食物不足引导
        OnFoodNotEnoughTrigger_IntroduceTrigger,
        OnFoodNotEnoughTrigger_ClickFoodBtnBrigger,
        #endregion

        OnGuideUnlockBaicaohu,
        OnGuideUnlockPracticeField,
        OnGuideUnlockKungfuLibrary,
        OnGuideUnlockForgeHouse,
        OnCuideKitchenFirstLvUp,
        OnGuideUnlockCollectSystem,
        OnCollectSystem_ClickLotusrootTrigger,//点击收集莲藕

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
        OnRandomFightTrigger_ClickAcceptBtn,

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


        OnCollectStoneProgressTaskTrigger,

        OnCloseFightingPanel,//关闭战斗面板

        OnClickVisitorBtnTrigger,//点击访客系统
        OnClickVisitorPanelAcceptBtnTrigger,//点击访客面板收下按钮
        OnFinishedClickWuWoodBubbleTrigger,//点击砍树气泡trigger完成
        OnDiscipleAutoWorkTrigger,//弟子自动工作trigger
        OnCreateVisitor,//添加访客item
        OnShowWorkBubble,//显示工作气泡（砍乌木和青岩）
        #region 装备武功秘籍
        OnKungFuTrigger_IntroduceTrigger,//武功秘籍trigger
        OnKungFuTrigger_ClickOpenDisciplePanelTrigger,//点击打开弟子界面
        OnKungFuTrigger_ChoiceTargetDiscipleTrigger,//选择目标弟子
        OnKungFuTrigger_ClickStudyKungFuTrigger,//点击学习武功秘籍
        OnKungFuTrigger_ChoiceKungFuTrigger,//选择武功秘籍 
        OnKungFuTrigger_ConfirmChoiceKungFuTrigger,//确认选择武功秘籍
        #endregion

        #region 装备武器
        OnArmsTrigger_IntroduceTrigger,//武器trigger
        OnArmsTrigger_ClickOpenDisciplePanelTrigger,//点击打开弟子界面
        OnArmsTrigger_ChoiceTargetDiscipleTrigger,//目标弟子选择
        OnArmsTrigger_ClickArmsBtnTrigger,//点击武器槽
        OnArmsTrigger_ChoiceArmsTrigger,//选择武器
        OnArmsTrigger_ConfirmChoiceArmsTrigger,//确定选择武器
        #endregion

        #region 挑战系统
        OnChallengeSystemTrigger_IntroduceTrigger,//挑战系统trigger
        OnChallengeSystemTrigger_ClickChallengeBtnTrigger1,//点击挑战系统按钮
        OnChallengeSystemTrigger_ChoiceChallengeObjTrigger,//选择挑战对象
        OnChallengeSystemTrigger_ChoiceChallengeLevelTrigger,//选择挑战关卡
        OnChallengeSystemTrigger_ClickAcceptChallengeBtnTrigger,//点击接受挑战
        OnChallengeSystemTrigger_ClickAKeyChoiceTrigger,//点击一键选择
        OnChallengeSystemTrigger_ClickStartChallengeTrigger,//点击开始战斗
        #endregion

        #region 招募系统
        OnRecruitmentSystem_IntroduceTrigger1 = 13301,
        OnRecruitmentSystem_IntroduceTrigger2,
        OnRecruitmentSystem_ClickLobbyFacilityTrigger,
        OnRecruitmentSystem_ClickGetCharacterTrigger,
        #endregion

        #region 签到系统
        OnSignInSystem_IntroduceTrigger = 13401,
        OnSignInSystem_ClickSignBtnTrigger,
        OnSignInSystem_ClickSignReceiveBtnTrigger,
        #endregion

        OnRecruitmentSystem_FinishedTrigger = 13501,
        OnSignInSystem_FinishedTrigger = 13601,

        RandomFightTrigger_FinishedIntroduce = 13701,
        OnShowMaskWithAlphaZeroPanel,//显示一个透明的遮罩面板，防止误触

        #endregion

        OnAddArmor,//获取新装备护甲
        OnAddArms,//获取信装备武器
        #region 定点计时
        OnFixedPointRefreshEvent,
        #endregion

        OnLoginWeChatSuccess,
        OnRefeshDailyTaskPanel,
        OnRefeshMainTask,
        OnRefeshDailyTask,
        OnMainMenuDailyTaskRedPoint,

        #region deliver system
        /// <summary>
        /// 角色到达镖车集合点
        /// </summary>
        OnCharacterReachDeliverCarGatherPoint,
        /// <summary>
        /// 镖车开始向外出发
        /// </summary>
        OnDeliverCarStartGoOut,
        /// <summary>
        /// 镖车开始回来
        /// </summary>
        OnDeliverCarStartComeBack,
        /// <summary>
        /// 镖车到达镖局时
        /// </summary>
        OnDeliverCarArrive,

        OnDeliverTrigger_BuildTrigger = 13801,          //镖局建造引导
        OnDeliverTrigger_ClickDeliverTrigger,           //点击镖局，进入镖局界面
        OnDeliverTrigger_ClickQuickStartBtnTrigger,     //点击快速出发按钮
        OnDeliverTrigger_ClickDoubleSpeedUpBtnTrigger,  //点击双倍加速按钮
        #endregion

        #region Tower
        OnRefeshTowerCoin,
        OnRefeshTowerShop,
        OnTowerBattleOver,

        OnTowerTrigger_IntroduceTrigger = 14001,        //伏魔塔系统介绍引导
        OnTowerTrigger_ClickTowerBtnTrigger,            //点击伏魔塔按钮
        OnTowerTrigger_IntroduceTrigger2,               //伏魔塔讲解
        OnTowerTrigger_ClickChallengeBtnTrigger,        //伏魔塔点击挑战按钮
        OnTowerTrigger_ClickAKeyChoiceBtnTrigger,       //伏魔塔点击一键选择弟子
        OnTowerTrigger_ClickStartFightBtnTrigger,       //伏魔塔 点击开始战斗按钮
        OnTowerTrigger_FightFinishedClickShopBtnTrigger,//伏魔塔_战斗结束_点击商店按钮
        OnTowerTrigger_SuccessTrigger,                  //伏魔塔挑战成功引导
        OnTowerTrigger_FaildTrigger,                    //伏魔塔挑战失败引导
        #endregion

        #region HeroTrial
        OnEnterHeroTrial,
        OnExitHeroTrial,
        OnRefreshTrialPanel,
        OnCountDownRefresh,
        OnEnableFinishBtn,
        OnTrialTimeOver,

        OnHeroTrialSystemTrigger_IntroduceTrigger,              //试炼系统介绍引导
        OnHeroTrialSystemTrigger_ClickTrialBtnTrigger,          //点击试炼按钮
        OnHeroTrialSystemTrigger_IntroduceTrigger2,             //试炼系统介绍引导2
        OnHeroTrialSystemTrigger_ClickStartBtnTrigger,          //试炼系统_点击前往按钮
        OnHeroTrialSystemTrigger_ClickChoiceDiscipleTrigger,    //试炼系统，选择弟子按钮
        OnHeroTrialSystemTrigger_ChoiceDiscipleBtnTrigger,      //试炼系统，选择弟子
        OnHeroTrialSystemTrigger_ClickEnterTrialBtnTrigger,     //试炼系统，点击进入试炼按钮

        OnHeroTrialSystemTrigger_ClickTrialFinishedTrigger,     //试炼系统，点击完成试炼
        OnHeroTrialSystemTrigger_DiscipleUpgradSuccessTrigger,  //试炼系统，弟子突破成功
        #endregion

        #region SelectedHeadPhoto
        OnRefreshHeadPhoto,
        OnRefreshSettingHeadPhoto,
        OnConfirmHeadPhoto,
        #endregion

        #region Arena
        OnRefeshArenaCoin,
        OnRefeshArenaShop,
        OnRefeshArenaChallengeCount,


        OnArenaSystemTrigger_IntroduceTrigger = 15001,             //竞技场系统介绍引导
        OnArenaSystemTrigger_ClickArenaBtnTrigger,                 //点击竞技场按钮
        OnArenaSystemTrigger_ClickChallengeBtnTrigger,             //竞技场点击挑战按钮s
        OnArenaSystemTrigger_ClickAKeyChoiceBtnTrigger,            //竞技场点击一键选择弟子
        OnArenaSystemTrigger_ClickStartFightBtnTrigger,            //竞技场点击选定开始战斗
        OnArenaSystemTrigger_FightFinishedClickShopBtnTrigger,     //竞技场_战斗结束_点击商城按钮
        #endregion
    }
}
