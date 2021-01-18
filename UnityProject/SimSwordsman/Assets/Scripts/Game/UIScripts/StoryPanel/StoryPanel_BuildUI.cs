using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace GameWish.Game
{
    public partial class StoryPanel
	{
        [Header("Text")]
        [SerializeField] private Text m_ContentText;
        [SerializeField] private Text m_NameText;

        [Header("Button")]
        [SerializeField] private Button m_NextButton;
        [SerializeField] private Button m_SkipButton;

        [Header("Image")]
        [SerializeField] private Image m_BGImage;

        [Header("Shader")]
        [SerializeField] private Shader m_ReversalShader;

        //[Header("Transform")]
        //[SerializeField] private Transform m_RoleParent;

        //[Header("GameObject")]
        //[SerializeField] private GameObject[] m_RolePrefabArr;
    }
}