using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class WaterProblemInputValidator : MonoBehaviour
{
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
        try{
            string[] splitRes = s.Split( new char[]{','} );
            foreach (var s in splitRes)
            {
                int num = int.Parse(s);
                if (num < 0) {
                    isValid = false;
                    break;
                }
                //Debug.Log("parse result : " + num);
            }
        }
        catch(Exception e)
        {
            Debug.Log("Failed to parse the input : " + e);
            isValid = false;
        }
        if (isValid) onInputCheckPass.Invoke();
        else onInputCheckFailed.Invoke();
    }
}
