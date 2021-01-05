using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public enum PracticeFieldState
    {
        None,
        /// <summary>
        /// ������
        /// </summary>
        Free,
        /// <summary>
        /// δ����
        /// </summary>
        NotUnlocked,
        /// <summary>
        /// ������
        /// </summary>
        CopyScriptures,
        /// <summary>
        /// ������
        /// </summary>
        Practice,
    }


    public class PracticeDisciple : MonoBehaviour,ItemICom
	{
        [SerializeField]
        private Text m_PracticePos;
        [SerializeField]
        private Text m_Time;
        [SerializeField]
        private Text m_CurPractice;
        [SerializeField]
        private Image m_PracticeImg;
        [SerializeField]
        private Button m_PracticeBtn;

        private int m_CurLevel;
        private PracticeField m_PracticeFieldInfo = null;
        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {

            m_PracticeFieldInfo = t as PracticeField;
            m_CurLevel = (int)obj[0];

            BindAddListenEvent();

            m_PracticePos.text = "����λ:" + m_PracticeFieldInfo.Index;
            RefreshPracticeFieldState();
        }

     

        private void BindAddListenEvent()
        {
            m_PracticeBtn.onClick.AddListener(()=> {
                UIMgr.S.OpenPanel(UIID.ChooseDisciplePanel, m_PracticeFieldInfo);
            });
        }

        private void Callback(AbstractPanel obj)
        {
            throw new NotImplementedException();
        }

        public void RefreshPracticeFieldState()
        {

            switch (m_PracticeFieldInfo.PracticeFieldState)
            {
                case PracticeFieldState.None:
                    break;
                case PracticeFieldState.Free:
                    m_PracticeBtn.enabled = true;
                    m_CurPractice.text = "���ŵ���";
                    m_Time.text = "����";
                    break;
                case PracticeFieldState.NotUnlocked:
                    m_PracticeBtn.enabled = false;
                    m_CurPractice.text = "������" + m_PracticeFieldInfo.UnlockLevel + "�������";
                    m_Time.text = Define.COMMON_DEFAULT_STR;
                    break;
                case PracticeFieldState.CopyScriptures:
                    break;
                case PracticeFieldState.Practice:
                    m_CurPractice.text = "��ǰѵ��:" + m_PracticeFieldInfo.CharacterItem.name;
                    //StartCoroutine(BattleCountdown(m_PracticeFieldInfo.PracticeTime));
                    m_PracticeBtn.enabled = true;
                    break;
                default:
                    break;
            }
        }

        public IEnumerator BattleCountdown(int second)
        {
            while (second >= 0)
            {
                if (second <= 5)
                {
                    //m_CombatTime.color = Color.red;
                    //m_CombatTime.Alpha
                }

                m_Time.text = SplicingTime(second);
                yield return new WaitForSeconds(1);
                second--;
            }
        }
        /// <summary>
        /// ƴ��ʱ��
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public string SplicingTime(int second)
        {
            if (second.ToString().Length > 1)
                return "00:" + second;
            else
                return "00:0" + second;
        }


        public void SetButtonEvent(Action<object> action)
        {
            throw new NotImplementedException();
        }
	}
	
}