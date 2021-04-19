using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public class TowerEnemyIcon : MonoBehaviour
    {
        [SerializeField] private Image m_ImgBg;
        [SerializeField] private Image m_ImgEnemyIcon;
        [SerializeField] private Text m_TxtID;


        public void SetEnemy(int id)
        {
            m_TxtID.text = id.ToString();
        }

        public void SetGrey(bool isGrey, Material greyMat)
        {
            if (isGrey)
            {
                m_ImgBg.material = greyMat ?? null;
                m_ImgEnemyIcon.material = greyMat ?? null;
            }
            else
            {
                m_ImgBg.material = null;
                m_ImgEnemyIcon.material = null;
            }
        }
    }

}