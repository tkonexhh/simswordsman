using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
    /// <summary>
    /// 检测所有功夫的sprite是否能够正常加载
    /// </summary>
    public class KungFuIconTest : MonoBehaviour
    {

        void Start()
        {
            var datas = TDKongfuConfigTable.dataList;
            for (int i = 0; i < datas.Count; i++)
            {
                string iconName = datas[i].iconName;
                var sprite = SpriteHandler.S.GetSprite("MartialArtsAtlas", iconName);
                Debug.LogError(datas[i].id + ":" + sprite.name);
            }
        }


    }

}