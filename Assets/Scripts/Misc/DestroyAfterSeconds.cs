using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{

    public float waitTime;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DestroyAfter(waitTime));
    }

    IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Destroy(gameObject);
    }
}
