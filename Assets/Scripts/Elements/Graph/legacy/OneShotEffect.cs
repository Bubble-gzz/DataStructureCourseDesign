using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float lifetime = 1;
    void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    // Update is called once per frame
    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
