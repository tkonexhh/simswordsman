using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Qarth;

namespace GameWish.Game
{
    public class RawMatItem : MonoBehaviour, IRawMatStateHander
    {
        public CollectedObjType collectedObjType = CollectedObjType.None;
        public List<Transform> collectPos = new List<Transform>();
        public GameObject bubble = null;

        //private DateTime m_LastShowBubbleTime;

        //private bool m_IsBubbleShowed = false;
        //private bool m_IsCharacterCollected = false;

        //private List<Transform> m_UsedCollectPos = new List<Transform>();
        private WorkConfigItem m_WorkConfigItem = null;

        //private bool m_IsWorking = false;

        private CharacterController m_SelectedCharacter = null;

        private RawMatStateMachine m_StateMachine = null;
        public RawMatStateID m_CurState = RawMatStateID.None;

        public WorkConfigItem WorkConfigItem { get => m_WorkConfigItem; }

        public virtual void OnInit()
        {
            m_StateMachine = new RawMatStateMachine(this);

            m_WorkConfigItem = TDWorkTable.GetWorkConfigItem(collectedObjType);
            if (m_WorkConfigItem == null)
            {
                Log.e("Work config item is null: " + collectedObjType.ToString());
            }

            RegisterEvents();
        }

        public void InitState()
        {
            // State may set Working by character before init data
            if (m_CurState != RawMatStateID.None)
            {
                Log.i("RawMatItem state is: " + m_CurState);
                return;
            }

            bool isUnlocked = IsUnlocked();

            if (!isUnlocked)
            {
                SetState(RawMatStateID.Locked);
            }
            else
            {
                SetState(RawMatStateID.Idle);
            }

        }

        private void RegisterEvents()
        {
            EventSystem.S.Register(EventID.OnShowWorkBubble, HandleEvent);
        }

        public void SetState(RawMatStateID state)
        {
            if (state != m_CurState)
            {
                m_CurState = state;

                m_StateMachine.SetCurrentStateByID(m_CurState);

                OnStateChanged(state);
            }
        }

        public virtual void OnUpdate()
        {
            m_StateMachine.UpdateState(Time.deltaTime);
        }

        public virtual void OnCharacterArriveCollectPos()
        {
        }
        //public void Refresh()
        //{
        //    CheckUnlocked();

        //    if (!m_IsUnlocked)
        //        return;
        //    if (m_IsWorking)
        //        return;

        //    TimeSpan timeSpan = DateTime.Now - m_LastShowBubbleTime;

        //    ////气泡在，角色未选
        //    //if (m_IsBubbleShowed && !m_IsCharacterCollected)
        //    //{
        //    //    if (!(KongfuLibraryPanel.isOpened || PracticeFieldPanel.isOpened))
        //    //    {
        //    //        if (timeSpan.TotalSeconds > m_WorkConfigItem.waitingTime && (DateTime.Now-GameplayMgr.resumeTime).TotalSeconds > 5)
        //    //        {
        //    //            AutoSelectCharacter();
        //    //        }
        //    //    }
        //    //}

        //    //气泡不在，角色未选
        //    if (!m_IsBubbleShowed && !m_IsCharacterCollected)
        //    {
        //        if (timeSpan.TotalSeconds > m_WorkConfigItem.workInterval)
        //        {
        //            ShowBubble();
        //        }
        //    }

        //    //气泡不在，角色已选
        //    if (!m_IsBubbleShowed && m_IsCharacterCollected && m_SelectedCharacter != null)
        //    {
        //        if (m_SelectedCharacter.CollectObjType == collectedObjType)
        //        {
        //            if (m_SelectedCharacter.CurState == CharacterStateID.Wander)
        //            {
        //                Log.e("RawMatItem, The selected character state wrong, this should not happen, collectedObjType: " + collectedObjType);
        //                m_SelectedCharacter.SetState(CharacterStateID.CollectRes);
        //            }
        //        }
        //        else
        //        {
        //            Log.e("RawMatItem, The selected character collectedObjType not right, this should not happen, collectedObjType: " + collectedObjType);

        //            ResetData();
        //        }
        //    }
        //}

