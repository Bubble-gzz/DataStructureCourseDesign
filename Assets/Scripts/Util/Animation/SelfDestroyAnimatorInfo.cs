using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyAnimatorInfo : AnimationInfo
{
    bool animated;
    bool widthOnly;
    public SelfDestroyAnimatorInfo(GameObject _gameObject, bool _animated = true, bool widthOnly = false)
    {
        this.gameObject = _gameObject;
        this.animated = _animated;
        this.widthOnly = widthOnly;
    }
    public override void Invoke()
    {
        SelfDestroyAnimator animator = gameObject.GetComponent<SelfDestroyAnimator>();
        animator.info = this;
        if (animator == null) {
            Debug.Log("Animator does not exist!\n");
            return;
        }
        animator.animated = animated;
        animator.widthOnly = widthOnly;
        animator.Invoke();
    }
}
