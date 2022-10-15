using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopAnimatorInfo : AnimationInfo
{
    PopAnimator.Type type;
    public PopAnimatorInfo(GameObject _gameObject, PopAnimator.Type _type = PopAnimator.Type.Appear)
    {
        this.gameObject = _gameObject;
        this.type = _type;
    }
    public override void Invoke()
    {
        PopAnimator animator = gameObject.GetComponent<PopAnimator>();
        animator.info = this;
        if (animator == null) {
            Debug.Log("Animator does not exist!\n");
            return;
        }
        animator.type = type;
        animator.block = this.block;
        animator.Invoke();
    }
}
