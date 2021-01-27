using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using System;

namespace GameWish.Game
{
    /// <summary>
    /// 敏感词检测
    /// </summary>
    public class FilterSensitiveWords
    {
        static Dictionary<char, IList<string>> keyDict;

        private const string keywordsPath = "Illegal/IllegalKeywords";
        private const string urlsPath = "Illegal/IllegalUrls";

        public static void Initialize()
        {
            if (keyDict != null && keyDict.Count > 0)
                return;

            TextAsset words1 = Resources.Load(keywordsPath) as TextAsset;
            TextAsset words2 = Resources.Load(urlsPath) as TextAsset;
            var words = new List<string>();
            foreach (var item in words1.text.Split(new string[] { "\n" }, StringSplitOptions.None))
            {
                words.Add(item.Trim());
            }
            foreach (var item in words2.text.Split(new string[] { "\n" }, StringSplitOptions.None))
            {
                words.Add(item.Trim());
            }
            keyDict = new Dictionary<char, IList<string>>();
            foreach (string s in words)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                if (keyDict.ContainsKey(s[0]))
                    keyDict[s[0]].Add(s.Trim(new char[] { '\r' }));
                else
                    keyDict.Add(s[0], new List<string> { s.Trim(new char[] { '\r' }) });
            }
        }

        //判断一个字符串是否包含敏感词，包括含的话将其替换为*
        public static bool IsContainSensitiveWords(ref string text, out string SensitiveWords)
        {
            bool isFind = false;
            SensitiveWords = "";
            if (string.IsNullOrEmpty(text))
                return isFind;

            int len = text.Length;
            StringBuilder sb = new StringBuilder(len);
            bool isOK = true;
            for (int i = 0; i < len; i++)
            {
                if (keyDict.ContainsKey(text[i]))
                {
                    foreach (string s in keyDict[text[i]])
                    {
                        isOK = true;
                        int j = i;
                        foreach (char c in s)
                        {
                            if (j >= len || c != text[j++])
                            {
                                isOK = false;
                                break;
                            }
                        }
                        if (isOK)
                        {
                            SensitiveWords = s;
                            isFind = true;
                            i += s.Length - 1;
                            sb.Append('*', s.Length);
                            break;
                        }

                    }
                    if (!isOK)
                        sb.Append(text[i]);
                }
                else
                    sb.Append(text[i]);
            }
            if (isFind)
                text = sb.ToString();

            return isFind;
        }
    }
}