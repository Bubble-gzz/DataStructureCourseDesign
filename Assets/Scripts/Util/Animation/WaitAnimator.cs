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
    bool UserAction(int debugCount = 0)
    {
        if (Global.pressEventConsumed) return false;
        return Input.anyKeyDown;
    }
    protected override IEnumerator Animate()
    {
        WaitAnimatorInfo info = this.info;
        bool useSetting = this.useSetting;
        float sec = this.sec;
        int debugCount = Global.debugCount++;
        //Debug.Log("useSetting:" + useSetting);

        if (!useSetting) yield return new WaitForSeconds(sec);
        else {
            if (Settings.animationTimeScale < 0)
            {
                Message message = Instantiate(messagePrefab).GetComponent<Message>();
                while (true)
                {
                    if (UserAction(debugCount)) {
                        Global.pressEventConsumed = true;
                        /*Debug.Log("frame[" + Time.frameCount + "] thread: " +  debugCount 
                        + "  Global.pressConsumed:" + Global.pressConsumed);*/
                        break;
                    }
                    yield return null;
                }
                //while (!UserAction()) yield return null;
                Global.pressEventConsumed = true;
                message.FadeOut();
                //Debug.Log("frameCount = " + Time.frameCount);
            }
            else {
                sec *= Settings.animationTimeScale;
                yield return new WaitForSeconds(sec);
            }
        }
        info.completed = true;
        yield return null;
    }
}
