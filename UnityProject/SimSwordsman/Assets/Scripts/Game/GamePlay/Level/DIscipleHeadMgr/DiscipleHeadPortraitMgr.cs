using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameWish.Game
{
	public class DiscipleHeadPortraitMgr : TMonoSingleton<DiscipleHeadPortraitMgr>
	{
		private ResLoader m_ResLoader;
		public override void OnSingletonInit()
		{
			base.OnSingletonInit();
			m_ResLoader = ResLoader.Allocate("DiscipleHeadPortraitMgr", null);
		}

		public GameObject CreateDiscipleHeadIcon(CharacterItem characterItem, Transform parent, Vector3 pos, Vector3 scall, bool isFirst = true)
		{
			DiscipleHeadPortrait discipleHeadPortrait = Instantiate(GetDiscipleHeadPortrait(characterItem), parent).GetComponent<DiscipleHeadPortrait>();
			discipleHeadPortrait.OnInit(true);
			if (isFirst)
				discipleHeadPortrait.transform.SetSiblingIndex(0);
			discipleHeadPortrait.transform.localPosition = pos;
			discipleHeadPortrait.transform.localScale = scall;
			return discipleHeadPortrait.gameObject;
		}

		public GameObject GetDiscipleHeadPortrait(CharacterItem characterItem)
		{
			string iconName = string.Empty;

			//给试炼默认弟子图片使用
			if (characterItem == null)
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
			try
			{
				if (!GameObjectPoolMgr.S.group.HasPool(iconName))
					GameObjectPoolMgr.S.AddPool(iconName, (GameObject)m_ResLoader.LoadSync(iconName), 10, 2);
				return GameObjectPoolMgr.S.Allocate(iconName);
			}
			catch (System.Exception)
			{
				Log.e("未找到资源,Name = " + iconName);
				return (GameObject)m_ResLoader.LoadSync("Head_good_1_0_4");
			}
			//return (GameObject)m_ResLoader.LoadSync("Head_good_1_0_4");
		}
	}


}