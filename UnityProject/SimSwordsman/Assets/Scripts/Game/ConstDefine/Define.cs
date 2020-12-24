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
        public const string Slash = "/";

        public const int AD_MAX_TIME = 120;

        //Common
        public const string COMMON_UPGRADERESOURCES = "Common_UpgradeResources";
        public const string COMMON_BUILDRESOURCES = "Common_BuildResources";
        public const string COMMON_UPGRADEINFODESC = "Common_UpgradeInfoDesc";
        public const string COMMON_BUILDINFODESC = "Common_BuildInfoDesc";
        public const string COMMON_UPGRADE = "Common_Upgrade";
        public const string COMMON_UNIT_GRADE = "Common_Unit_Grade";
        public const string COMMON_UNIT_PEOPLE = "Common_Unit_People";
        public const string COMMON_BUILD = "Common_Build";
        public const string COMMON_NOTBUILD = "Common_NotBuild";
        public const string COMMON_FULLLEVEL = "Common_FullLevel";
        public const string COMMON_DEFAULT_STR = "";



        // Character
        public const int CHARACTER_NORAML_MAX_LEVEL = 250;
        public const int CHARACTER_GOOD_MAX_LEVEL = 350;
        public const int CHARACTER_EXCELLENT_MAX_LEVEL = 450;

        // Facility
        public const int FACILITY_MAX_LEVEL = 20;

        // Chapter
        public const int MAX_CHAPTER_COUNT = 5;
        public const int LEVEL_COUNT_PER_CHAPTER = 25;

        // Inventory
        public const int MAX_PROP_COUNT = 99;
  
        //LobbyPanel
        public const string LEVEL = "级";
        public const string LECTURE_HALL = "讲武堂升至";
        public const string RECRUIT_FREE = "免费";
        public const string SILVER_ORDER = "银牌招募令";
        public const string GOLD_MEDAL_RECRUITt_ORDER = "金牌招募令";
        public const string LOOKING_AT_SILVER_ADVERTISEMENT  = "看银牌广告";
        public const string WATCH_GOLD_MEDALS = "看金牌广告";

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
        public const string FACILITY_WAREHOUSE_CLASS = "Facility_Warehouse_Class";

        //Disciple
        public const string DISCIPLE_NAME = "Disciple_Name";
        public const string DISCIPLE_DESCRIBE = "Disciple_Describe";
        public const string DISCIPLE_QUALITY_NORMAL = "Disciple_Quality_Normal";
        public const string DISCIPLE_NAME_GOOD = "Disciple_Quality_Good";
        public const string DISCIPLE_NAME_PREFECT = "Disciple_Quality_Perfect";

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


    }
}
