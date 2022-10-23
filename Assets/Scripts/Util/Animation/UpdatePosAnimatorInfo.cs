using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosAnimatorInfo : AnimationInfo
{
    Vector2 target;
    bool animated;
    public UpdatePosAnimatorInfo(GameObject _gameObject, Vector2 _target, bool _animated = true)
    {
        this.gameObject = _gameObject;
        this.target = _target;
        this.animated = _animated;
    }
    public override void Invoke()
    {
        UpdatePosAnimator animator = gameObject.GetComponent<UpdatePosAnimator>();
        animator.info = this;
        if (animator == null) {
            Debug.Log("Animator does not exist!\n");
            return;
        }
        animator.target = target;
        animator.animated = animated;
        animator.block = block;
        animator.Invoke();
    }
}
