using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnimatorInfo : AnimationInfo
{
    float sec;
    bool useSetting;
    public string messageText = "";
    public WaitAnimatorInfo(GameObject _gameObject, float _sec, bool _useSetting = true)
    {
        this.gameObject = _gameObject;
        this.sec = _sec;
        this.useSetting = _useSetting;
        Global.waitingEventCount++;
    }
    public override void Invoke()
    {
        WaitAnimator animator = gameObject.GetComponent<WaitAnimator>();
        animator.info = this;
        if (animator == null) {
            Debug.Log("Animator does not exist!\n");
            return;
        }
        animator.messageText = messageText;
        animator.sec = sec;
        animator.useSetting = useSetting;
        animator.Invoke();
    }
}
