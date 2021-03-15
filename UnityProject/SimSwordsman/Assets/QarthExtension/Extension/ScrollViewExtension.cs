using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameWish.Game
{
    public static class ScrollViewExtension
    {

        /// <summary>
		/// ֻ���þ��ȷֵ����?
        /// ��ȡScrollView��ӦVerticalNormalizedPosition����HorizontalNormalizedPosition
        /// ������vertical��horizontalͬʱ��ѡ�����?
        /// </summary>
        /// <param name="currentChildIndex">�����������е�index</param>
        /// <param name="inverse">�Ƿ����������϶��¡���������Ҫ������</param>
        /// <param name="pixelOffset">����ƫ�ƣ���������Ϊ��</param>
        /// <returns>0 ~ 1��VerticalNormalizedPosition����HorizontalNormalizedPosition</returns>
        public static float GetScrollViewNormalizedPosition(ScrollRect scrollRect, int currentChildIndex, bool inverse = false, float pixelOffset = 0)
        {
            if (scrollRect.viewport == null || scrollRect.content == null)
            {
                Debug.LogError("ScrollView��Content��ViewportΪ��");
                return inverse ? 1 : 0;
            }
            var childTrans = scrollRect.content.GetChild(0) as RectTransform;
            if (childTrans == null)
            {
                Debug.LogError("Content����û���������RectTransform");
                return inverse ? 1 : 0;
            }

            Rect viewportRect = scrollRect.viewport.rect;
            Rect contentRect = scrollRect.content.rect;
            Rect childrenRect = childTrans.rect;

            if (scrollRect.vertical && scrollRect.horizontal)
            {
                Debug.LogError("��ʱ������ScrollView��vertical��horizontalͬʱ��ѡ�����?");
                return inverse ? 1 : 0;
            }


            if (scrollRect.vertical)
            {
                VerticalLayoutGroup group = scrollRect.content.GetComponent<VerticalLayoutGroup>();
                if (group == null)
                {
                    Debug.LogError("��ȡVerticalLayoutGroupʧ��");
                    return inverse ? 1 : 0;
                }


                var diff = contentRect.height - viewportRect.height;
                float elementLength = childrenRect.height + group.spacing;


                if (inverse)
                    return Mathf.Clamp01(1 - (currentChildIndex * elementLength + pixelOffset) / diff);
                else
                    return Mathf.Clamp01((currentChildIndex * elementLength - pixelOffset) / diff);
            }


            if (scrollRect.horizontal)
            {
                HorizontalLayoutGroup group = scrollRect.content.gameObject.GetComponent<HorizontalLayoutGroup>();
                if (group == null)
                {
                    Debug.LogError("��ȡHorizontalLayoutGroupʧ��");
                    return inverse ? 1 : 0;
                }


                var diff = contentRect.width - viewportRect.width;
                float elementLength = childrenRect.width + group.spacing;


                if (inverse)
                    return Mathf.Clamp01(1 - (currentChildIndex * elementLength - pixelOffset) / diff);
                else
                    return Mathf.Clamp01((currentChildIndex * elementLength + pixelOffset) / diff);
            }


            return inverse ? 1 : 0;
        }


        public static float GetScrollViewNormalizedPosition(ScrollRect scrollRect, int currentChildIndex)
        {
            // if (scrollRect.viewport == null || scrollRect.content == null)
            // {
            //     throw new NullReferenceException("ScrollView��Content��ViewportΪ��");
            // }

            // var childTrans = scrollRect.content.GetChild(currentChildIndex) as RectTransform;
            // if (childTrans == null)
            // {
            //     throw new NullReferenceException("Content����û���������RectTransform");
            // }

            // float range = Mathf.Clamp01(childTrans.localPosition.y / scrollRect.content.sizeDelta.y);
            // return range;

            return GetScrollViewNormalizedPosition(scrollRect, scrollRect.content, currentChildIndex);
        }

        public static float GetScrollViewNormalizedPosition(ScrollRect scrollRect, RectTransform content, int currentChildIndex, float delta = 0)
        {
            if (scrollRect.viewport == null || scrollRect.content == null)
            {
                // throw new NullReferenceException("ScrollView??Content??Viewport???
                return 0;
            }
            if (content.childCount <= currentChildIndex)
            {
                return 0;
            }
            var childTrans = content.GetChild(currentChildIndex) as RectTransform;
            if (childTrans == null)
            {
                // throw new NullReferenceException("Content??????????????RectTransfo
                return 0;
            }

            if (childTrans == null)
            {
                // throw new NullReferenceException("Content??????????????RectTransfo
                return 0;
            }
            var diff = content.rect.height - scrollRect.viewport.rect.height;
            float range = Mathf.Clamp01((childTrans.localPosition.y + delta) / diff);

            return range;
        }

    }

}