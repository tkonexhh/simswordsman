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

        private void Start()
        {
            m_Mat = m_ProgressBar.GetComponent<SpriteRenderer>().material;
        }

        public void SetPercent(float percent)
        {
            m_Mat.SetFloat("_FillPercent", percent);
        }
    }

}