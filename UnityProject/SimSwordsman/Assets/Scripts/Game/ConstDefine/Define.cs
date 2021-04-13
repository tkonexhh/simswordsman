using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class Define
    {
        public const int DEFAULT_COIN_NUM = 500;
        public const int DEFAULT_FOOD_NUM = 0;
        public const int DEFAULT_TIP_NUM = 5;

        public const string DEFAULT_SOUND = "";
        public const string DEFAULT_NUMBER_ZERO = "0";

        public const string NAME_SPACE_PREFIX = "GameWish.Game.";
        //offline
        public const int OFFLINE_MAX_TIME = 120;
        public const int OFFLINE_MIN_TIME = 120;
        public const int OFFLINE_RATE_MIN = 2;
        public const int OFFLINE_RATE_MAX = 6;

        //offline earning
        public const int OFFLINE_MONEY_RATE = 1;
        public const int OFFLINE_EXP_RATE = 30;
        public const int OFFLINE_DIAMOND_COST = 100;

        //rate record
        public const string RATE_RECORD = "rate";


        //log out 
        public const string LogoutKey = "m_LogoutKey";


        public const string AD_PLACEMENT_INTER = "";
        public const string SLASH = "/";
        public const string PLUS = "+";
        public const string RIDE = "x";
        public const string SPACE = " ";
        public const string COMMA = ",";
        public const string PERCENT = "%";

        public const int AD_MAX_TIME = 120;

        //Common
        public const string COMMON_UPGRADERESOURCES = "Common_UpgradeResources";
        public const string COMMON_BUILDRESOURCES = "Common_BuildResources";
        public const string COMMON_UPGRADEINFODESC = "Common_UpgradeInfoDesc";
        public const string COMMON_UPGRADENEEDS = "Common_UpgradeNeeds";
        public const string COMMON_BUILDINFODESC = "Common_BuildInfoDesc";
        public const string COMMON_INFODESC = "Common_InfoDesc";
        public const string COMMON_UPGRADE = "Common_Upgrade";
        public const string COMMON_BUILD = "Common_Build";
        public const string COMMON_NOTBUILD = "Common_NotBuild";
        public const string COMMON_NOTUNLOCKED = "Common_NotUnlocked";
        public const string COMMON_UNLOCKED = "Common_Unlocked";
        public const string COMMON_FULLEDLEVEL = "Common_FulledLevel";
        public const string COMMON_FULLLEVEL = "Common_FullLevel";

        public const string COMMON_UNIT_GRADE = "Common_Unit_Grade";
        public const string COMMON_UNIT_PEOPLE = "Common_Unit_People";
        public const string COMMON_UNIT_DAY = "Common_Unit_Day";
        public const string COMMON_UNIT_PAET = "Common_Unit_Part";
        public const string COMMON_UNIT_CLASS = "Common_Unit_Class";
        public const string COMMON_UNIT_INDIVIDUAL = "Common_Unit_Individual";
        public const string COMMON_UNIT_ONLY = "Common_Unit_Only";
        public const string COMMON_UNIT_LAYER = "Common_Unit_Layer";
        public const string COMMON_UNIT_TENTHOUSAND = "Common_Unit_TenThousand";
        public const string COMMON_NEXTLEVELUNLOCK = "Common_NextLevelUnlock";
        public const string COMMON_REWARD = "Common_Reward";
        public const string COMMON_DECLINED = "Common_Declined";
        public const string COMMON_GOTO = "Common_GoTo";
        public const string COMMON_POPUP_NEEDLOBBY = "Common_Popup_NeedLobby";
        public const string COMMON_POPUP_MATERIALS = "Common_Popup_Materials";
        public const string COMMON_POPUP_COIN = "Common_Popup_Coin";

        //公用数字
        public const string COMMON_NUMBER_ZERO = "Common_Number_Zero";
        public const string COMMON_NUMBER_ONE = "Common_Number_One";
        public const string COMMON_NUMBER_TWO = "Common_Number_Two";
        public const string COMMON_NUMBER_THREE = "Common_Number_Three";
        public const string COMMON_NUMBER_FOUR = "Common_Number_Four";
        public const string COMMON_NUMBER_FIVE = "Common_Number_Five";
        public const string COMMON_NUMBER_SIX = "Common_Number_Six";
        public const string COMMON_NUMBER_SEVENT = "Common_Number_Seven";
        public const string COMMON_NUMBER_EIGHT = "Common_Number_Eight";
        public const string COMMON_NUMBER_NINE = "Common_Number_Nine";
        public const string COMMON_NUMBER_TEN = "Common_Number_Ten";

        public const string COMMON_DEFAULT_STR = "";

        // Character
        public const int CHARACTER_NORAML_MAX_LEVEL = 250;
        public const int CHARACTER_GOOD_MAX_LEVEL = 350;
        public const int CHARACTER_EXCELLENT_MAX_LEVEL = 450;
        public const int CHARACTER_MAX_LEVEL = 1000;
        public const string CHARACTER_TASK_REWARD_BUBBLE = "CharacterTaskRewardBubble";
        public const string CHARACTER_WORK_PROGRESS_BAR = "WorkProgressBar";
        public const string CHARACTER_WORK_TIP = "WorkTip";
        public const string CHARACTER_WORK_REWARD_POP = "PopRewardCanvas";
        //public const string CHARACTER_POP_REWARD_CANVAS = "PopRewardCanvas";

        // Animation
        public const string ANIM_MOVE = "move";
        public const string ANIM_ATTACK = "attack";
        public const string ANIM_HUNTING = "hunting";
        public const string ANIM_IDLE = "idle";
        public const string ANIM_LUMBERING = "lumbering";
        public const string ANIM_MINING = "mining";
        public const string ANIM_PRACTICE = "practice";
        public const string ANIM_TUMBLE = "tumble";
        public const string ANIM_TUMBLE_GETUP = "tumble_get_up";

        // Facility
        public const int FACILITY_MAX_PRACTIVEFIELD = 6;
        public const int FACILITY_MAX_LOBBY = 8;
        public const int FACILITY_MAX_KUNGFULIBRARY = 5;
        public const int FACILITY_MAX_LIVABLEROOM = 6;
        public const int FACILITY_MAX_BULLETInBOARD = 10;
        public const int FACILITY_MAX_WAREHOUSE = 5;
        public const int FACILITY_MAX_BAICAOHU = 4;
        public const int FACILITY_MAX_DELIVER = 4;
        public const int FACILITY_MAX_KITCHEN = 5;
        public const int FACILITY_MAX_FORGEHOUSE = 6;
        public const int FACILITY_MAX_TOTALCOUNT = 10;

        // Chapter
        public const int MAX_CHAPTER_COUNT = 5;
        public const int LEVEL_COUNT_PER_CHAPTER = 2000;

        // Inventory
        public const int MAX_PROP_COUNT = 99999;

        //LobbyPanel
        public const string FACILITY_LOBBY_GOLDRECRUITMENT = "Facility_Lobby_GoldRecruitment";
        public const string FACILITY_LOBBY_SILVERRECRUITMENT = "Facility_Lobby_SilverRecruitment";
        public const string FACILITY_LOBBY_POSSIBLERECRUITMENT = "Facility_Lobby_PossibleRecruitment";
        public const string FACILITY_LOBBY_APPRENTICE = "Facility_Lobby_Apprentice";
        public const string FACILITY_LOBBY_ELITE = "Facility_Lobby_Elite";
        public const string FACILITY_LOBBY_GENEIUS = "Facility_Lobby_Genius";
        public const string FACILITY_LOBBY_TIMESTODAY = "Facility_Lobby_TimesToday";
        public const string FACILITY_LOBBY_FREE = "Facility_Lobby_Free";
        public const string FACILITY_LOBBY_RECRUIT = "Facility_Lobby_Recruit";
        public const string FACILITY_LOBBY_CURCOUNT = "Facility_Lobby_CurCount";



        //ChallengePanel
        public const string CHALLENGE_NAME = "ChallengeName";
        public const string CHALLENGE_DESCRIBE = "ChallengeDescribe";
        public const string CHALLENGE_STATUE_COMPLETED = "Challenge_Status_Completed";
        public const string CHALLENGE_STATUE_UNLOCKED = "Challenge_Status_Unlocked";
        public const string CHALLENGE_LOG_TITLE = "Challenge_Log_Title";
        public const string CHALLENGE_LOG_CONTENT = "Challenge_Log_Content";
        public const string CHALLENGE_LOG_ACCEPTBTNTXT = "Challenge_Log_AcceptBtnTxt";
        public const string CHALLENGE_LOG_REFUSEBTNTXT = "Challenge_Log_RefuseBtnTxt";
        public const string CHALLENGE_PROGRESS = "Challenge_Progress";
        public const string CHALLENGE_PROGRESS_OVER = "Challenge_Progress_Over";
        public const string Is_Enter_Challenge_Panel = "IsEnterChallengePanel";

        //Warehouse
        public const string FACILITY_WAREHOUSE_NAME = "Facility_Warehouse_Name";
        public const string FACILITY_WAREHOUSE_DESCRIBE = "Facility_Warehouse_Describe";
        public const string FACILITY_WAREHOUSE_INDIVIDUAL = "Facility_Warehouse_Individual";

        //Disciple
        public const string DISCIPLE_NAME = "Disciple_Name";
        public const string DISCIPLE_DESCRIBE = "Disciple_Describe";
        public const string DISCIPLE_QUALITY_NORMAL = "Disciple_Quality_Normal";
        public const string DISCIPLE_NAME_GOOD = "Disciple_Quality_Good";
        public const string DISCIPLE_NAME_PREFECT = "Disciple_Quality_Perfect";
        public const string DISCIPLE_TITLE_LEVEL = "Disciple_Title_Level";
        public const string DISCIPLE_TITLE_SKILL = "Disciple_Title_Skill";
        public const string DISCIPLE_TITLE_ENTRYTIME = "Disciple_Title_EntryTime";
        public const string DISCIPLE_TITLE_RANK = "Disciple_Title_Rank";
        public const string DISCIPLE_STATE_WORKING = "Disciple_State_Working";
        public const string DISCIPLE_STATE_FREE = "Disciple_State_Free";
        public const string DISCIPLE_PRACTICE = "Disciple_Practice";
        public const string DISCIPLE_WORK = "Disciple_Work";
        public const string DISCIPLE_EJECT = "Disciple_Eject";
        public const string DISCIPLE_BTNVALUE_NORMAL = "Disciple_BtnValue_Normal";
        public const string DISCIPLE_BTNVALUE_GOOD = "Disciple_BtnValue_Good";
        public const string DISCIPLE_BTNVALUE_PREFECT = "Disciple_BtnValue_Perfect";
        public const string DISCIPLE_BTNVALUE_ALL = "Disciple_BtnValue_All";

        //LivableRoom
        public const string FACILITY_LIVABLEROOMEAST_NAME = "Facility_LivableRoomEast_Name";
        public const string FACILITY_LIVABLEROOMWEST_NAME = "Facility_LivableRoomWest_Name";
        public const string FACILITY_LIVABLEROOM_DESCRIBLE = "Facility_LivableRoom_Describe";
        public const string FACILITY_LIVABLEROOM_CURRENTLYHABITABLE = "Facility_LivableRoom_CurrentlyHabitable";
        public const string FACILITY_LIVABLEROOM_NEXTHABITABLE = "Facility_LivableRoom_NextHabitable";



        //PracticeField
        public const string FACILITY_PRACTICEFIELDEAST_NAME = "Facility_PracticeFieldEast_Name";
        public const string FACILITY_PRACTICEFIELDWEST_NAME = "Facility_PracticeFieldWest_Name";
        public const string FACILITY_PRACTICEFIELD_DESCRIBLE = "Facility_PracticeField_Describe";

        //KongfuLibrary
        public const string FACILITY_KONGFULIVRARY_NAME = "Facility_KongfuLibrary_Name";
        public const string FACILITY_KONGFULIVRARY_DESCRIBLE = "Facility_KongfuLibrary_Describe";

        //Kitchen
        public const string FACILITY_KITCHEN_NAME = "Facility_Kitchen_Name";
        public const string FACILITY_KITCHEN_DESCRIBLE = "Facility_Kitchen_Describe";

        //BaiCaoHu
        public const string FACILITY_BAICAOHU_NAME = "Facility_BaiCaoHu_Name";
        public const string FACILITY_BAICAOHU_DESCRIBLE = "Facility_BaiCaoHu_Describe";

        //ForgeHouse
        public const string FACILITY_FORGEHOUSE_NAME = "Facility_ForgeHouse_Name";
        public const string FACILITY_FORGEHOUSE_DESCRIBLE = "Facility_ForgeHouse_Describe";

        //BulletinBoard
        public const string BULLETINBOARD_VIEWDETAILS = "BulletinBoard_ViewDetails";
        public const string BULLETINBOARD_RECEIVEREWARDS = "BulletinBoard_ReceiveRewards";
        public const string BULLETINBOARD_PUTAWAY = "BulletinBoard_PutAway";
        public const string BULLETINBOARD_RECOMMENDED = "BulletinBoard_Recommended";
        public const string BULLETINBOARD_SELECTEDDISCIPLEY = "BulletinBoard_SelectedDisciple";
        public const string BULLETINBOARD_SELECTEDDISCIPLEYSKILLS = "BulletinBoard_SelectedDiscipleSkills";
        public const string BULLETINBOARD_NOTARRANGED = "BulletinBoard_NotArranged";
        public const string BULLETINBOARD_RELAXED = "BulletinBoard_Relaxed";
        public const string BULLETINBOARD_AUTIOUS = "BulletinBoard_Autious";
        public const string BULLETINBOARD_DANGER = "BulletinBoard_Danger";
        public const string BULLETINBOARD_NEEDLEVEL = "BulletinBoard_NeedLevel";
        public const string BULLETINBOARD_REWARD = "BulletinBoard_Reward";

        //Kungfo
        public const string KUNGFU_TITLE = "Kungfu_Title";
        public const string KUNGFU_STATE_NOTLEARNED = "kungfu_State_NotLearned";
        public const string KUNGFU_STATE_LEARNABLE = "kungfu_State_Learnable";
        public const string KUNGFU_NAME_BASICBOXING = "Kungfu_Name_BasicBoxing";
        public const string KUNGFU_NAME_XLSBZ = "Kungfu_Name_XLSBZ";

        //Equipment
        public const string EQUIP_TITLE_SKILL = "Equip_Title_Skill";
        public const string EQUIP_INTENSIFY = "Equip_Intensify";
        //ARMPR
        public const string ARMOR_TITLE = "Armor_title";
        public const string ARMOR_NAME_MKJ = "Armor_Name_MKJ";
        //ARMS
        public const string ARMS_TITLE = "Arms_title";

        //Work
        public const int WORK_NEED_FOOD_COUNT = 5;

        //Sound
        public const string MUSIC_MENU = "menu_music";
        public const string CLEVELUP = "CLevelup";
        public const string INTERFACE = "Interface";
        public const string MUSIC_BATTLE = "battle_music";
        public const string SOUND_UI_BTN = "Button";
        public const string SOUND_BLEVELUP = "Blevelup";
        public const string SOUND_BATTLE_WIN = "Battle_win";
        public const string SOUND_BATTLE_LOSE = "Battle_lose";
        public const string SOUND_BEAR = "Bear";
        public const string SOUND_CHARACTER_LEVEL_UP = "Character_Levelup";
        public const string SOUND_CHICKEN = "Chicken";
        public const string SOUND_COLLECT = "Collect";
        public const string SOUND_CURE = "Cure";
        public const string SOUND_DEATH_1 = "Death_1";
        public const string SOUND_DEATH_2 = "Death_2";
        public const string SOUND_DEATH_GIRL = "Death_W";
        public const string SOUND_DEER = "Deer";
        public const string SOUND_FORGE = "Forge";
        public const string SOUND_GET_UP = "Getup";
        public const string SOUND_HIT_1 = "Hit1";
        public const string SOUND_HIT_2 = "Hit2";
        public const string SOUND_HIT_3 = "Hit3";
        public const string SOUND_HIT_4 = "Hit4";
        public const string SOUND_HUNT = "Hunt";
        public const string SOUND_INTERFACE = "Interface";
        public const string SOUND_LUMBER = "Lumber";
        public const string SOUND_MINE = "Mine";
        public const string SOUND_PIG = "Pig";
        public const string SOUND_POUND = "Pound";
        public const string SOUND_RECRUIT = "Recruit";
        public const string SOUND_REVIVE = "Revive";
        public const string SOUND_SNAKE = "Snake";
        public const string SOUND_SWEEP = "Sweep";
        public const string SOUND_WELL = "Well";
        public const string SOUND_WOLF = "Wolf";
        public const string SOUND_FACILITY_BUILD_UPGRAD = "FacilityBuildAndUpgradSound";

        #region layer name
        public const string Bubble = "Bubble";
        public const string Facility = "Facility";
        #endregion

        #region collect system
        public const string IsClickCollectSytemBubble = "IsClickCollectSytemBubble";
        #endregion

        #region 微信相关
        public const string WeChatKey = "m_WeChatKey";
        #endregion


        #region 广告相关
        public const string DoubleGetBossRewardADName = "DoubleReward";
        #endregion
    }
}
