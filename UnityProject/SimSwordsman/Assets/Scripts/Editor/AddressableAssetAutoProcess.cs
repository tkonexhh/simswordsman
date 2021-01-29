﻿//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using UnityEngine.AddressableAssets;
//using UnityEditor.AddressableAssets.Settings;
//using Qarth;
//using System.Linq;
//using System.Data;

//namespace Qarth.Editor
//{
//    public class AddressableAssetAutoProcess : AssetPostprocessor
//    {
//        private static AddressableAssetSettings setting = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");

//        private static string addressableFolderName = "AddressableAssets";
//        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deleteAsset, string[] movedAssets, string[] movedFromAssetPaths)
//        {
//            //for (int i = 0; i < importedAsset.Length; i++)
//            //{
//            //    Log.i("importedAsset : " + importedAsset[i]);
//            //}

//            //for (int i = 0; i < movedAssets.Length; i++)
//            //{
//            //    Log.i("movedAssets : " + movedAssets[i]);
//            //}

//            ProcessImportedAssets(importedAsset);
//            ProcessMovedAsset(movedAssets, movedFromAssetPaths);
//        }

//        private static void ProcessImportedAssets(string[] assetPath)
//        {
//            if (assetPath == null || assetPath.Length == 0)
//            {
//                return;
//            }

//            for (int i = 0; i < assetPath.Length; ++i)
//            {
//                if (CheckIsRes4Addresable(assetPath[i]))
//                {
//                    ProcessAssetGroup(assetPath[i]);
//                }
//            }
//        }

//        private static void ProcessMovedAsset(string[] movedAssets, string[] movedFromAssets)
//        {

//            if (movedAssets != null && movedAssets.Length > 0)
//            {
//                for (int i = 0; i < movedAssets.Length; ++i)
//                {
//                    ProcessAssetGroup(movedAssets[i], movedFromAssets[i]);
//                }
//            }
//        }


//        private static bool CheckIsRes4Addresable(string name)
//        {
//            if (name.StartsWith("Assets/") && name.Contains("/" + addressableFolderName + "/"))
//            {
//                return true;
//            }

//            return false;
//        }

//        private static void ProcessAssetGroup(string assetPath)
//        {
//            AssetImporter ai = AssetImporter.GetAtPath(assetPath);
//            if (ai == null)
//            {
//                Log.e("Not Find Asset:" + assetPath);
//                return;
//            }

//            string fullPath = EditorUtils.AssetsPath2ABSPath(assetPath);
//            if (Directory.Exists(fullPath))
//            {
//                return;
//            }

//            string groupName = string.Empty;

//            string dirName = Path.GetDirectoryName(assetPath);


//            if (dirName.Contains("FolderMode"))
//            {
//                int index = dirName.LastIndexOf('\\');
//                string assetBundleName = dirName.Substring(index, dirName.Length - index);
//                if (assetBundleName.Contains("\\"))
//                {
//                    assetBundleName = assetBundleName.Replace("\\", "");
//                }
//                //string assetBundleName = EditorUtils.AssetPath2ReltivePath(dirName).ToLower();
//                //assetBundleName = assetBundleName.Replace(addressableFolderName + "/", "");

//                groupName = assetBundleName;
//            }
//            else
//            {
//                groupName = setting.DefaultGroup.name;
//            }

//            groupName = groupName.Replace("/", "-");
//            var group = setting.FindGroup(groupName);
//            if (group == null)
//            {
//                //Debug.LogError("ProcessAssetGroup:" + groupName);
//                //group = setting.CreateGroup(groupName, false, false, false, null);
//                group = setting.CreateGroup(groupName, false, false, false, new List<AddressableAssetGroupSchema> { setting.DefaultGroup.Schemas[0] }, typeof(SchemaType));
//            }

//            if (group == null)
//            {
//                return;
//            }

//            var guid = AssetDatabase.AssetPathToGUID(assetPath);
//            var entry = setting.CreateOrMoveEntry(guid, group);
//            entry.SetAddress(PathHelper.FileNameWithoutSuffix(Path.GetFileName(assetPath)), true);
//            //EditorUtility.SetDirty(setting);
//        }

//        /// <summary>
//        /// 处理移动
//        /// </summary>
//        /// <param name="assetPath"></param>
//        /// <param name="moveFromPath"></param>
//        private static void ProcessAssetGroup(string assetPath, string moveFromPath)
//        {
//            AssetImporter ai = AssetImporter.GetAtPath(assetPath);
//            if (ai == null)
//            {
//                Log.e("Not Find Asset:" + assetPath);
//                return;
//            }

//            string fullPath = EditorUtils.AssetsPath2ABSPath(assetPath);
//            if (Directory.Exists(fullPath))
//            {
//                return;
//            }

//            if (CheckIsRes4Addresable(assetPath))//如果移动到了另一个资源文件夹
//            {
//                ProcessAssetGroup(assetPath);
//            }
//            else
//            {
//                var guid = AssetDatabase.AssetPathToGUID(assetPath);
//                setting.RemoveAssetEntry(guid);
//            }

//            if (CheckIsRes4Addresable(moveFromPath))
//            {
//                //处理移动前的Group
//                string removeFromGroupName = string.Empty;
//                string dirName = Path.GetDirectoryName(moveFromPath);

//                int index = dirName.LastIndexOf('\\');
//                string assetBundleName = dirName.Substring(index, dirName.Length - index);
//                if (assetBundleName.Contains("\\"))
//                {
//                    assetBundleName = assetBundleName.Replace("\\", "");
//                }

//                if (moveFromPath.Contains("FolderMode"))
//                {
//                    removeFromGroupName = assetBundleName;
//                }
//                else
//                {
//                    removeFromGroupName = setting.DefaultGroup.name;
//                }
//                removeFromGroupName = removeFromGroupName.Replace("/", "-");
//                //Debug.LogError("removeFromGroupName:" + removeFromGroupName);
//                var group = setting.FindGroup(removeFromGroupName);
//                if (group != null)
//                {
//                    if (group.entries.Count == 0)
//                    {
//                        setting.RemoveGroup(group);
//                    }

//                }
//            }

//            //EditorUtility.SetDirty(setting);
//        }
//    }
//}
