using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class SelfDestroyAnimator : Animation
{
    public bool animated;
    public bool widthOnly;
    public SelfDestroyAnimatorInfo info;

    protected override IEnumerator Animate()
    {
        bool animated = this.animated;
        bool widthOnly = this.widthOnly;
        SelfDestroyAnimatorInfo info = this.info;
        info.completed = true;
        PopAnimator[] popAnimators;
        if (gameObject.GetComponent<PopAnimator>())
        {
            popAnimators = new PopAnimator[1]{gameObject.GetComponent<PopAnimator>()};
        }
        else popAnimators = gameObject.GetComponentsInChildren<PopAnimator>();
        foreach(PopAnimator popAnimator in popAnimators)
        {
            PopAnimatorInfo popAnimatorInfo = new PopAnimatorInfo(popAnimator.gameObject, PopAnimator.Type.Disappear);
            objectAnimationBuffer.Add(popAnimatorInfo);
        }
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        yield break;
    }
}
