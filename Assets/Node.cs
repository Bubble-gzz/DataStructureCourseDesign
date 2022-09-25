using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 scale0;
    [SerializeField]
    float size = 1.0f;
    bool mouseHovering;
    [SerializeField]
    GameObject ripple;
    [SerializeField]
    float rippleIntervals = 3f;
    bool isRippling;
    Transform root;
    Animator animator;
    void Start()
    {
        scale0 = transform.localScale;
        mouseHovering = false;
        isRippling = false;
        root = transform.parent;
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
        //size = 1.2f;
        mouseHovering = true;
        animator.SetBool("mouseHovering", true);
    }
    public void OnMouseExit()
    {
        //size = 1;
        mouseHovering = false;
        animator.SetBool("mouseHovering", false);
    }
    IEnumerator Rippling()
    {
        isRippling = true;
        GameObject _ripple = Instantiate(ripple, root);
        yield return new WaitForSeconds(rippleIntervals);
        isRippling = false;
    }
}
