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
        public Text Count;

        int count;


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
                if (count >= tb.collectMin)
                {
                    if (!CollectView.gameObject.activeSelf)
                        CollectView.gameObject.SetActive(true);

                    Count.text = count.ToString();
                }
                else if(CollectView.gameObject.activeSelf)
                {
                    CollectView.gameObject.SetActive(false);
                }
            }
        }

        public void OnClicked()
        {
            CollectSystem.S.Collect(id);
            CollectView.gameObject.SetActive(false);
        }
	}
	
}