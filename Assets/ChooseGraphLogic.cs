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
    public void CreateNewGraph()
    {
        sceneSwitcher.FadeOut();
        GameObject newWizard = Instantiate(newGraphWizardPrefab);
        newWizard.GetComponentInChildren<UIPanel>().FadeIn();
        newWizard.GetComponentInChildren<UIPanel>().panelClosed.AddListener(sceneSwitcher.FadeIn);
        newWizard.GetComponentInChildren<MyInputField>().inputFieldOK.AddListener(CreateNewGraphWithName);
    }
    public void CreateNewGraphWithName(string graphName)
    {
        Global.fileName = graphName;
        sceneSwitcher.OnLeaveScene("Graph");
    }
}
