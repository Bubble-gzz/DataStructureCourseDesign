using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestCollider : MonoBehaviour
{
    // Start is called before the first frame update
    Color colorA = new Color(1,1,1,1);
    Color colorB = new Color(0.5f,0.5f,0.5f,1);
    MyCollider myCollider;
    void Start()
    {
        myCollider = transform.GetComponentInChildren<MyCollider>();
        myCollider.onMouseEnter.AddListener(MyOnMouseEnter);
        myCollider.onMouseExit.AddListener(MyOnMouseExit);
    }
    public void MyOnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = colorB;
    }

    public void MyOnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = colorA;
    }
}
