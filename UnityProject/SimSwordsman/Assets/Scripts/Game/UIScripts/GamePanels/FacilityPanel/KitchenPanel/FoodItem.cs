using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class FoodItem : MonoBehaviour, ItemICom
    {
        [SerializeField]
        private Image m_FoodImg;
        [SerializeField]
        private Text m_FoodNameTxt;
        [SerializeField]
        private Text m_FoodContTxt;
        [SerializeField]
        private Text m_FoodEffecTxt;
        [SerializeField]
        private Button m_MakeBtn;
        [SerializeField]
        private Button m_MakeADBtn;
        [SerializeField]
        private Text m_EffectiveTimeTxt;
        [SerializeField]
        private Text m_ADEffectiveTimeTxt;
        [SerializeField]
        private Text m_DurationTxt;

        [SerializeField]
        private Transform m_DurationTra;
        [HideInInspector]
        public int ID;

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
            BindAddListenerEvent();
            Init((int)obj[0]);
        }
        
        public void StartEffect(string dur)
        {
            SetState(true);
            m_DurationTxt.text = dur;
        }
        public void StopEffect()
        {
            SetState(false);
        }
        public void Countdown(string dur)
        {
            m_DurationTxt.text = dur;
        }


        void Init(int id)
        {
            ID = id;
            TDFoodConfig tb = TDFoodConfigTable.GetData(id);
            //m_FoodImg.sprite = 
            m_FoodNameTxt.text = tb.name;
            m_FoodContTxt.text = tb.desc;
            m_FoodEffecTxt.text = BuffSystem.S.GetEffectDesc(tb);
            m_EffectiveTimeTxt.text = GetTimeDesc(tb.buffTime) + "有效";
            m_ADEffectiveTimeTxt.text = GetTimeDesc(tb.buffTimeAD) + "有效";

            if (BuffSystem.S.IsActive(ID))
            {
                string dur = BuffSystem.S.GetCurrentCountdown(ID);
                if (dur != null)
                {
                    SetState(true);
                    m_DurationTxt.text = dur;
                }
                else
                {
                    SetState(false);
                }
            }
        }

        string GetTimeDesc(int minute)
        {
            if (minute >= 60)
                return minute / 60.0f + "小时";
            else
                return minute + "分钟";
        }

        public void SetButtonEvent(Action<object> action)
        {
        }

        private void BindAddListenerEvent()
        {
            m_MakeBtn.onClick.AddListener(() => 
            {
                //判断材料

                
                BuffSystem.S.StartBuff(ID);
            });
            m_MakeADBtn.onClick.AddListener(() => 
            {
                //判断材料


                FloatMessage.S.ShowMsg("这里应该显示广告");
                BuffSystem.S.StartBuff(ID, true);
            });
        }

        void SetState(bool active)
        {
            if (active)
            {
                m_DurationTxt.gameObject.SetActive(true);
                m_DurationTra.gameObject.SetActive(true);
                m_MakeBtn.gameObject.SetActive(false);
                m_MakeADBtn.gameObject.SetActive(false);
            }
            else
            {
                m_DurationTxt.gameObject.SetActive(false);
                m_DurationTra.gameObject.SetActive(false);
                m_MakeBtn.gameObject.SetActive(true);
                m_MakeADBtn.gameObject.SetActive(true);
            }
        }
    }

}

