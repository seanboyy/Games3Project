using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    public GameObject ghostPrefab;

    public int moveAmount = 2;

    private int movesTaken = 0;

    private GameObject activeGhost;

    private void Update()
    {

        if (activeGhost != null && movesTaken < 2)
        {
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            {
                activeGhost.transform.position += new Vector3(0, 0.5F);
                movesTaken++;
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                activeGhost.transform.position += new Vector3(-0.5F, 0);
                movesTaken++;
            }
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                activeGhost.transform.position += new Vector3(0, -0.5F);
                movesTaken++;
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                activeGhost.transform.position += new Vector3(0.5F, 0);
                movesTaken++;
            }
        }
        if (activeGhost != null && Input.GetKeyUp(KeyCode.Return))
        {
            transform.root.position = activeGhost.transform.position;
            Destroy(activeGhost.gameObject);
            transform.parent.position += new Vector3(0, 0, 100);
            transform.parent.gameObject.SetActive(false);
            movesTaken = 0;
        }
    }

    private void OnMouseDown()
    {
        activeGhost = Instantiate(ghostPrefab, transform.root.position, Quaternion.identity);
        transform.parent.position += new Vector3(0, 0, -100);
    }
}
