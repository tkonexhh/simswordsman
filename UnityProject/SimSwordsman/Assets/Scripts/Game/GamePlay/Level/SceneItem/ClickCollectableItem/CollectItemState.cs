using System;
using Qarth;
using UnityEngine;


namespace GameWish.Game
{
	public class CollectItemState : MonoBehaviour
    {
        [SerializeField]
        private GameObject Lotus;
        [SerializeField]
        private GameObject Lotus_Full;

        private void Awake()
        {
            EventSystem.S.Register(EventID.OnChangeCollectLotusState1, OnChnageState1);
            EventSystem.S.Register(EventID.OnChangeCollectLotusState2, OnChnageState2);
        }

        private void OnChnageState1(int key, object[] param)
        {
            if (!Lotus.gameObject.activeSelf)
            {
                Lotus.gameObject.SetActive(true);
                Lotus_Full.gameObject.SetActive(false);
            }
        }

        private void OnChnageState2(int key, object[] param)
        {
            if (!Lotus_Full.gameObject.activeSelf)
            {
                Lotus_Full.gameObject.SetActive(true);
                Lotus.gameObject.SetActive(false);
            }
        }
    }
}