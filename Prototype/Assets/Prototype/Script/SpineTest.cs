using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineTest : MonoBehaviour
{
    private SkeletonAnimation spine;
    private void Awake()
    {
        spine = FindObjectOfType<SkeletonAnimation>();
        
    }
    public void BtnEvt_Attack()
    {
        spine.AnimationState.SetAnimation(1, "attack", true);
    }
    public void BtnEvt_Run()
    {
        spine.AnimationState.SetAnimation(1, "run", true);
    }
    public void BtnEvt_Dead()
    {
        spine.AnimationState.SetAnimation(1, "dead", false);
    }
    public void BtnEvt_Blink()
    {
        spine.AnimationState.SetAnimation(0, "eye blink", true);
    }
}
