using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Qarth;
using System.Linq;

namespace GameWish.Game
{
    public partial class TDFacilityLivableRoomTable
    {
        public static List<LivableRoomLevelInfo> levelInfoDic = new List<LivableRoomLevelInfo>();

        static void CompleteRowAdd(TDFacilityLivableRoom tdData)
        {
            FacilityLevelInfo levelInfo = PassLevelInfo(tdData.id);
            levelInfo.level = tdData.level;
            LivableRoomLevelInfo roomLevelInfo = new LivableRoomLevelInfo();
            roomLevelInfo.Warp(levelInfo);
            roomLevelInfo.SetCurCapatity( tdData.capability);
            roomLevelInfo.roomId = tdData.houseId;

            //if (!levelInfoDic.ContainsKey(tdData.level))
            {
                levelInfoDic.Add(roomLevelInfo);
            }
        }

        public static LivableRoomLevelInfo GetLevelInfo(int roomId, int level)
        {
            LivableRoomLevelInfo info = levelInfoDic.FirstOrDefault(i => i.roomId == roomId && i.level == level);
            
            return info;
        }

        private static FacilityLevelInfo PassLevelInfo(int id)
        {
            FacilityLevelInfo levelInfo = null;

            TDFacilityLivableRoom item = null;
            bool haveData = m_DataCache.TryGetValue(id, out item);
            if (haveData)
            {
                levelInfo = TDFacilityLobbyTable.PassLevelInfo(id, item.upgradeCost, item.upgradePreconditions, item.upgradeRes);
            }

            return levelInfo;
        }
    }
}