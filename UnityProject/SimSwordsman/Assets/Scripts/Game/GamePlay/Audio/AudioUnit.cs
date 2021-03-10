using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Qarth
{
	public partial class AudioMgr 
    {
        public partial class AudioUnit
        {
            private GameObject m_Root;

            public void SetAudio3D(GameObject root, Vector3 worldPos, string name, bool loop, bool isEnable, bool is3DSound)
            {
                if (string.IsNullOrEmpty(name))
                {
                    return;
                }

                if (m_Name == name)
                {
                    return;
                }

                if (m_Source == null)
                {
                    if (m_Root == null)
                    {
                        m_Root = new GameObject("soundObj");
                        m_Root.transform.SetParent(root.transform);
                        m_Source = m_Root.AddComponent<AudioSource>();
                    }

                    if (!isEnable)
                    {
                        m_Source.enabled = isEnable;
                    }
                }

                if (is3DSound)
                {
                    m_Source.spatialBlend = 1;
                    //m_Source.minDistance = 0.7f;
                    //m_Source.maxDistance = 10;//暂定自己项目中的参数

                    m_Source.rolloffMode = AudioRolloffMode.Linear;
                    m_Source.minDistance = 8f;
                    m_Source.maxDistance = 16;//暂定自己项目中的参数
                }
                else
                {
                    m_Source.spatialBlend = 0;
                    m_Source.rolloffMode = AudioRolloffMode.Logarithmic;
                    m_Source.minDistance = 1;
                    m_Source.maxDistance = 500;
                }

                SetRootPos(worldPos, root.transform);

                //防止卸载后立马加载的情况
                ResLoader preLoader = m_Loader;
                m_Loader = null;
                CleanResources();

                RegisterActiveAudioUnit(this);

                m_Loader = ResLoader.Allocate("AudioUnit");

                m_IsLoop = loop;
                m_Name = name;

                m_Loader.Add2Load(name, OnResLoadFinish);

                if (preLoader != null)
                {
                    preLoader.Recycle2Cache();
                    preLoader = null;
                }

                if (m_Loader != null)
                {
                    m_Loader.LoadAsync();
                }
            }

            public void SetRoot(GameObject root)
            {
                this.m_Root = root;
            }

            public void SetRootPos(Vector3 pos, Transform root)
            {
                if (m_Root != null)
                {
                    m_Root.transform.position = pos;
                }
                else
                {
                    root.transform.position = pos;
                }
            }
        }
    }
}