using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class UIPanel: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    //bool show;
    protected CanvasGroup canvasGroup;
    [SerializeField]
    GameObject rootObject;
    bool mouseEntered;

    bool brandNew = true;
    [SerializeField]
    protected bool blockMouse = true;
    [SerializeField]
    protected bool hovering = true;
    protected bool fadingOut = false;
    public bool appearOnCreate = true, destroyOnFadeOut = true;
    public UnityEvent panelClosed = new UnityEvent();
    [SerializeField]
    bool visibleState;
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        mouseEntered = false;
        visibleState = false;
    }
    protected virtual void Start()
    {
        canvasGroup.blocksRaycasts = false;
        if (appearOnCreate) FadeIn();
    }

    // Update is called once per frame
    IEnumerator BrandNewCountDown()
    {
        //Debug.Log("[frame:"+ Time.frameCount +"] enterBrandNewCountDown");
        yield return null;
        //Debug.Log("[frame:"+ Time.frameCount +"] enterBrandNewCountDownAgain");
        brandNew = false;
    }
    protected virtual void Update()
    {
        //Debug.Log("rootObject " + rootObject);
        if (hovering)
        {
            if (Input.GetMouseButtonDown(0) && !mouseEntered && !brandNew)
                FadeOut();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (blockMouse) Global.mouseOverUI = true;
        mouseEntered = true;
        //Debug.Log("eventData.position" + eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (blockMouse) Global.mouseOverUI = false;
        mouseEntered = false;
    }
    public void FadeIn()
    {
        if (visibleState) return;
        StartCoroutine(BrandNewCountDown());
        StartCoroutine(_FadeIn());
    }
    IEnumerator _FadeIn()
    {
        canvasGroup.blocksRaycasts = true;
        Canvas canvas = rootObject.GetComponentInChildren<Canvas>();
        if (canvas != null) canvas.enabled = true;
        float progress = 0, speed = 5f;
        canvasGroup.alpha = progress;
        while (progress + speed * Time.deltaTime < 1)
        {
            progress += speed * Time.deltaTime;
            canvasGroup.alpha = progress;
            yield return null;
        }
        canvasGroup.alpha = 1;
        visibleState = true;
    }
    public void FadeOut()
    {
        if (!visibleState && !destroyOnFadeOut) return;
        StartCoroutine(_FadeOut());
    }
    IEnumerator _FadeOut()
    {
        //Debug.Log("Message" + Global.debugCount + " FadeOut");
        canvasGroup.blocksRaycasts = false;
        float progress = 1, speed = 5f;
        fadingOut = true;
        canvasGroup.alpha = progress;
        while (progress + speed * Time.deltaTime > 0)
        {
            progress -= speed * Time.deltaTime;
            canvasGroup.alpha = progress;
            yield return null;
        }
        canvasGroup.alpha = 0;
        panelClosed.Invoke();
        
        Canvas canvas = rootObject.GetComponentInChildren<Canvas>();
        if (canvas != null) canvas.enabled = false;

        if (destroyOnFadeOut) Destroy(rootObject);
        if (mouseEntered && blockMouse) Global.mouseOverUI = false;
        brandNew = true;
        visibleState = false;
    }
}
