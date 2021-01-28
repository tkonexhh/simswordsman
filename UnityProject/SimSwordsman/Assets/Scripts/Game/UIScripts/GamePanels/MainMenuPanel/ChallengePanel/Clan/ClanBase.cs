using Qarth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class ClanBase : MonoBehaviour
    {
        [SerializeField]
        private ScrollRect m_ScrollRect;
        [SerializeField]
        private Button[] m_Buttons;
        // Start is called before the first frame update
        public virtual void Start()
        {
            m_ScrollRect.DoScrollVertical(0, 0.6f);
        }

        // Update is called once per frame
        public virtual void Update()
        {

        }
    }

}