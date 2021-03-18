using System.Collections;
using System.Collections.Generic;
using Qarth;

namespace GameWish.Game
{
    public class CharacterStateReading : CharacterStateCD
    {
        protected override CharacterStateID targetState => CharacterStateID.Reading;
        protected override string animName => "write";

        public CharacterStateReading(CharacterStateID stateEnum) : base(stateEnum) { }

        protected override BaseSlot GetTargetSlot()
        {
            KongfuLibraryController kongFuController = (KongfuLibraryController)MainGameMgr.S.FacilityMgr.GetFacilityController(FacilityType.KongfuLibrary);
            return kongFuController.GetIdlePracticeSlot();
        }

        protected override void OnCDOver()
        {
            int kungfuID = (int)MainGameMgr.S.FacilityMgr.GetKungfuForWeightAndLevel(MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.KongfuLibrary));
            RewardBase reward = RewardMgr.S.GetRewardBase(RewardItemType.Kongfu, kungfuID, 1);
            List<RewardBase> rewards = new List<RewardBase>();
            rewards.Add(reward);
            UIMgr.S.OpenPanel(UIID.RewardPanel, null, rewards);
            reward.AcceptReward();
        }
    }
}
