using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
    public class AbstractFullPanel : AbstractPanel
    {
        protected override void OnOpen()
        {
            base.OnOpen();
            //关闭游戏摄像机
            MainGameMgr.S.MainCamera.CloseGameCamera();
            InputMgr.S.DisableInput();
        }

        protected override void OnClose()
        {
            base.OnClose();
            //打开游戏摄像机
            MainGameMgr.S.MainCamera.OpenGameCamera();
            InputMgr.S.EnableInput();
        }
    }

}