using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SelfDestroyAnimator : Animation
{
    public bool animated;
    public SelfDestroyAnimatorInfo info;

    protected override IEnumerator Animate()
    {
        bool animated = this.animated;
        SelfDestroyAnimatorInfo info = this.info;
        info.completed = true;
        PopAnimatorInfo popAnimatorInfo = new PopAnimatorInfo(gameObject, PopAnimator.Type.Disappear);
        popAnimatorInfo.block = true;
        objectAnimationBuffer.Add(popAnimatorInfo);
        while (!popAnimatorInfo.completed) {
            Debug.Log("popAnimatorInfo.completed = " + popAnimatorInfo.completed);
            yield return null;
        }
        Destroy(gameObject);
        yield break;
    }
}