        public bool IsFoodEnough()
        {
            int curFood = GameDataMgr.S.GetPlayerData().GetFoodNum();
            return curFood >= Define.WORK_NEED_FOOD_COUNT;
        }

        public void OnClicked()
        {
            if (IsFoodEnough() == false && GuideMgr.S.IsGuideFinish(31) && GuideMgr.S.IsGuideFinish(14))
            {
                if (GuideMgr.S.IsGuideFinish(32) == false)
                {
                    EventSystem.S.Send(EventID.OnFoodNotEnoughTrigger_IntroduceTrigger);
                }
                else 
                {
                    int remaintCount = GameDataMgr.S.GetPlayerData().GetFoodRefreshTimesToday();
                    if (remaintCount > 0)
                    {
                        UIMgr.S.OpenPanel(UIID.SupplementFoodPanel);
                    }
                    else
                    {
                        DataAnalysisMgr.S.CustomEvent(DotDefine.out_of_food);
                        FloatMessage.S.ShowMsg("食物不足");
                    }
                }

                return;
            }

            CharacterController character = SelectIdleCharacterToCollectRes(true);
            if (character == null)
            {
                FloatMessage.S.ShowMsg("无空闲弟子");
            }
            else
            {
                OnCharacterSelected(character, true);
                DataAnalysisMgr.S.CustomEvent(DotDefine.work_enter, collectedObjType.ToString());
            }
        }

        public CharacterController SelectIdleCharacterToCollectRes(bool manual)
        {
            CharacterController character = MainGameMgr.S.CharacterMgr.CharacterControllerList.FirstOrDefault(i => i.CharacterModel.IsIdle());
            return character;
        }

        public void OnCharacterSelected(CharacterController character, bool manual)
        {
            if (character != null)
            {
                m_SelectedCharacter = character;

                character.CollectObjType = collectedObjType;
                character.ManualSelectedToCollectObj = manual;
                character.SetState(CharacterStateID.CollectRes);

                //HideBubble();

                DataAnalysisMgr.S.CustomEvent(DotDefine.work_auto_enter, collectedObjType.ToString());

                GameDataMgr.S.GetPlayerData().ReduceFoodNum(Define.WORK_NEED_FOOD_COUNT);

                SetState(RawMatStateID.Working);
            }
        }

        public virtual void OnStateChanged(RawMatStateID state)
        {

        }

        public void ShowBubble()
        {
            bubble.SetActive(true);
        }

        public void HideBubble()
        {
            bubble.SetActive(false);
        }

        public void OnCharacterSelected(CharacterController character)
        {
            m_SelectedCharacter = character;

            SetState(RawMatStateID.Working);
        }

        public Transform GetRandomCollectPos()
        {
            //List<Transform> list = new List<Transform>();
            //foreach (Transform t in collectPos)
            //{
            //    if (!m_UsedCollectPos.Contains(t))
            //    {
            //        list.Add(t);
            //    }
            //}

            //if (list.Count > 0)
            //{
            //    int index = UnityEngine.Random.Range(0, list.Count);
            //    return list[index];
            //}

            return collectPos[0];
        }

        public bool IsUnlocked()
        {
            bool isUnlocked = MainGameMgr.S.FacilityMgr.GetFacilityState(FacilityType.Lobby) == FacilityState.Unlocked 
                && MainGameMgr.S.FacilityMgr.GetFacilityCurLevel(FacilityType.Lobby) >= m_WorkConfigItem.unlockHomeLevel;

            return isUnlocked;
        }

        private void HandleEvent(int key, object[] param)
        {
            switch (key)
            {
                case (int)EventID.OnShowWorkBubble:
                    CollectedObjType type = (CollectedObjType)param[0];
                    if (this.collectedObjType == type) {
                        //ShowBubble();
                        SetState(RawMatStateID.BubbleShowing);
                    }
                    break;
            }
        }

        //private void ResetData()
        //{
        //    m_SelectedCharacter = null;
        //    m_IsCharacterCollected = false;
        //    m_IsWorking = false;
        //}

        public RawMatItem GetRawMatItem()
        {
            return this;
        }
    }
	
}