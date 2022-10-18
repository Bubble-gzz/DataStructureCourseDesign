using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_legacy : MonoBehaviour
{
    // Start is called before the first frame update
    /*
    Vector2 scale0;
    [SerializeField]
    float size = 1.0f;
    bool mouseHovering
    {
        get{
            return parent.mouseHovering;
        }
    }
    [SerializeField]
    GameObject ripple;
    [SerializeField]
    float rippleIntervals = 3f;
    bool isRippling;
    Transform root;
    Animator animator;
    NodeRoot parent;

    void Start()
    {
        scale0 = transform.localScale;
        isRippling = false;
        root = transform.parent;
        parent = root.GetComponent<NodeRoot>();
        animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.SetBool("mouseHovering", false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = scale0 * size;
        if (!isRippling && mouseHovering) StartCoroutine(Rippling());
    }

    public void OnMouseEnter()
    {
        animator.SetBool("mouseHovering", true);
    }
    public void OnMouseExit()
    {
        animator.SetBool("mouseHovering", false);
    }
    IEnumerator Rippling()
    {
        isRippling = true;
        GameObject _ripple = Instantiate(ripple, root);
        yield return new WaitForSeconds(rippleIntervals);
        isRippling = false;
    }
    */
}
