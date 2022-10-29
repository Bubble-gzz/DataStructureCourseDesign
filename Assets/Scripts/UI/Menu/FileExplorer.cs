using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
public class FileExplorer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform contentRoot;
    [SerializeField]
    GameObject itemPrefab;
    string curPath;
    [SerializeField]
    public string initialPath = "";
    List<string> targetExtensions = new List<string>();
    string rootPath;
    public UnityEvent<string> openFile = new UnityEvent<string>();
    void Awake()
    {
        rootPath = Application.dataPath;

        //Debug.Log(curPath);
        targetExtensions.Add(".data");
    }
    void Start()
    {
        curPath = Application.dataPath + "/" + initialPath;
        ReloadItems(curPath);
    }
    void ResetPath(string newPath, bool reload = true)
    {
        curPath = newPath;
        Debug.Log(curPath);
        if (reload) ReloadItems(curPath);
    }
    bool FilterCheck(FileInfo file)
    {
        foreach (var ext in targetExtensions)
            if (file.Extension == ext) return true;
        return false;
    }
    void ClearItems()
    {
        foreach (Transform item in contentRoot)
            Destroy(item.gameObject);
    }
    void ReloadItems(string curPath)
    {
        DirectoryInfo curDir = new DirectoryInfo(curPath);
        ClearItems();
        DirectoryInfo[] directories = curDir.GetDirectories();
        foreach (var directory in directories)
        {
            FileEntry newFileEntry = Instantiate(itemPrefab, contentRoot).GetComponent<FileEntry>();
            newFileEntry.isFile = false;
            newFileEntry.fileExplorer = this;
            newFileEntry.SetName(directory.Name, directory.FullName);
        }
        FileInfo[] files = curDir.GetFiles();
        foreach (var file in files)
        {
            if (!FilterCheck(file)) continue;
            FileEntry newFileEntry = Instantiate(itemPrefab, contentRoot).GetComponent<FileEntry>();
            newFileEntry.fileExplorer = this;
            newFileEntry.SetName(file.Name, file.FullName);
        }
    }
    public void EnterDirectory(string dirName)
    {
        ResetPath(curPath + "/" + dirName);
    }
    public void ReturnToLastLevelDirectory()
    {
        if (curPath == rootPath) return;
        int i;
        for (i = curPath.Length - 1; i > 0; i--)
            if (curPath[i] == '/') break;
        string lastLevelPath = curPath.Substring(0, i);
        ResetPath(lastLevelPath);
    }
    public void SelectFile(string filePath)
    {
        openFile.Invoke(filePath);
    }
}
