using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Qarth;

namespace GameWish.Game
{

	public class DiscipleHeadPortraitMgr : TSingleton<DiscipleHeadPortraitMgr>
	{
		private ResLoader m_ResLoader;
		public override void OnSingletonInit()
        {
            base.OnSingletonInit();
			m_ResLoader = ResLoader.Allocate("DiscipleHeadPortraitMgr", null);
		}

		public GameObject GetDiscipleHeadPortrait(CharacterItem characterItem)
		{
			string iconName = string.Empty;

			//给试炼默认弟子图片使用
			if (characterItem==null)
            {
				iconName = "Head_good_1_0_4";
			}
            else
			{
				switch (characterItem.quality)
				{
					case CharacterQuality.Normal:
					case CharacterQuality.Good:
					case CharacterQuality.Perfect:
						iconName = "Head_" + characterItem.quality.ToString().ToLower() + "_" + characterItem.bodyId +
							"_0_" + characterItem.headId;
						break;
					case CharacterQuality.Hero:
						iconName = "Head_hero_" + characterItem.bodyId +
					"_" + ((int)characterItem.heroClanType) + "_" + characterItem.headId;
						break;
					default:
						break;
				}
			}
		
            if (!GameObjectPoolMgr.S.group.HasPool(iconName))
                GameObjectPoolMgr.S.AddPool(iconName, (GameObject)m_ResLoader.LoadSync(iconName), 10, 2);
            return GameObjectPoolMgr.S.Allocate(iconName);

            //return (GameObject)m_ResLoader.LoadSync("Head_good_1_0_4");
        }
	}

	public class DiscipleHeadPortrait : MonoBehaviour
	{
		[SerializeField]
		private Image m_Head;
		[SerializeField]
		private Image m_Body;
		[SerializeField]
		private Mask m_Mask;
		private CharacterItem m_CharacterItem;
		void Start()
        {
			
		}

        // Update is called once per frame
        void Update()
	    {
	        
	    }

		public void OnInit(bool head)
		{
            if (head)
            {
				m_Mask.enabled = true;
			}
		}
	}
	
}