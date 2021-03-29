using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{

    public class TaskHandler
    {
        public virtual int count => 0;
        public virtual string taskTitle => "";
        public virtual string taskSubTitle => "";
    }

    public class TaskLevelHandler : TaskHandler
    {
        protected int m_Level;

        public TaskLevelHandler(int level)
        {
            m_Level = level;
        }
    }

    public class TaskHandler_BuildLivableRoom : TaskLevelHandler
    {
        public TaskHandler_BuildLivableRoom(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
        }

        private void HandleEvent(int key, params object[] args)
        {
            if (args == null || args.Length <= 0)
                return;
            FacilityType facilityType = (FacilityType)args[0];
            if (facilityType >= FacilityType.LivableRoomEast1 && facilityType <= FacilityType.LivableRoomWest4)
            {
                EventSystem.S.Send(EventID.OnRefeshMainTask);
            }
        }

        public override int count
        {
            get
            {
                int count = 0;
                for (int i = (int)FacilityType.LivableRoomEast1; i < (int)FacilityType.LivableRoomWest4; i++)
                {
                    int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel((FacilityType)i);
                    if (level >= m_Level)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public override string taskSubTitle
        {
            get
            {
                if (m_Level <= 1)
                    return "建造{0}间屋舍";
                else
                    return "拥有{0}间" + m_Level + "级屋舍";
            }
        }
    }

    public class TaskHandler_OwnStudents : TaskHandler
    {
        public TaskHandler_OwnStudents()
        {
            EventSystem.S.Register(EventID.OnAddCharacter, HandleEvent);
        }

        private void HandleEvent(int key, params object[] args)
        {
            EventSystem.S.Send(EventID.OnRefeshMainTask);
        }

        public override int count => MainGameMgr.S.CharacterMgr.GetCharacterCount();
        public override string taskSubTitle => "拥有{0}名弟子";

    }

    public class TaskHandler_BuildPracticeField : TaskLevelHandler
    {
        public TaskHandler_BuildPracticeField(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
        }
        private void HandleEvent(int key, params object[] args)
        {
            if (args == null || args.Length <= 0)
                return;
            FacilityType facilityType = (FacilityType)args[0];
            if (facilityType == FacilityType.PracticeFieldEast || facilityType == FacilityType.PracticeFieldWest)
            {
                EventSystem.S.Send(EventID.OnRefeshMainTask);
            }
        }

        public override int count
        {
            get
            {
                int count = 0;
                int levelEast = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.PracticeFieldEast);
                int levelWest = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.PracticeFieldWest);
                if (levelEast >= m_Level)
                    count++;

                if (levelWest >= m_Level)
                    count++;
                return count;
            }
        }

        public override string taskSubTitle
        {
            get
            {
                if (m_Level <= 1)
                    return "建造{0}间练功场";
                else
                    return "拥有{0}间" + m_Level + "级练功场";
            }
        }
    }

    public class TaskHandler_BuildLobby : TaskHandler
    {
        public TaskHandler_BuildLobby()
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
        }

        private void HandleEvent(int key, params object[] args)
        {
            if (args == null || args.Length <= 0)
                return;
            FacilityType facilityType = (FacilityType)args[0];
            if (facilityType == FacilityType.Lobby)
            {
                EventSystem.S.Send(EventID.OnRefeshMainTask);
            }
        }
        public override int count => MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();

        public override string taskSubTitle => "讲武堂升至{0}级";
    }

    public class TaskHandler_Chanllenge : TaskLevelHandler
    {
        public TaskHandler_Chanllenge(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnChanllengeSuccess, HandleEvent);
        }

        private void HandleEvent(int key, params object[] args)
        {
            EventSystem.S.Send(EventID.OnRefeshMainTask);
        }

        public override int count
        {
            get
            {
                var dataLst = GameDataMgr.S.GetPlayerData().chapterDataList;
                var chapterData = dataLst[dataLst.Count - 1];
                return chapterData.level;
            }
        }
        public override string taskSubTitle
        {
            get
            {
                int chapter = m_Level / 100;
                int count = m_Level - chapter * 100;
                return "完成挑战" + chapter + "-" + count;
            }
        }
    }

    public class TaskHandler_BuildLibrary : TaskLevelHandler
    {
        public TaskHandler_BuildLibrary(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
        }

        private void HandleEvent(int key, params object[] args)
        {
            if (args == null || args.Length <= 0)
                return;
            FacilityType facilityType = (FacilityType)args[0];
            if (facilityType == FacilityType.KongfuLibrary)
            {
                EventSystem.S.Send(EventID.OnRefeshMainTask);
            }
        }

        public override int count => MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.KongfuLibrary);

        public override string taskSubTitle
        {
            get
            {
                if (m_Level <= 1)
                    return "建造藏经阁";
                else
                    return "建造藏升至{0}级";
            }
        }
    }

    public class TaskHandler_BuildForgeHouse : TaskLevelHandler
    {
        public TaskHandler_BuildForgeHouse(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
        }

        private void HandleEvent(int key, params object[] args)
        {
            if (args == null || args.Length <= 0)
                return;
            FacilityType facilityType = (FacilityType)args[0];
            if (facilityType == FacilityType.ForgeHouse)
            {
                EventSystem.S.Send(EventID.OnRefeshMainTask);
            }
        }

        public override int count => MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.ForgeHouse);

        public override string taskSubTitle
        {
            get
            {
                if (m_Level <= 1)
                    return "建造锻造屋";
                else
                    return "锻造屋升至{0}级";
            }
        }
    }

    public class TaskHandler_BuildBaicaohu : TaskLevelHandler
    {
        public TaskHandler_BuildBaicaohu(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
        }

        private void HandleEvent(int key, params object[] args)
        {
            if (args == null || args.Length <= 0)
                return;
            FacilityType facilityType = (FacilityType)args[0];
            if (facilityType == FacilityType.Baicaohu)
            {
                EventSystem.S.Send(EventID.OnRefeshMainTask);
            }
        }

        public override int count => MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Baicaohu);

        public override string taskSubTitle
        {
            get
            {
                if (m_Level <= 1)
                    return "建造百草屋";
                else
                    return "百草屋升至{0}级";
            }
        }
    }

    ////////////////////
    public class TaskHandler_DailyFood : TaskHandler
    {
        public TaskHandler_DailyFood() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.food.dailyCount;
        public override string taskSubTitle => "补充{0}次食物";
    }

    public class TaskHandler_DailyVisitor : TaskHandler
    {
        public TaskHandler_DailyVisitor() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.visitor.dailyCount;
        public override string taskSubTitle => "接待任意{0}位访客";
    }

    public class TaskHandler_DailyRecruit : TaskHandler
    {
        public TaskHandler_DailyRecruit() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.recruit.dailyCount;
        public override string taskSubTitle => "招募{0}次弟子";
    }
    public class TaskHandler_DailyJob : TaskHandler
    {
        public TaskHandler_DailyJob() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.job.dailyCount;
        public override string taskSubTitle => "派遣弟子完成任意{0}次工作";
    }
    public class TaskHandler_DailyPractice : TaskHandler
    {
        public TaskHandler_DailyPractice() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.practice.dailyCount;
        public override string taskSubTitle => "派遣弟子在练功场完成{0}次练功";
    }
    public class TaskHandler_DailyCopy : TaskHandler
    {
        public TaskHandler_DailyCopy() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.copy.dailyCount;
        public override string taskSubTitle => "派遣弟子在藏经阁完成{0}次抄经";
    }
    public class TaskHandler_DailyCook : TaskHandler
    {
        public TaskHandler_DailyCook() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.cook.dailyCount;
        public override string taskSubTitle => "在伙房烹饪{0}次菜品";
    }
    public class TaskHandler_DailyCollect : TaskHandler
    {
        public TaskHandler_DailyCollect() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.collect.dailyCount;
        public override string taskSubTitle => "在池塘边完成任意{0}次采集";
    }
    public class TaskHandler_DailyChanllenge : TaskHandler
    {
        public TaskHandler_DailyChanllenge() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.chanllenge.dailyCount;
        public override string taskSubTitle => "派遣弟子完成{0}次挑战";
    }

    public class TaskHandler_DailyForge : TaskHandler
    {
        public TaskHandler_DailyForge() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.forge.dailyCount;
        public override string taskSubTitle => "在锻造屋完成{0}次装备打造";
    }

    public class TaskHandler_DailyMedicine : TaskHandler
    {
        public TaskHandler_DailyMedicine() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.medicine.dailyCount;
        public override string taskSubTitle => "在百草屋完成{0}次草药炼制";
    }

}