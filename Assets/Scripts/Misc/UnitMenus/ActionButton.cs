using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    private void OnMouseExit()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
