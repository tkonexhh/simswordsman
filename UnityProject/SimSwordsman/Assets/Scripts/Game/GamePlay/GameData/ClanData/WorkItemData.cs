using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    [Serializable]
    public class WorkItemData
    {
        /// <summary>
        /// 建筑类型
        /// </summary>
        public FacilityType FacilityType;
        /// <summary>
        /// 弟子id
        /// </summary>
        public int CharacterID;
        /// <summary>
        /// 工作总时间
        /// </summary>
        public int WorkTotalTime;
        /// <summary>
        /// 当前已工作时间
        /// </summary>
        public int CurrentWorkTime = 0;

        public WorkItemData() { }

        public WorkItemData(FacilityType FacilityType, int characterID, int WorkTotalTime)
        {
            this.FacilityType = FacilityType;
            this.CharacterID = characterID;
            this.WorkTotalTime = WorkTotalTime;
        }
    }
}