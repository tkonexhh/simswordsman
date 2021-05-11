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
        public RecordItemData recruit = new RecordItemData();//��ļ
        public RecordItemData job = new RecordItemData();
        public RecordItemData practice = new RecordItemData();
        public RecordItemData copy = new RecordItemData();
        public RecordItemData cook = new RecordItemData();
        public RecordItemData collect = new RecordItemData();
        public RecordItemData chanllenge = new RecordItemData();
        public RecordItemData forge = new RecordItemData();
        public RecordItemData medicine = new RecordItemData();
        public RecordItemData towerShopRefesh = new RecordItemData();
        public RecordItemData towerRevive = new RecordItemData();
        public RecordItemData arenaShopRefesh = new RecordItemData();

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
            recruit.ResetDailyCount();
            job.ResetDailyCount();
            practice.ResetDailyCount();
            copy.ResetDailyCount();
            cook.ResetDailyCount();
            collect.ResetDailyCount();
            chanllenge.ResetDailyCount();
            forge.ResetDailyCount();
            medicine.ResetDailyCount();
            towerShopRefesh.ResetDailyCount();
            towerRevive.ResetDailyCount();
            arenaShopRefesh.ResetDailyCount();

            SetDataDirty();
        }


        public void AddFood()
        {
            food.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddVisitor()
        {
            visitor.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddRecruit()
        {
            recruit.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddJob()
        {
            job.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddPractice()
        {
            practice.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddCopy()
        {
            copy.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddCook()
        {
            cook.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddCollect()
        {
            collect.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddChanllenge()
        {
            chanllenge.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddForge()
        {
            forge.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddMedicine()
        {
            medicine.AddCount();
            SetDataDirty();
            EventSystem.S.Send(EventID.OnRefeshDailyTask);
        }

        public void AddTowerShopRefesh()
        {
            towerShopRefesh.AddCount();
            SetDataDirty();
        }

        public void AddArenaShopRefesh()
        {
            arenaShopRefesh.AddCount();
            SetDataDirty();
        }

        public void AddTowerRevive()
        {
            towerRevive.AddCount();
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