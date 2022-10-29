using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject messagePrefab;
    virtual public void OnClicked()
    {
        Message newMessage = Instantiate(messagePrefab).GetComponent<Message>();
        newMessage.SetText("Data has been saved successfully!");
        newMessage.Blink();
    }
}
