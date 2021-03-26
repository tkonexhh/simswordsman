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
                CollectView.SetActive(true);
            }
        }

        public void OnClicked()
        {
            CollectSystem.S.Collect(id);
            isGuide = false;
            CollectView.gameObject.SetActive(false);
            GameDataMgr.S.GetPlayerData().recordData.AddCollect();
            PlayerPrefs.SetInt(Define.IsClickCollectSytemBubble, 1);
        }
    }
}