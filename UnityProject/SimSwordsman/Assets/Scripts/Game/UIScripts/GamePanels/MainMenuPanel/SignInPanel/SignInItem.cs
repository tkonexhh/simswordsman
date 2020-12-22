using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
	public class SignInItem : MonoBehaviour
	{
		[SerializeField]
		private Text m_SignInTime;
		[SerializeField]
		private Text m_SignInReward;
		[SerializeField]
		private Text m_SignInCompleteCont;

		[SerializeField]
		private GameObject SignInResult;

		[HideInInspector]
		public bool isSignInState = false;

	    // Start is called before the first frame update
	    void Start()
	    {
	        
	    }
	
	    // Update is called once per frame
	    void Update()
	    {
	        
	    }

		public void SetSignInState()
		{
			isSignInState = true;
			SignInResult.SetActive(true);
		}


	}
	
}