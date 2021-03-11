using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qarth;

namespace GameWish.Game
{
	public class CharacterWorkProgressBar : MonoBehaviour
	{
        [SerializeField]
        private GameObject m_ProgressBar = null;

        private Material m_Mat;

        public Material Mat 
        {
            get {
                if (m_Mat == null) {
                    m_Mat = m_ProgressBar.GetComponent<SpriteRenderer>().material;
                    SetPercent(0);
                }

                return m_Mat;
            }
        }

        private void OnEnable()
        {
            SetPercent(0);
        }

        public void SetPercent(float percent)
        {
            if (Mat != null) {
                Mat.SetFloat("_FillPercent", percent);
            }            
        }
    }

}