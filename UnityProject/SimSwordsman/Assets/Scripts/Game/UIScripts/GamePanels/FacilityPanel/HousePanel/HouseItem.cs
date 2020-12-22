using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class HouseItem : MonoBehaviour
    {
        [SerializeField]
        private Text m_HouseLevel;
        [SerializeField]
        private Text m_ResidentsNumberValue;
        [SerializeField]
        private Text m_NextResidentsNumberValue;
        [SerializeField]
        private Text m_Restrictions;
        [SerializeField]
        private Text m_UpgradeResourcesValue;


        [SerializeField]
        private Button m_UpgradeBtn;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnInit()
        {
            
        }

    }

}