using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnimator : Animation
{
    public float sec;
    public WaitAnimatorInfo info;

    protected override IEnumerator Animate()
    {
        float sec = this.sec;
        WaitAnimatorInfo info = this.info;
        yield return new WaitForSeconds(sec);
        info.completed = true;
    }
}
