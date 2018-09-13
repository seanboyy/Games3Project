using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour {
    public GameObject ghostPrefab;

    private GameObject activeGhost;

    private void Update()
    {
        if(activeGhost != null)
        {
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                activeGhost.transform.position += new Vector3(0, 0.5F);
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                activeGhost.transform.position += new Vector3(0.5F, 0);
            }
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                activeGhost.transform.position += new Vector3(0, -0.5F);
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                activeGhost.transform.position += new Vector3(-0.5F, 0);
            }
            if (Input.GetKeyUp(KeyCode.Return))
            { 
                Destroy(activeGhost.gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        activeGhost = Instantiate(ghostPrefab);
    }
}
