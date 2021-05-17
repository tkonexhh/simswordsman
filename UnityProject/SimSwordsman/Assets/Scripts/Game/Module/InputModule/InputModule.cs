using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Qarth;
using UnityEngine.SceneManagement;

namespace GameWish.Game
{
    public class InputModule : AbstractModule
    {
        private IInputter m_KeyboardInputter;
        private KeyCodeTracker m_KeyCodeTracker;

        public override void OnComLateUpdate(float dt)
        {
            m_KeyboardInputter.LateUpdate();
            m_KeyCodeTracker.LateUpdate();
        }

        protected override void OnComAwake()
        {
            m_KeyCodeTracker = new KeyCodeTracker();
            m_KeyCodeTracker.SetDefaultProcessListener(ShowBackKeydownTips);

            m_KeyboardInputter = new KeyboardInputter();
            m_KeyboardInputter.RegisterKeyCodeMonitor(KeyCode.F1, null, OnClickF1, null);
            m_KeyboardInputter.RegisterKeyCodeMonitor(KeyCode.F2, null, OnClickF2, null);
            m_KeyboardInputter.RegisterKeyCodeMonitor(KeyCode.F3, null, OnClickF3, null);
            m_KeyboardInputter.RegisterKeyCodeMonitor(KeyCode.F4, null, OnClickF4, null);
        }

        private void ShowBackKeydownTips()
        {
            //WeGameSdkAdapter.S.ExitGame(null);
            if (PlayerPrefs.GetInt("channel_exit_key", 0) == 1)
            {
                FloatMessage.S.ShowMsg("再按一次退出游戏");
            }
        }

        private void OnClickF1()
        {
            MeasureUnitHelper.AddCount(0, 0, -50, -500);
            Log.i(MeasureUnitHelper.GetTotalCount());
        }
        int adtype = (int)AdType.PowerUp;
        private void OnClickF2()
        {

            //  EventSystem.S.Send(EventID.OnShowPopAdUI, AdType.SummonGiant);

            EventSystem.S.Send(EventID.OnShowPopAdUI, AdType.SummonReinforcements);

            // adtype++;
            // UIMgrExtend.S.OpenOccupyOverPanel(TDStageTable.GetData(101));
            // UIMgrExtend.S.OpenUnlockSoldierPanel(TDSoldierListTable.GetData(1));
            // EventSystem.S.Send(EventID.GuideEventTrigger, 1);
            //RateMgr.S.RequestReview();
        }

        private void OnClickF3()
        {
            GameDataMgr.S.GetPlayerData().towerData.ResetDailyData();
            GameDataMgr.S.Save();
        }

        private void OnClickF4()
        {
            var controller = MainGameMgr.S.CharacterMgr.GetCharacterController(1);
            Debug.LogError(CommonUIMethod.GetTenThousandOrMillion((long)controller.CharacterModel.CharacterItem.atkValue));
        }

        private void OnSceneLoadResult(string sceneName, bool result)
        {
            Log.i("SceneLoad:" + sceneName + " " + result);
        }
    }
}
