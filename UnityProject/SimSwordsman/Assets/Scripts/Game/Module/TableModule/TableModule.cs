using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class TableModule : AbstractTableModule
    {

        protected override void OnTableLoadFinish()
        {
            TDConstTable.InitArrays(typeof(ConstType));

            //处理所有表的重建

            Log.i("Load table finished");
        }

        protected override void InitPreLoadTableMetaData()
        {
            TableConfig.preLoadTableArray = new TDTableMetaData[]
            {
                // Default table
                TDConstTable.metaData,
                TDLanguageTable.GetLanguageMetaData(),
                TDGuideTable.metaData,
                TDGuideStepTable.metaData,
                TDSocialAdapterTable.metaData,
                TDAdConfigTable.metaData,
                TDAdPlacementTable.metaData,
                TDAppConfigTable.metaData,
                TDRemoteConfigTable.metaData,
                TDPurchaseTable.metaData,

                TDAdNativeViewTable.metaData,

                // Game play table        
                TDLevelConfigTable.metaData,
                TDCharacterConfigTable.metaData,
                TDCharacterNameTable.metaData,
                TDEquipmentConfigTable.metaData,
                TDCharacterStageConfigTable.metaData,

                //Facility
                TDFacilityConfigTable.metaData,
                TDFacilityKongfuLibraryTable.metaData,
                TDFacilityLivableRoomTable.metaData,
                TDFacilityLobbyTable.metaData,
                TDFacilityWarehouseTable.metaData,
                TDFacilityPracticeFieldTable.metaData,
                TDFacilityKitchenTable.metaData,
                TDFacilityForgeHouseTable.metaData,
                TDFacilityDeliverTable.metaData,
                TDFacilityBaicaohuTable.metaData,
                TDFacilityPatrolRoomTable.metaData,

                //Kongfu
                TDKongfuConfigTable.metaData,
                TDKongfuStageConfigTable.metaData,
                TDKongfuAnimationConfigTable.metaData,

                //Level
                TDLevelConfigTable.metaData,
                TDChapterConfigTable.metaData,

                TDMainTaskTable.metaData,
                TDCommonTaskTable.metaData,
                TDBattleWordsTable.metaData,
                TDEnemyConfigTable.metaData,

                TDItemConfigTable.metaData,
                //TDWeaponConfigTable.metaData,

                //Herb
                TDHerbConfigTable.metaData,
 
                // Clan
                TDClanConfigTable.metaData,

                TDWorkTable.metaData,
                TDSystemConfigTable.metaData,

                TDSectNameTable.metaData,
                TDDailySigninTable.metaData,
                TDVisitorConfigTable.metaData,
                TDVisitorRewardConfigTable.metaData,
                TDFoodConfigTable.metaData,
                TDCollectConfigTable.metaData,
                Qarth.TDAdSceneConfigTable.metaData,
                //Task
                TDDailyTaskTable.metaData,
                TDMainTaskTable.metaData,

                TDDeliverTable.metaData,
                //Tower
                TDTowerConfigTable.metaData,
                TDTowerEnemyConfigTable.metaData,
                TDTowerRewardConfigTable.metaData,
                TDTowerShopTable.metaData,

                //头像
                TDAvatarTable.metaData,

                //Trial
                TDHeroTrialConfigTable.metaData,

                TDTalkTable.metaData,
                //Arena
                TDArenaConfigTable.metaData,
                TDArenaEnemyConfigTable.metaData,
                TDArenaEnemyNameTable.metaData,
                TDArenaShopTable.metaData,
            };
        }
    }
}
