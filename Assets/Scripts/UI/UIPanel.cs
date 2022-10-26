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
    [SerializeField]

    bool brandNew = true;
    [SerializeField]
    protected bool blockMouse = true;
    [SerializeField]
    protected bool hovering = true;
    protected bool fadingOut = false;
    public UnityEvent panelClosed = new UnityEvent();
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        //transform.parent.GetComponent<Canvas>().worldCamera = Global.mainCamera.GetComponent<Camera>();
        canvasGroup.alpha = 0;
        mouseEntered = false;
        StartCoroutine(BrandNewCountDown());
        FadeIn();
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
        StartCoroutine(_FadeIn());
    }
    IEnumerator _FadeIn()
    {
        float progress = 0, speed = 5f;
        canvasGroup.alpha = progress;
        while (progress + speed * Time.deltaTime < 1)
        {
            progress += speed * Time.deltaTime;
            canvasGroup.alpha = progress;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }
    public void FadeOut()
    {
        StartCoroutine(_FadeOut());
    }
    IEnumerator _FadeOut()
    {
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
        Destroy(rootObject);
        if (mouseEntered && blockMouse) Global.mouseOverUI = false;
    }
}
