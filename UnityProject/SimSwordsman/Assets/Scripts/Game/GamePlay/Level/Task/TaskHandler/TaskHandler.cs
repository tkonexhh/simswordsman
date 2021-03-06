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
        public virtual Transform targetTransform => null;
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
            EventSystem.S.Register(EventID.OnStartUnlockFacility, HandleEvent);
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
                for (int i = (int)FacilityType.LivableRoomEast1; i <= (int)FacilityType.LivableRoomWest4; i++)
                {
                    if (!MainGameMgr.S.FacilityMgr.IsFacilityUnlocked((FacilityType)i))
                        continue;

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
                    return "??????{0}?????????";
                else
                    return "??????{0}???" + m_Level + "?????????";
            }
        }

        public override Transform targetTransform
        {
            get
            {
                for (int i = (int)FacilityType.LivableRoomEast1; i <= (int)FacilityType.LivableRoomWest4; i++)
                {
                    if (!MainGameMgr.S.FacilityMgr.IsFacilityUnlocked((FacilityType)i))
                    {
                        var controller = MainGameMgr.S.FacilityMgr.GetFacilityController((FacilityType)i);
                        return controller.view.transform;
                    }
                    else
                    {
                        int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel((FacilityType)i);
                        if (level < m_Level)//????????????????????????????????????
                        {
                            var controller = MainGameMgr.S.FacilityMgr.GetFacilityController((FacilityType)i);
                            return controller.view.transform;
                        }
                    }
                }

                return null;
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
        public override string taskSubTitle => "??????{0}?????????";

        public override Transform targetTransform
        {
            get
            {
                return MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Lobby).view.transform;
            }
        }


    }

    public class TaskHandler_BuildPracticeField : TaskLevelHandler
    {
        public TaskHandler_BuildPracticeField(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, HandleEvent);
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
                int levelEast = 0;
                int levelWest = 0;

                if (MainGameMgr.S.FacilityMgr.IsFacilityUnlocked(FacilityType.PracticeFieldEast))
                {
                    levelEast = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.PracticeFieldEast);
                }

                if (MainGameMgr.S.FacilityMgr.IsFacilityUnlocked(FacilityType.PracticeFieldWest))
                {
                    levelWest = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.PracticeFieldWest);
                }

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
                    return "??????{0}????????????";
                else
                    return "??????{0}???" + m_Level + "????????????";
            }
        }

        public override Transform targetTransform
        {
            get
            {
                for (int i = (int)FacilityType.PracticeFieldEast; i <= (int)FacilityType.PracticeFieldWest; i++)
                {
                    if (!MainGameMgr.S.FacilityMgr.IsFacilityUnlocked((FacilityType)i))
                    {
                        return MainGameMgr.S.FacilityMgr.GetFacilityController((FacilityType)i).view.transform;
                    }
                    else
                    {
                        int level = MainGameMgr.S.FacilityMgr.GetFacilityCurLevel((FacilityType)i);
                        if (level < m_Level)
                        {
                            return MainGameMgr.S.FacilityMgr.GetFacilityController((FacilityType)i).view.transform;
                        }
                    }
                }
                return null;
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

        public override string taskSubTitle => "???????????????{0}???";

        public override Transform targetTransform
        {
            get
            {
                return MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Lobby).view.transform;
            }
        }
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
                if (dataLst.Count <= 0)
                    return 0;
                int index = Mathf.Max(0, dataLst.Count - 1);
                var chapterData = dataLst[index];
                return chapterData.number;
            }
        }
        public override string taskSubTitle
        {
            get
            {
                int chapter = m_Level / 100;
                int count = m_Level - chapter * 100;
                return "????????????" + chapter + "-" + count;
            }
        }

        public override Transform targetTransform
        {
            get
            {
                UIMgr.S.OpenPanel(UIID.ChallengePanel);
                return null;
            }
        }
    }

    public class TaskHandler_BuildLibrary : TaskLevelHandler
    {
        public TaskHandler_BuildLibrary(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, HandleEvent);
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

        public override int count
        {
            get
            {
                if (!MainGameMgr.S.FacilityMgr.IsFacilityUnlocked(FacilityType.KongfuLibrary))
                    return 0;

                return MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.KongfuLibrary);
            }
        }


        public override string taskSubTitle
        {
            get
            {
                if (m_Level <= 1)
                    return "???????????????";
                else
                    return "???????????????{0}???";
            }
        }

        public override Transform targetTransform
        {
            get
            {
                return MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.KongfuLibrary).view.transform;
            }
        }
    }

    public class TaskHandler_BuildForgeHouse : TaskLevelHandler
    {
        public TaskHandler_BuildForgeHouse(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, HandleEvent);
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

        public override int count
        {
            get
            {
                if (!MainGameMgr.S.FacilityMgr.IsFacilityUnlocked(FacilityType.ForgeHouse))
                    return 0;

                return MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.ForgeHouse);
            }
        }


        public override string taskSubTitle
        {
            get
            {
                if (m_Level <= 1)
                    return "???????????????";
                else
                    return "???????????????{0}???";
            }
        }

        public override Transform targetTransform
        {
            get
            {
                return MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.ForgeHouse).view.transform;
            }
        }
    }

    public class TaskHandler_BuildBaicaohu : TaskLevelHandler
    {
        public TaskHandler_BuildBaicaohu(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, HandleEvent);
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

        public override int count
        {
            get
            {
                if (!MainGameMgr.S.FacilityMgr.IsFacilityUnlocked(FacilityType.Baicaohu))
                    return 0;

                return MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Baicaohu);
            }
        }

        public override string taskSubTitle
        {
            get
            {
                if (m_Level <= 1)
                    return "???????????????";
                else
                    return "???????????????{0}???";
            }
        }

        public override Transform targetTransform
        {
            get
            {
                return MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Baicaohu).view.transform;
            }
        }
    }


    public class TaskHandler_BuildDeliver : TaskLevelHandler
    {
        public TaskHandler_BuildDeliver(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, HandleEvent);
        }

        private void HandleEvent(int key, params object[] args)
        {
            if (args == null || args.Length <= 0)
                return;
            FacilityType facilityType = (FacilityType)args[0];
            if (facilityType == FacilityType.Deliver)
            {
                EventSystem.S.Send(EventID.OnRefeshMainTask);
            }
        }

        public override int count
        {
            get
            {
                if (!MainGameMgr.S.FacilityMgr.IsFacilityUnlocked(FacilityType.Deliver))
                    return 0;

                return MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Deliver);
            }
        }

        public override string taskSubTitle
        {
            get
            {
                if (m_Level <= 1)
                    return "????????????";
                else
                    return "????????????{0}???";
            }
        }

        public override Transform targetTransform
        {
            get
            {
                return MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Deliver).view.transform;
            }
        }
    }

    public class TaskHandler_BuildKitchen : TaskLevelHandler
    {
        public TaskHandler_BuildKitchen(int level) : base(level)
        {
            EventSystem.S.Register(EventID.OnStartUpgradeFacility, HandleEvent);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, HandleEvent);
        }

        private void HandleEvent(int key, params object[] args)
        {
            if (args == null || args.Length <= 0)
                return;
            FacilityType facilityType = (FacilityType)args[0];
            if (facilityType == FacilityType.Kitchen)
            {
                EventSystem.S.Send(EventID.OnRefeshMainTask);
            }
        }

        public override int count
        {
            get
            {
                if (!MainGameMgr.S.FacilityMgr.IsFacilityUnlocked(FacilityType.Kitchen))
                    return 0;

                return MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Kitchen);
            }
        }

        public override string taskSubTitle
        {
            get
            {
                if (m_Level <= 1)
                    return "????????????";
                else
                    return "????????????{0}???";
            }
        }

        public override Transform targetTransform
        {
            get
            {
                return MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.Kitchen).view.transform;
            }
        }
    }

    ////////////////////
    public class TaskHandler_DailyFood : TaskHandler
    {
        public TaskHandler_DailyFood() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.food.dailyCount;
        public override string taskSubTitle => "??????{0}?????????";
    }

    public class TaskHandler_DailyVisitor : TaskHandler
    {
        public TaskHandler_DailyVisitor() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.visitor.dailyCount;
        public override string taskSubTitle => "????????????{0}?????????";
    }

    public class TaskHandler_DailyRecruit : TaskHandler
    {
        public TaskHandler_DailyRecruit() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.recruit.dailyCount;
        public override string taskSubTitle => "??????{0}?????????";
    }
    public class TaskHandler_DailyJob : TaskHandler
    {
        public TaskHandler_DailyJob() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.job.dailyCount;
        public override string taskSubTitle => "????????????????????????{0}?????????";
    }
    public class TaskHandler_DailyPractice : TaskHandler
    {
        public TaskHandler_DailyPractice() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.practice.dailyCount;
        public override string taskSubTitle => "??????????????????????????????{0}?????????";
    }
    public class TaskHandler_DailyCopy : TaskHandler
    {
        public TaskHandler_DailyCopy() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.copy.dailyCount;
        public override string taskSubTitle => "??????????????????????????????{0}?????????";
    }
    public class TaskHandler_DailyCook : TaskHandler
    {
        public TaskHandler_DailyCook() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.cook.dailyCount;
        public override string taskSubTitle => "???????????????{0}?????????";
    }
    public class TaskHandler_DailyCollect : TaskHandler
    {
        public TaskHandler_DailyCollect() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.collect.dailyCount;
        public override string taskSubTitle => "????????????????????????{0}?????????";
    }
    public class TaskHandler_DailyChanllenge : TaskHandler
    {
        public TaskHandler_DailyChanllenge() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.chanllenge.dailyCount;
        public override string taskSubTitle => "??????????????????{0}?????????";
    }

    public class TaskHandler_DailyForge : TaskHandler
    {
        public TaskHandler_DailyForge() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.forge.dailyCount;
        public override string taskSubTitle => "??????????????????{0}???????????????";
    }

    public class TaskHandler_DailyMedicine : TaskHandler
    {
        public TaskHandler_DailyMedicine() { }
        public override int count => GameDataMgr.S.GetPlayerData().recordData.medicine.dailyCount;
        public override string taskSubTitle => "??????????????????{0}???????????????";
    }

}