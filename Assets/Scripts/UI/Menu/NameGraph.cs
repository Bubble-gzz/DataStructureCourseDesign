using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameGraph : MonoBehaviour
{
    // Start is called before the first frame update
    SceneSwitcher sceneSwitcher;
    void Awake()
    {
        sceneSwitcher = GameObject.Find("SceneSwitcher").GetComponentInChildren<SceneSwitcher>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateNewGraphWithName(string graphName)
    {
        Global.fileName = graphName;
        sceneSwitcher.OnLeaveScene("ChooseGraphDirection");
    }
}
