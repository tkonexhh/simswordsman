using Spine.Unity;
using UnityEngine;


namespace GameWish.Game
{
    public class UISpineShaderRebind : MonoBehaviour
    {
        void Start()
        {
#if UNITY_EDITOR
            RebindShader();
#endif
        }

        public void RebindShader()
        {
            SkeletonGraphic[] spines = GetComponentsInChildren<SkeletonGraphic>();
            foreach (SkeletonGraphic spine in spines)
            {
                Material material = spine.material;
                if (material != null)
                {
                    var shaderName = material.shader.name;
                    var newShader = Shader.Find(shaderName);
                    if (newShader != null)
                    {
                        material.shader = newShader;
                    }
                    else
                    {
                        Debug.LogWarning("unable to refresh shader: " + shaderName + " in material " + material.name);
                    }
                }
            }
        }
    }
}
