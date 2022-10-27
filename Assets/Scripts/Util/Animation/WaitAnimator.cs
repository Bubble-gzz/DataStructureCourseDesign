using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAnimator : Animation
{
    public float sec;
    public bool useSetting;
    public WaitAnimatorInfo info;
    GameObject messagePrefab;
    override protected void Awake()
    {
        messagePrefab = Resources.Load<GameObject>("Prefabs/UI/Message");
    }
    bool UserAction()
    {
        if (Global.pressEventConsumed) return false;
        return Input.anyKeyDown;
    }
    protected override IEnumerator Animate()
    {
        WaitAnimatorInfo info = this.info;
        bool useSetting = this.useSetting;
        float sec = this.sec;
        if (!useSetting && sec > 0) yield return new WaitForSeconds(sec);
        else {
            if (Settings.animationTimeScale < 0 || sec < 0)
            {
                Message message = Instantiate(messagePrefab).GetComponent<Message>();
                message.StartBreathing();
                while (true)
                {
                    if (UserAction()) {
                        Global.pressEventConsumed = true;
                        break;
                    }
                    yield return null;
                }
                Global.pressEventConsumed = true;
                message.FadeOut();
            }
            else {
                sec *= Settings.animationTimeScale;
                yield return new WaitForSeconds(sec);
            }
        }
        info.completed = true;
        Global.waitingEventCount--;
        yield return null;
    }
}
