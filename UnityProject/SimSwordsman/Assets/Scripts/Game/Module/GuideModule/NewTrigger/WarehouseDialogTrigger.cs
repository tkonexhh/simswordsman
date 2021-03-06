using System;
using Qarth;


namespace GameWish.Game
{
	public class WarehouseDialogTrigger : ITrigger
	{
        bool m_CanStart = false;
        public bool isReady { get { return m_CanStart;  } }

        Action<bool, ITrigger> m_Listener;

        public void SetParam(object[] param)
        {
            
        }

        public void Start(Action<bool, ITrigger> l)
        {
            m_Listener = l;
            EventSystem.S.Register(EventID.OnGuideUnlockWarehouse, OnEventListener);
        }
        void OnEventListener(int key, object[] param)
        {
            foreach (var item in TDFacilityWarehouseTable.GetLevelInfo(1).GetUpgradeResCosts())
            {
                if (MainGameMgr.S.InventoryMgr.GetCurrentCountByItemType((RawMaterial)item.itemId) < item.value)
                    return;
            }
            MainGameMgr.S.FacilityMgr.SetFacilityState(FacilityType.Warehouse, FacilityState.ReadyToUnlock);
            EventSystem.S.Send(EventID.OnCloseAllUIPanel);

            m_CanStart = true;

            if (isReady)
            {
                m_Listener(true, this);
            }
            else
            {
                m_Listener(false, this);
            }
        }
        public void Stop()
        {
            m_CanStart = false;
            m_Listener = null;
            EventSystem.S.UnRegister(EventID.OnGuideUnlockWarehouse, OnEventListener);

            EventSystem.S.Send(EventID.OnGuideBuildWarehouse);
        }

	}
	
}