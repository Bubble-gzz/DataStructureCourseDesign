using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePosAnimator : Animation
{
    public Vector2 target;
    public bool animated;
    public UpdatePosAnimatorInfo info;
    protected override IEnumerator Animate()
    {
        Vector2 target = this.target;
        bool animated = this.animated;
        UpdatePosAnimatorInfo info = this.info;
        int myOrder = animationOrder.NewOrder();
        if (!block) info.completed = true;
        if (animated)
        {
            while (true)
            {
                Vector2 delta = target - (Vector2)transform.position;
                float dist = delta.magnitude;
                if (dist < 0.01f) break;
                float speed = Mathf.Max(0.5f, dist * 10);
                transform.position += (Vector3)delta.normalized * speed * Time.deltaTime;
                yield return null;
                if (!animationOrder.isLatest(myOrder)) {
                    yield break;
                }
            }
        }
        transform.position = target;
        if (block) info.completed = true;
    }
}
