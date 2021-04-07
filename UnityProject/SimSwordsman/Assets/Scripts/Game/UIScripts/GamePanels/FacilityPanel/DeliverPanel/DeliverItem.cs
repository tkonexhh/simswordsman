using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class DeliverItem : MonoBehaviour
	{
		[SerializeField]
		private Image m_DeliverPhoto;
		[SerializeField]
		private Text m_DeliverTitle;
		[SerializeField]
		private Transform m_DeliverTra;
		[SerializeField]
		private GameObject m_DeliverDisciple;

		[Header("StateStart")]
		[SerializeField]
		private GameObject m_StateStart;
		[SerializeField]
		private Transform m_ResTra;
		[SerializeField]
		private GameObject m_ResItem;
		[SerializeField]
		private Button m_QuickStart;

		[Header("StateStarting")]
		[SerializeField]
		private GameObject m_StateStarting;
		[SerializeField]
		private Text m_CountDown;
		[SerializeField]
		private Slider m_CountDownSlider;
		[SerializeField]
		private Button m_DoubleSpeedBtn;
		[Header("StateLock")]
		[SerializeField]
		private Text m_StateLock;

		// Start is called before the first frame update
		void Start()
		{

		}

		public void OnInt() { }	
	    // Update is called once per frame
	    void Update()
	    {
	        
	    }
	}
	
}