using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{
    Image image;
    void Awake()
    {
        image = gameObject.GetComponent<Image>();
    }
    void Start()
    {
        FadeIn();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnLeaveScene(string newSceneName)
    {
        StartCoroutine(_OnLeaveScene(newSceneName));
    }
    public void FadeOut()
    {
        StartCoroutine(_FadeOut());
    }
    IEnumerator _OnLeaveScene(string newSceneName)
    {
        yield return _FadeOut(); 
        SceneManager.LoadScene(newSceneName);
    }
    IEnumerator _FadeOut()
    {
        float progress = 0, speed = 4f;
        while (progress < 1)
        {
            progress += speed * Time.deltaTime;
            Color newColor = image.color;
            newColor.a = progress;
            image.color = newColor;
            yield return null;
        }
    }
    public void FadeIn()
    {
        StartCoroutine(_FadeIn());
    }
    IEnumerator _FadeIn()
    {
        float progress = 0, speed = 4f;
        while (progress < 1)
        {
            progress += speed * Time.deltaTime;
            Color newColor = image.color;
            newColor.a = 1 - progress;
            image.color = newColor;
            yield return null;
        }

    }
}
