using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnimator : Animation
{
    public float sec;
    public WaitAnimatorInfo info;

    bool UserAction()
    {
        return Input.anyKey;
    }
    protected override IEnumerator Animate()
    {
        WaitAnimatorInfo info = this.info;
        if (Settings.animationTimeScale == -1)
        {
            while (!UserAction()) yield return null;
        }
        else {
            float sec = this.sec * Settings.animationTimeScale;
            yield return new WaitForSeconds(sec);
        }
        info.completed = true;
    }
}
