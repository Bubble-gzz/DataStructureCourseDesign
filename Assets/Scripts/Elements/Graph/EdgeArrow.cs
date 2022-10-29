using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeArrow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float arrowOffset;
    public VisualizedEdgePro root;
    void Start()
    {
        if (!Global.curGraph.graph.directed) GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RefreshPos(Vector2 start, Vector2 end)
    {
        float offset = this.arrowOffset;
        Vector2 dir = (end - start).normalized;

        if (root.state != VisualizedEdgePro.State.Drawn) offset = 0;
        transform.position = end - dir * offset;
        transform.right = dir;
    }
}
