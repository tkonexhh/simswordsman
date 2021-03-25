using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{

    public class TaskHandler
    {
        public virtual int count => 0;
        public virtual string taskTitle => "";
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
        public TaskHandler_BuildLivableRoom(int level) : base(level) { }

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
    }

    public class TaskHandler_OwnStudents : TaskHandler
    {
        public TaskHandler_OwnStudents() { }
        public override int count => MainGameMgr.S.CharacterMgr.GetCharacterCount();

    }

    public class TaskHandler_BuildPracticeField : TaskLevelHandler
    {
        public TaskHandler_BuildPracticeField(int level) : base(level) { }
        public override int count
        {
            get
            {
                return 0;
            }
        }
    }

    public class TaskHandler_BuildLobby : TaskLevelHandler
    {
        public TaskHandler_BuildLobby(int level) : base(level) { }
        public override int count => MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
    }

    public class TaskHandler_Chanllenge : TaskLevelHandler
    {
        public TaskHandler_Chanllenge(int level) : base(level) { }
        //TODO
        public override int count => 0;//MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
    }

    public class TaskHandler_BuildLibrary : TaskLevelHandler
    {
        public TaskHandler_BuildLibrary(int level) : base(level) { }
        //TODO
        public override int count => 0;//MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
    }

    public class TaskHandler_BuildForgeHouse : TaskLevelHandler
    {
        public TaskHandler_BuildForgeHouse(int level) : base(level) { }
        //TODO
        public override int count => 0;//MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
    }

    public class TaskHandler_BuildBaicaohu : TaskLevelHandler
    {
        public TaskHandler_BuildBaicaohu(int level) : base(level) { }
        //TODO
        public override int count => 0;//MainGameMgr.S.FacilityMgr.GetLobbyCurLevel();
    }

    ////////////////////
    public class TaskHandler_DailyFood : TaskHandler
    {
        public TaskHandler_DailyFood() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.food.dailyCount;
    }

    public class TaskHandler_DailyVisitor : TaskHandler
    {
        public TaskHandler_DailyVisitor() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.visitor.dailyCount;
    }

    public class TaskHandler_DailyCruise : TaskHandler
    {
        public TaskHandler_DailyCruise() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.cruise.dailyCount;
    }
    public class TaskHandler_DailyJob : TaskHandler
    {
        public TaskHandler_DailyJob() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.job.dailyCount;
    }
    public class TaskHandler_DailyPractice : TaskHandler
    {
        public TaskHandler_DailyPractice() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.practice.dailyCount;
    }
    public class TaskHandler_DailyCopy : TaskHandler
    {
        public TaskHandler_DailyCopy() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.copy.dailyCount;
    }
    public class TaskHandler_DailyCook : TaskHandler
    {
        public TaskHandler_DailyCook() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.cook.dailyCount;
    }
    public class TaskHandler_DailyCollect : TaskHandler
    {
        public TaskHandler_DailyCollect() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.collect.dailyCount;
    }
    public class TaskHandler_DailyChanllenge : TaskHandler
    {
        public TaskHandler_DailyChanllenge() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.food.dailyCount;
    }

    public class TaskHandler_DailyForge : TaskHandler
    {
        public TaskHandler_DailyForge() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.forge.dailyCount;
    }

    public class TaskHandler_DailyMedicine : TaskHandler
    {
        public TaskHandler_DailyMedicine() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.medicine.dailyCount;
    }

}