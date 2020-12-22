using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Qarth;
namespace GameWish.Game
{
	public class TaskBackImg : MonoBehaviour, ItemICom
	{
		[SerializeField]
		private Image m_TaskPhoto;
		[SerializeField]
		private Text m_TaskName;
		[SerializeField]
		private Text m_TaskIntrodution;

		[SerializeField]
		private Button FuncBtn;

		private SimGameTask m_CurTaskInfo;

		private Action<object> m_Action;
		// Start is called before the first frame update
		void Start()
	    {
            
	    }

        private void BindAddListenerEvevnt()
        {
			FuncBtn.onClick.AddListener(()=> {
				m_Action?.Invoke(m_CurTaskInfo);
			});

		}

        // Update is called once per frame
        void Update()
	    {
	        
	    }

		public void InitItem(MainTaskItemInfo mainTask) {
		
		}

        public void OnInit<T>(T t, Action action = null, params object[] obj)
        {
			m_CurTaskInfo = t as SimGameTask;
			m_TaskName.text = m_CurTaskInfo.MainTaskItemInfo.title;
			m_TaskIntrodution.text = m_CurTaskInfo.MainTaskItemInfo.id.ToString();
			BindAddListenerEvevnt();
		}

        public void SetButtonEvent(Action<object> action)
        {
			m_Action = action;
		}
    }
}