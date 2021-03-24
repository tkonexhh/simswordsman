using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace GameWish.Game
{
    public class SpineHelper
    {
        public static void PlayAnim(SkeletonAnimation spine, string name, bool loop, System.Action callback)
        {
            Spine.TrackEntry trackEntry = spine.AnimationState.SetAnimation(0, name, loop);
            trackEntry.Complete += (Spine.TrackEntry entry) =>
            {
                if (callback != null)
                {
                    callback.Invoke();
                }
            };
        }

        public static bool HasAnimation(SkeletonAnimation spine, string name, string animationName)
        {
            Spine.Animation anim = spine.Skeleton.Data.FindAnimation(name);
            return anim != null;
        }
    }
}