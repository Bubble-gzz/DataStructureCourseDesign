using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSaveButton : SaveButton
{
    // Start is called before the first frame update
    override public void OnClicked()
    {
        base.OnClicked();
        Global.curGraph.SaveData();
    }
}
