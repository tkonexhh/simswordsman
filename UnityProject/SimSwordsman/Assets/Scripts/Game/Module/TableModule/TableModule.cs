﻿using System;
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

                // Game play table        
                TDLevelConfigTable.metaData,
                TDCharacterConfigTable.metaData,
                TDCharacterNameTable.metaData,
                TDArmsConfigTable.metaData,
                TDArmorConfigTable.metaData,
                TDCharacterQualityConfigTable.metaData,
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
                TDFacilityBaicaohuTable.metaData,
                TDFacilityPatrolRoomTable.metaData,
                TDFacilityBartizanTable.metaData,

                //Kongfu
                TDKongfuConfigTable.metaData,

                //Level
                TDLevelConfigTable.metaData,
                TDChapterConfigTable.metaData,

                TDMainTaskTable.metaData,

                TDItemConfigTable.metaData,
                TDWeaponConfigTable.metaData,

                //Herb
                TDHerbConfigTable.metaData,
 
                // Clan
                TDClanConfigTable.metaData,

            };
        }
    }
}
