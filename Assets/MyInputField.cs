using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MyInputField : MonoBehaviour
{
    public UnityEvent<string> inputFieldOK = new UnityEvent<string>();
    public void OnInputFieldOK()
    {
        TMP_InputField inputField = GetComponent<TMP_InputField>();
        inputFieldOK.Invoke(inputField.text);
    }
}
