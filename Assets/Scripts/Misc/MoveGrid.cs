using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrid : MonoBehaviour {
    public bool isClicked = false;

    private void OnMouseDown()
    {
        isClicked = true;
    }
}
