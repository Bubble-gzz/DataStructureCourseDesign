using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Global.mainCamera = GetComponent<Camera>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void ExitApplication()
    {
        Application.Quit();
    }
}
