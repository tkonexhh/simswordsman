using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class RecordData : IDataClass, IDailyResetData
    {
        public RecordItemData food;
        public RecordItemData visitor;
        public RecordItemData cruise;
        public RecordItemData job;
        public RecordItemData practice;
        public RecordItemData copy;
        public RecordItemData cook;
        public RecordItemData collect;
        public RecordItemData chanllenge;
        public RecordItemData forge;
        public RecordItemData medicine;

        public void ResetDailyData()
        {
            food.ResetDailyCount();
            visitor.ResetDailyCount();
            cruise.ResetDailyCount();
            job.ResetDailyCount();
            practice.ResetDailyCount();
            copy.ResetDailyCount();
            cook.ResetDailyCount();
            collect.ResetDailyCount();
            chanllenge.ResetDailyCount();
            forge.ResetDailyCount();
            medicine.ResetDailyCount();
            SetDataDirty();
        }


        public void AddFood()
        {
            food.AddCount();
            SetDataDirty();
        }

        public void AddVisitor()
        {
            visitor.AddCount();
            SetDataDirty();
        }

        public void AddCruise()
        {
            cruise.AddCount();
            SetDataDirty();
        }

        public void AddJob()
        {
            job.AddCount();
            SetDataDirty();
        }

        public void AddPractice()
        {
            practice.AddCount();
            SetDataDirty();
        }

        public void AddCopy()
        {
            copy.AddCount();
            SetDataDirty();
        }

        public void AddCook()
        {
            cook.AddCount();
            SetDataDirty();
        }

        public void AddCollect()
        {
            collect.AddCount();
            SetDataDirty();
        }

        public void AddChanllenge()
        {
            chanllenge.AddCount();
            SetDataDirty();
        }

        public void AddForge()
        {
            forge.AddCount();
            SetDataDirty();
        }

        public void AddMedicine()
        {
            medicine.AddCount();
            SetDataDirty();
        }
    }


    public class RecordItemData
    {
        public int totalCount;
        public int dailyCount;

        public void ResetDailyCount()
        {
            dailyCount = 0;
        }

        public void AddCount()
        {
            totalCount++;
            dailyCount++;
        }
    }

}