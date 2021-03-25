using Qarth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class TacticalFunctionBtn : MonoBehaviour
	{
		[SerializeField]
		private Button m_TacticalFunctionBtn;
		[SerializeField]
		private Image m_TacticalImg;
		[SerializeField]
		private Text m_TacticalName;

		public int TaskID = -1;
		private SimGameTask m_SimGameTask = null;

		public SimGameTask SimGameTask { get { return m_SimGameTask; } }

		void Start()
	    {  
			m_TacticalFunctionBtn.onClick.AddListener(()=> {
				UIMgr.S.OpenPanel(UIID.TacticalFunctionPanel, m_SimGameTask);
			});
        }
	
	    // Update is called once per frame
	    void Update()
	    {
	        
	    }

        public void OnInit(MainMenuPanel mainMenuPanel, SimGameTask simGameTask)
        {
			m_SimGameTask = simGameTask;
			TaskID = m_SimGameTask.TaskId;
			m_TacticalImg.sprite = mainMenuPanel.FindSprite("enemy_icon_" + m_SimGameTask.CommonTaskItemInfo.iconRes);
            m_TacticalName.text  = m_SimGameTask.CommonTaskItemInfo.title;
		}
    }
	
}