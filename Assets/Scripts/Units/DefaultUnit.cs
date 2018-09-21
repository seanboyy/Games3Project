using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultUnit : MonoBehaviour
{
    public GameObject unitMenu;

    public void Awake()
    {
        Messenger<RaycastHit>.AddListener(Messages.CURSORCLICK, CheckRaycast);
    }

    void CheckRaycast(RaycastHit info)
    {
        if(info.collider.gameObject == gameObject)
        {
            OnCursorClick();
        }
    }

    private void OnMouseDown()
    {
        OnCursorClick();
    }

    private void OnCursorClick()
    {
        unitMenu.SetActive(true);
    }

    private void OnDestroy()
    {
        Messenger<RaycastHit>.RemoveListener(Messages.CURSORCLICK, CheckRaycast);
    }
}
