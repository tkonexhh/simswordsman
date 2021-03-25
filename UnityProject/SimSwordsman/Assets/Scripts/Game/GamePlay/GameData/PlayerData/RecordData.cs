using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class RecordData : IDataClass, IDailyResetData
    {
        public RecordItemData food = new RecordItemData();
        public RecordItemData visitor = new RecordItemData();
        public RecordItemData goldRecruit = new RecordItemData();//½ðÅÆÕÐÄ¼
        public RecordItemData silverRecruit = new RecordItemData();//ÒøÅÆÕÐÄ¼
        public RecordItemData job = new RecordItemData();
        public RecordItemData practice = new RecordItemData();
        public RecordItemData copy = new RecordItemData();
        public RecordItemData cook = new RecordItemData();
        public RecordItemData collect = new RecordItemData();
        public RecordItemData chanllenge = new RecordItemData();
        public RecordItemData forge = new RecordItemData();
        public RecordItemData medicine = new RecordItemData();

        public override void InitWithEmptyData()
        {

        }

        public override void OnDataLoadFinish()
        {

        }

        public void ResetDailyData()
        {
            food.ResetDailyCount();
            visitor.ResetDailyCount();
            goldRecruit.ResetDailyCount();
            silverRecruit.ResetDailyCount();
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

        public void AddGoldRecruit()
        {
            goldRecruit.AddCount();
            SetDataDirty();
        }

        public void AddSilverRecruit()
        {
            silverRecruit.AddCount();
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