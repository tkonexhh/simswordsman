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


        public void SetEnemy(Sprite icon)
        {
            m_ImgEnemyIcon.sprite = icon;
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