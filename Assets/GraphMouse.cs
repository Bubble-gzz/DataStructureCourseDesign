using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphMouse : MouseManager
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject nodePrefab;
    protected override void Start()
    {
        base.Start();
        editMode = EditMode.Create;
    }
    protected override void LeftClick()
    {
        Debug.Log("Create new node.");
        if (editMode == EditMode.Create)
        {
            if (hoverCount == 0)
                Instantiate(nodePrefab, (Vector2)Global.mainCamera.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        }
    }
}
