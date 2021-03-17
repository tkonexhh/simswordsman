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
        /// ��������
        /// </summary>
        public FacilityType FacilityType;
        /// <summary>
        /// ����id
        /// </summary>
        public int CharacterID;
        /// <summary>
        /// ������ʱ��
        /// </summary>
        public int WorkTotalTime;
        /// <summary>
        /// ��ǰ�ѹ���ʱ��
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