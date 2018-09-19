using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAfterSeconds : MonoBehaviour
{
    public float duration = 1f;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine("SelfDelete");
    }
	
    IEnumerator SelfDelete()
    {
        if (duration == -1)
            StopCoroutine("SelfDelete");
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
