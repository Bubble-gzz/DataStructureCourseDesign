using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnimator : Animation
{
    public float sec;
    public WaitAnimatorInfo info;

    protected override IEnumerator Animate()
    {
        WaitAnimatorInfo info = this.info;
        float sec = this.sec * Settings.animationTimeScale;
        yield return new WaitForSeconds(sec);
        info.completed = true;
    }
}
