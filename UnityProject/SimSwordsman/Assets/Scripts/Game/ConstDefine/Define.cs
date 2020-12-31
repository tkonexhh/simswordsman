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
        public const int DEFAULT_COIN_NUM = 0;
        public const int DEFAULT_FOOD_NUM = 0;
        public const int DEFAULT_TIP_NUM = 5;

        public const string DEFAULT_SOUND = "";
        public const string DEFAULT_NUMBER_ZERO = "0";
        public const string SOUND_DEFAULT_SOUND = "Sound_ButtonClick";
        public const string SOUND_BUTTON_CLICK = "Sound_ButtonClick";
        public const string SOUND_BLOCK_UPGRADE = "Sound_LevelUp"; 
        public const string SOUND_PANEL_CLOSE = "Sound_Close";    
        public const string SOUND_POSITIVE_EFFECT = "Sound_Positive"; 
        public const string SOUND_EVOLVE = "Sound_Evolve";

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

       

        public const string AD_PLACEMENT_INTER = "";
        public const string SLASH = "/";
        public const string PLUS = "+";
        public const string RIDE = "x";
        public const string COMMA = ",";

        public const int AD_MAX_TIME = 120;

        //Common
        public const string COMMON_UPGRADERESOURCES = "Common_UpgradeResources";
        public const string COMMON_BUILDRESOURCES = "Common_BuildResources";
        public const string COMMON_UPGRADEINFODESC = "Common_UpgradeInfoDesc";
        public const string COMMON_UPGRADENEEDS = "Common_UpgradeNeeds";
        public const string COMMON_BUILDINFODESC = "Common_BuildInfoDesc";
        public const string COMMON_UPGRADE = "Common_Upgrade";
        public const string COMMON_BUILD = "Common_Build";
        public const string COMMON_NOTBUILD = "Common_NotBuild";
        public const string COMMON_NOTUNLOCKED = "Common_NotUnlocked";
        public const string COMMON_UNLOCKED = "Common_Unlocked";
        public const string COMMON_FULLLEVEL = "Common_FullLevel";

        public const string COMMON_UNIT_GRADE = "Common_Unit_Grade";
        public const string COMMON_UNIT_PEOPLE = "Common_Unit_People";
        public const string COMMON_UNIT_DAY = "Common_Unit_Day";
        public const string COMMON_UNIT_PAET = "Common_Unit_Part";
        public const string COMMON_UNIT_CLASS = "Common_Unit_Class";
        public const string COMMON_UNIT_INDIVIDUAL = "Common_Unit_Individual";
        public const string COMMON_UNIT_ONLY = "Common_Unit_Only";
        public const string COMMON_UNIT_LAYER = "Common_Unit_Layer";

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
        public const int FACILITY_MAX_LEVEL = 20;

        // Chapter
        public const int MAX_CHAPTER_COUNT = 5;
        public const int LEVEL_COUNT_PER_CHAPTER = 25;

        // Inventory
        public const int MAX_PROP_COUNT = 999;

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
        public const string Challenge_LOG_TITLE = "Challenge_Log_Title";
        public const string Challenge_LOG_CONTENT = "Challenge_Log_Content";
        public const string Challenge_LOG_ACCEPTBTNTXT = "Challenge_Log_AcceptBtnTxt";
        public const string Challenge_LOG_REFUSEBTNTXT = "Challenge_Log_RefuseBtnTxt";

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
        public const string DISCIPLE_TITLE_LEVEL= "Disciple_Title_Level";
        public const string DISCIPLE_TITLE_SKILL = "Disciple_Title_Skill";
        public const string DISCIPLE_TITLE_ENTRYTIME = "Disciple_Title_EntryTime";
        public const string DISCIPLE_TITLE_RANK = "Disciple_Title_Rank";
        public const string DISCIPLE_STATE_WORKING= "Disciple_State_Working";
        public const string DISCIPLE_STATE_FREE= "Disciple_State_Free";
        public const string DISCIPLE_PRACTICE = "Disciple_Practice";
        public const string DISCIPLE_WORK = "Disciple_Work";
        public const string DISCIPLE_EJECT = "Disciple_Eject";

        //LivableRoom
        public const string FACILITY_LIVABLEROOMEAST_NAME = "Facility_LivableRoomEast_Name";
        public const string FACILITY_LIVABLEROOMWEST_NAME = "Facility_LivableRoomWest_Name";
        public const string FACILITY_LIVABLEROOM_DESCRIBLE = "Facility_LivableRoom_Describe";
        public const string FACILITY_LIVABLEROOM_CURRENTLYHABITABLE= "Facility_LivableRoom_CurrentlyHabitable";
        public const string FACILITY_LIVABLEROOM_NEXTHABITABLE= "Facility_LivableRoom_NextHabitable";



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



    }
}
