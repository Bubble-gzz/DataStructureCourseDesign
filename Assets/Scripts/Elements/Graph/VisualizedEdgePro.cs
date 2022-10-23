using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualizedEdgePro : MonoBehaviour
{
    // Start is called before the first frame update
    LineRenderer dashedLine;
    public SpriteRenderer normalLine;
    List<Vector2> ends;
    Camera mainCam;
    [SerializeField]
    public List<Color> colors = new List<Color>();
    bool playingDrawAnimation;
    enum State{
        Hover,
        Drawn
    }
    State state;
    public List<GameObject> nodes;
    AnimationBuffer animationBuffer;
    SpriteRenderer sprite;
    TMP_Text text;
    Canvas canvas;
    GameObject textObject;
    void Awake()
    {
        dashedLine = transform.Find("DashedLine").GetComponent<LineRenderer>();
        dashedLine.positionCount = 2;
        dashedLine.enabled = true;
        normalLine = transform.Find("NormalLine").GetComponent<SpriteRenderer>();

        state = State.Hover;
        ends = new List<Vector2>();
        ends.Add(new Vector2(0, 0));
        ends.Add(new Vector2(0, 0));
        nodes = new List<GameObject>();
        nodes.Add(null); nodes.Add(null);
        playingDrawAnimation = false;
        

        sprite = normalLine.GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        textObject = transform.Find("TextObject").gameObject;
        canvas = transform.Find("TextObject/Canvas").GetComponent<Canvas>();
        canvas.enabled = false;
        text = GetComponentInChildren<TMP_Text>();


        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<ChangeColorAnimator>();
        gameObject.AddComponent<ChangeTextAnimator>();
        gameObject.AddComponent<SelfDestroyAnimator>();

        PopAnimator linePopAnimator = normalLine.gameObject.AddComponent<PopAnimator>();
        linePopAnimator.widthOnly = true;

        textObject.AddComponent<PopAnimator>();
        textObject.AddComponent<SelfDestroyAnimator>();
    }
    void Start()
    {
        mainCam = Global.mainCamera;
    }

    void Update()
    {
        RefreshEnds();
        RefreshPos();
        RefreshState();
    }
    void RefreshEnds()
    {
        for (int i = 0; i < 2; i++)
        {
            if (nodes[i] != null)
                ends[i] = nodes[i].transform.position;
        }
    }
    void RefreshPos()
    {
        if (state == State.Drawn) {
        }
        if (state == State.Hover)
        {
            ends[1] = (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition);
        }
        for (int i = 0; i < 2; i++)
        {
            dashedLine.SetPosition(i, ends[i]);
        }
        RefreshNormalLine(ends[0], ends[1]);
    }
    void RefreshNormalLine(Vector2 start, Vector2 end)
    {
        transform.position = (start + end) / 2;
        normalLine.transform.right = (end - start).normalized;
        float scaleY = normalLine.transform.lossyScale.y;
        SetGlobalScale(normalLine.transform, new Vector2((end - start).magnitude, scaleY));
    }
    void SetGlobalScale(Transform transform, Vector2 globalScale)
    {
        transform.localScale = Vector2.one;
        transform.localScale = new Vector2(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y);
    }
    void RefreshState()
    {
        if (state == State.Hover)
        {
            bool flag = true;
            if (Input.GetMouseButtonUp(0))
            {
                flag = Draw();
            } 
            if (Input.GetMouseButton(1) || !flag) Destroy(gameObject);
        }
    }
    bool Draw()
    {
        if (Global.selectedNode == null) return false;
        if (Global.selectedNode == nodes[0]) return false; // no self-loop
        nodes[1] = Global.selectedNode;
        VisualizedNode U = nodes[0].GetComponent<VisualizedNode>();
        VisualizedNode V = nodes[1].GetComponent<VisualizedNode>();
        //Debug.Log("U:" + U + "V:" + V);
        if (!U.graph.AddEdge(U, V, gameObject)) return false;
        
        state = State.Drawn;
        RefreshEnds();
        dashedLine.enabled = false;
        animationBuffer.Add(new PopAnimatorInfo(normalLine.gameObject, PopAnimator.Type.Appear));
        animationBuffer.Add(new PopAnimatorInfo(textObject, PopAnimator.Type.Appear));
        //StartCoroutine(DrawAnimation());
        return true;
    }
    IEnumerator DrawAnimation()
    {
        float progress = 0, speed = 7;
        playingDrawAnimation = true;
        normalLine.enabled = true;
        Vector2 start = ends[0], end = ends[1];
        while (1 - progress > Time.deltaTime * speed)
        {
            progress += Time.deltaTime * speed;
            RefreshNormalLine(start, Vector2.Lerp(start, end, progress));
            yield return null;
        }
        dashedLine.enabled = false;
        playingDrawAnimation = false;
    }
}
