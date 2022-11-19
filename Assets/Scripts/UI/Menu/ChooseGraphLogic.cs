using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseGraphLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject fileExplorerPrefab;
    [SerializeField]
    GameObject newGraphWizardPrefab;
    SceneSwitcher sceneSwitcher;
    void Awake()
    {
        sceneSwitcher = FindObjectOfType<SceneSwitcher>().GetComponent<SceneSwitcher>();
    }
    void Start()
    {
        Global.loadGraphFromFiles = false;
    }
    public void LoadGraphFromFiles()
    {
        sceneSwitcher.FadeOut();
        FileExplorer fileExplorer = Instantiate(fileExplorerPrefab).GetComponent<FileExplorer>();
        fileExplorer.initialPath = "GraphData";
        fileExplorer.gameObject.GetComponentInChildren<UIPanel>().panelClosed.AddListener(sceneSwitcher.FadeIn);
        fileExplorer.openFile.AddListener(LoadGraphFromFile);
    }
    void LoadGraphFromFile(string filePath)
    {
        Global.loadGraphFromFiles = true;
        Global.filePath = filePath;
        sceneSwitcher.OnLeaveScene("Graph");
    }
    public void GoToNameGraph()
    {
        sceneSwitcher.OnLeaveScene("NameGraph");
    }
    public void CreateNewGraph(bool isDirected)
    {
        Global.newGraphDirected = isDirected;
        sceneSwitcher.OnLeaveScene("Graph");
    }
    public void ReturnToMenu()
    {
        sceneSwitcher.OnLeaveScene("Entrance");
    }
    public void ReturnToChooseGraph()
    {
        sceneSwitcher.OnLeaveScene("ChooseGraph");
    }
}
