using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimator
{
    private string[] animationNameArray;
    private SkeletonAnimation skeletonAnimation;

    public SpineAnimator(SkeletonAnimation skeletonAnimation, string[] animationNameArray)
    {
        this.skeletonAnimation = skeletonAnimation;
        this.animationNameArray = animationNameArray;
    }
    public void SetAnimation(int animIndex, bool loop, float timeScale = 1f)
    {
        skeletonAnimation.AnimationState.TimeScale = timeScale;
        skeletonAnimation.AnimationState.SetAnimation(0, animationNameArray[animIndex], loop);
    }
    public bool IsCurrentAnimation(int animIndex)
    {
        if (skeletonAnimation.AnimationState.GetCurrent(0) == null) return false;
        if (skeletonAnimation.AnimationState.GetCurrent(0).ToString() == animationNameArray[animIndex])
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
