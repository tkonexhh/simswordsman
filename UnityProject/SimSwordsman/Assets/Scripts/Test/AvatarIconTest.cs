using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    public class AvatarIconTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var datas = TDAvatarTable.dataList;
            for (int i = 0; i < datas.Count; i++)
            {
                string iconName = datas[i].headIcon;
                var sprite = SpriteHandler.S.GetSprite("EnmeyHeadIconsAtlas", "enemy_icon_" + iconName);
                Debug.LogError(datas[i].id + ":" + sprite.name);
            }
        }


    }

}