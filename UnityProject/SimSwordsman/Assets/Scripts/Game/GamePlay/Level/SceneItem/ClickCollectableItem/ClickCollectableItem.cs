using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;
using UnityEngine.UI;
using System;

namespace GameWish.Game
{
	public class ClickCollectableItem : MonoBehaviour
	{
        public int id;
        public GameObject CollectView;

        bool isGuide = false;

        private void Awake()
        {
            EventSystem.S.Register(EventID.OnCollectCountChange, OnCountChange);
        }

        private void OnCountChange(int key, object[] param)
        {
            int _id = (int)param[0];
            if (_id == id)
            {
                var tb = TDCollectConfigTable.dataList[id];
                int count = (int)param[1];
                if (param.Length > 2)
                {
                    //Òýµ¼
                    isGuide = true;
                }
                if (isGuide)
                {
                    CollectView.gameObject.SetActive(true);
                }
                else
                {
                    if (count >= tb.collectMin)
                    {
                        if (!CollectView.gameObject.activeSelf)
                            CollectView.gameObject.SetActive(true);
                    }
                    else if (CollectView.gameObject.activeSelf)
                        CollectView.gameObject.SetActive(false);
                }
            }
        }

        public void OnClicked()
        {
            CollectSystem.S.Collect(id);
            isGuide = false;
            CollectView.gameObject.SetActive(false);
        }
	}
	
}