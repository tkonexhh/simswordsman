using Spine;
using Spine.Unity;

namespace GameWish.Game
{
    public class SpineHelper
    {
        public static void PlayAnim(SkeletonAnimation spine, string name, bool loop, System.Action callback = null)
        {
            Spine.TrackEntry trackEntry = spine.AnimationState.SetAnimation(0, name, loop);
            trackEntry.Complete += (Spine.TrackEntry entry) =>
            {
                callback?.Invoke();
            };
        }

        public static bool HasAnimation(SkeletonAnimation spine, string animationName)
        {
            Spine.Animation anim = spine.Skeleton.Data.FindAnimation(animationName);
            return anim != null;
        }

        public static TrackEntry PlayAnim(SkeletonGraphic spine, string name, bool loop, System.Action endCallBack = null)
        {
            Spine.TrackEntry trackEntry = spine.AnimationState.SetAnimation(0, name, loop);
            trackEntry.Complete += (Spine.TrackEntry entry) =>
            {
                endCallBack?.Invoke();
            };

            return trackEntry;
        }
    }
}
