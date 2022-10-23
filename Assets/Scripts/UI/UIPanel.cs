using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPanel: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    bool show;
    CanvasGroup canvasGroup;
    [SerializeField]
    GameObject rootObject;
    bool mouseEntered;
    bool safeZone = true;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Start()
    {
        transform.parent.GetComponent<Canvas>().worldCamera = Global.mainCamera.GetComponent<Camera>();
        show = false;
        canvasGroup.alpha = 0;
        mouseEntered = false;
        StartCoroutine(SafeCountDown());
        FadeIn();
    }

    // Update is called once per frame
    IEnumerator SafeCountDown()
    {
        yield return null;
        safeZone = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !mouseEntered && !safeZone)
            FadeOut();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Global.mouseOverUI = true;
        mouseEntered = true;
        //Debug.Log("eventData.position" + eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseEntered = false;
        Global.mouseOverUI = false;
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
        canvasGroup.alpha = progress;
        while (progress + speed * Time.deltaTime > 0)
        {
            progress -= speed * Time.deltaTime;
            canvasGroup.alpha = progress;
            yield return null;
        }
        canvasGroup.alpha = 0;
        Destroy(rootObject);
    }
}
