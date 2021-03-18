
using System.IO;
using Qarth;
using Qarth.Editor;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;


namespace GameWish.Game
{
    public class BulidAtlas
    {
        [MenuItem("Assets/Qarth Builder/Build SpritesAtlas")]
        static void BuidSpritesAtlas()
        {
            string folderPath = EditorUtils.GetSelectedDirAssetsPath();
            DirectoryInfo dInfo = new DirectoryInfo(EditorUtils.AssetsPath2ABSPath(folderPath));
            DirectoryInfo[] subFolders = dInfo.GetDirectories();
            if (subFolders == null || subFolders.Length == 0)
            {
                BuildSpritesAtlas(folderPath);
            }
            else
            {
                for (int i = 0; i < subFolders.Length; ++i)
                {
                    BuildSpritesAtlas(EditorUtils.ABSPath2AssetsPath(subFolders[i].FullName));
                }
            }
        }
        public static void BuildSpritesAtlas(string folderPath)
        {
            SpritesData data = null;

            string folderName = PathHelper.FullAssetPath2Name(folderPath);
            string spriteDataPath = folderPath + "/" + folderName + folderName + ".spriteatlas";

            if (!File.Exists(spriteDataPath))
            {
                SpriteAtlas atlas = new SpriteAtlas();
                SpriteAtlasPackingSettings packSet = new SpriteAtlasPackingSettings()
                {
                    enableRotation = false,
                    enableTightPacking = false,
                    padding = 8,
                };
                atlas.SetPackingSettings(packSet);
                TextureImporterPlatformSettings platform = new TextureImporterPlatformSettings()
                {
                    maxTextureSize = 2048,
                    format = TextureImporterFormat.Automatic,
                    compressionQuality = 50,
                    crunchedCompression = true,
                    textureCompression = TextureImporterCompression.Compressed,
                };
                atlas.SetPlatformSettings(platform);
                AssetDatabase.CreateAsset(atlas, spriteDataPath);
                UnityEngine.Object texture = AssetDatabase.LoadMainAssetAtPath(folderPath);
                SpriteAtlasExtensions.Add(atlas, new UnityEngine.Object[] { texture });
                AssetDatabase.SaveAssets();
            }

        }
    }

}