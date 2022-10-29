using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosAnimator : Animation
{
    public Vector2 target;
    public bool animated;
    public bool local;
    public UpdatePosAnimatorInfo info;
    protected override IEnumerator Animate()
    {
        Vector2 target = this.target;
        bool animated = this.animated;
        bool block = this.block;
        bool local = this.local;
        UpdatePosAnimatorInfo info = this.info;
        int myOrder = animationOrder.NewOrder();
        //Debug.Log("updatePos.block: " + block);
        if (!block) info.completed = true;
        if (animated)
        {
            while (true)
            {
                Vector2 delta;
                if (!local) delta = target - (Vector2)transform.position;
                else delta = target - (Vector2)transform.localPosition;
                float dist = delta.magnitude;
                if (dist < 0.01f) break;
                float speed = Mathf.Max(0.5f, dist * 10);
                if (!local) transform.position += (Vector3)delta.normalized * speed * Time.deltaTime;
                else transform.localPosition += (Vector3)delta.normalized * speed * Time.deltaTime;
                yield return null;
                if (!animationOrder.isLatest(myOrder)) {
                    yield break;
                }
            }
        }
        if (!local) transform.position = target;
        else transform.localPosition = target;
        if (block) info.completed = true;
    }
}
