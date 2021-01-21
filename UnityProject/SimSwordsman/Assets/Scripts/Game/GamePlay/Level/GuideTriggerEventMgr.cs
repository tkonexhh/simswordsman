using Qarth;

namespace GameWish.Game
{
	public class GuideTriggerEventMgr : TSingleton<GuideTriggerEventMgr>
	{
	    public void Init()
        {
            EventSystem.S.Register(EventID.OnGuideFirstGetCharacter, StartGuide_Task1);
            EventSystem.S.Register(EventID.OnGuideSecondGetCharacter, StartGuide_Task2);
            EventSystem.S.Register(EventID.OnStartUnlockFacility, UnlockFacility);
        }

        private void StartGuide_Task1(int key, object[] param)
        {
            //Timer.S.Post2Really(x => 
            //{
            UIMgr.S.ClosePanelAsUIID(UIID.LobbyPanel);
                EventSystem.S.Send(EventID.OnGuideDialog4);
            //}, 1f);
        }

        private void StartGuide_Task2(int key, object[] param)
        {
            //Timer.S.Post2Really(x =>
            //{
            UIMgr.S.ClosePanelAsUIID(UIID.LobbyPanel);
            EventSystem.S.Send(EventID.OnGuideDialog7);
            //}, 1f);
        }

        private void UnlockFacility(int key, object[] param)
        {
            FacilityType type = (FacilityType)param[0];
            switch (type)
            {
                case FacilityType.KongfuLibrary:
                    EventSystem.S.Send(EventID.OnGuideUnlockKungfuLibrary);
                    break;
                case FacilityType.ForgeHouse:
                    EventSystem.S.Send(EventID.OnGuideUnlockForgeHouse);
                    break;
                case FacilityType.Baicaohu:
                    EventSystem.S.Send(EventID.OnGuideUnlockBaicaohu);
                    break;
                case FacilityType.PracticeFieldEast:
                    if (GameDataMgr.S.GetClanData().GetFacilityData(FacilityType.PracticeFieldWest).facilityState != FacilityState.Unlocked)
                        EventSystem.S.Send(EventID.OnGuideUnlockPracticeField);
                    break;
                case FacilityType.PracticeFieldWest:
                    if (GameDataMgr.S.GetClanData().GetFacilityData(FacilityType.PracticeFieldEast).facilityState != FacilityState.Unlocked)
                        EventSystem.S.Send(EventID.OnGuideUnlockPracticeField);
                    break;
            }
        }
    }
	
}