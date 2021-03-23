using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameWish.Game
{
    public class KungfuLibraryData
    {
        public List<KungfuSoltDBData> kungfuLibraryDBDatas = new List<KungfuSoltDBData>();
        public KungfuLibraryData() { }

        public List<KungfuSoltDBData> GetKungfuLibrayData()
        {
            return kungfuLibraryDBDatas;
        }
        /// <summary>
        /// ????????¦Ë???
        /// </summary>
        /// <param name="kungfuLibraySlot"></param>
        public void AddKungfuLibrayData(KungfuLibraySlot kungfuLibraySlot)
        {
            KungfuSoltDBData kungfuLibraySlotDB = kungfuLibraryDBDatas.Where(i => i.facilityType == kungfuLibraySlot.FacilityType && i.soltID == kungfuLibraySlot.Index).FirstOrDefault();
            if (kungfuLibraySlotDB == null)
                kungfuLibraryDBDatas.Add(new KungfuSoltDBData(kungfuLibraySlot));
        }

        public void RefreshPracticeState(PracticeField kungfuLibraySlot)
        {
            //kungfuSoltDBData kungfuLibraySlotDB = kungfuLibraryDBDatas.Where(i => i.facilityType == kungfuLibraySlot.FacilityType && i.soltID == kungfuLibraySlot.Index).FirstOrDefault();
            //if (kungfuLibraySlotDB != null)
            //	kungfuLibraySlotDB.kungfuLibraySlotState = kungfuLibraySlot.PracticeFieldState;
        }

        /// <summary>
        /// ??????????????
        /// </summary>
        /// <param name="kungfuLibraySlot"></param>
        public void RefresDBData(KungfuLibraySlot kungfuLibraySlot)
        {
            KungfuSoltDBData kungfuLibraySlotDB = kungfuLibraryDBDatas.Where(i => i.facilityType == kungfuLibraySlot.FacilityType && i.soltID == kungfuLibraySlot.Index).FirstOrDefault();
            if (kungfuLibraySlotDB != null)
                kungfuLibraySlotDB.RefresDBData(kungfuLibraySlot);
        }

        public void TrainingIsOver(KungfuLibraySlot kungfuLibraySlot)
        {
            KungfuSoltDBData kungfuLibraySlotDB = kungfuLibraryDBDatas.Where(i => i.facilityType == kungfuLibraySlot.FacilityType && i.soltID == kungfuLibraySlot.Index).FirstOrDefault();
            if (kungfuLibraySlotDB != null)
                kungfuLibraySlotDB.TrainingIsOver();
        }
    }

    [Serializable]
    public class KungfuSoltDBData : SoltDBDataBase
    {
        public KungfuSoltDBData() { }
        public KungfuSoltDBData(KungfuLibraySlot kungfuLibraySlot) : base(kungfuLibraySlot)
        {
        }
    }
}