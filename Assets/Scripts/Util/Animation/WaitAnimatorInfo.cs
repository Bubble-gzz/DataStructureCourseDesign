using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnimatorInfo : AnimationInfo
{
    float sec;
    public WaitAnimatorInfo(GameObject _gameObject, float _sec)
    {
        this.gameObject = _gameObject;
        this.sec = _sec;
    }
    public override void Invoke()
    {
        WaitAnimator animator = gameObject.GetComponent<WaitAnimator>();
        animator.info = this;
        if (animator == null) {
            Debug.Log("Animator does not exist!\n");
            return;
        }
        animator.sec = sec;
        animator.Invoke();
    }
}