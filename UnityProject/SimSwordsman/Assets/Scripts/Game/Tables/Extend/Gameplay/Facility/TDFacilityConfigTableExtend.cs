using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public partial class TDFacilityConfigTable
    {
        public static Dictionary<int, FacilityConfigInfo> facilityConfigInfoDic = new Dictionary<int, FacilityConfigInfo>();

        static void CompleteRowAdd(TDFacilityConfig tdData)
        {
            FacilityType prefacilityType = EnumUtil.ConvertStringToEnum<FacilityType>(tdData.preFacility);
            FacilityConfigInfo configInfo = new FacilityConfigInfo((FacilityType)tdData.id, 
                tdData.name, tdData.desc, prefacilityType);

            if (!facilityConfigInfoDic.ContainsKey(tdData.id))
            {
                facilityConfigInfoDic.Add(tdData.id, configInfo);
            }
        }

        public static FacilityConfigInfo GetFacilityConfigInfo(FacilityType facilityType)
        {
            int id = (int)facilityType;

            if (facilityConfigInfoDic.ContainsKey(id))
            {
                return facilityConfigInfoDic[id];
            }

            return null;
        }
    }

    public class FacilityConfigInfo
    {
        public FacilityType facilityType;
        public string name;
        public string desc;
        public FacilityType prefacilityType;
        //public int needLobbyLevel;
        //public int unlockCost;

        public FacilityConfigInfo(FacilityType facilityType, string name, string desc, 
            FacilityType prefacilityType/*, int needLobbyLevel, int unlockCost*/)
        {
            this.facilityType = facilityType;
            this.name = name;
            this.desc = desc;
            this.prefacilityType = prefacilityType;
            //this.needLobbyLevel = needLobbyLevel;
            //this.unlockCost = unlockCost;
        }

        public int GetNeedLobbyLevel()
        {
            int needLevel = -1;

            try
            {
                needLevel = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(facilityType, 1).GetNeedLobbyLevel();
            }
            catch (Exception e)
            {
                Debug.LogError("Error: " + e.Message.ToString() + " facilityType is: " + facilityType.ToString());
            }
            return needLevel;
        }

        public int GetUnlockCoinCost()
        {
            int coin = MainGameMgr.S.FacilityMgr.GetFacilityLevelInfo(facilityType, 1).upgradeCoinCost;
            return coin;
        }
    }

}