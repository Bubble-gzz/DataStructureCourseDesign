using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BracketSequenceValidator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    UnityEvent onInputCheckPass = new UnityEvent();
    [SerializeField]
    UnityEvent onInputCheckFailed = new UnityEvent();
    string s = "";
    public void UpdateContent(string _s)
    {
        this.s = _s;
    }
    public void CheckInput()
    {
        bool isValid = true;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '(' || s[i] == ')') continue;
            if (s[i] == '[' || s[i] == ']') continue;
            if (s[i] == '{' || s[i] == '}') continue;
            isValid = false;
            break;
        }
        Debug.Log("Check result : " + isValid);
        if (isValid) onInputCheckPass.Invoke();
        else onInputCheckFailed.Invoke();
    }
}
