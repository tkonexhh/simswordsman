using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Qarth
{
    public partial class AudioMgr
    {
        public int PlaySound3D(string name, Vector3 worldPos, bool loop = false, Action<int> callBack = null, int customEventID = -1)
        {
            if (string.IsNullOrEmpty(name))
            {
                return -1;
            }

            AudioUnit unit = AudioUnit.Allocate();

            if (unit == null)
            {
                return -1;
            }

            unit.SetAudio3D(gameObject, worldPos, name, loop, m_IsSoundEnable, true);
            unit.SetOnFinishListener(callBack);
            unit.customEventID = customEventID;
            return unit.id;
        }
    }
}