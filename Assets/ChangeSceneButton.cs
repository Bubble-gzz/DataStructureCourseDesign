using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : ImageButton
{
    // Start is called before the first frame update
    [SerializeField]
    string sceneName;
    protected override void OnMouseClicked()
    {
        base.OnMouseClicked();
        SceneManager.LoadScene(sceneName);
    }
}
