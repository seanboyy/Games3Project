using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrid : MonoBehaviour
{

    public void Awake()
    {
        Messenger<RaycastHit>.AddListener(Messages.CURSORCLICK, CheckRaycast);
    }

    void CheckRaycast(RaycastHit info)
    {
        if (info.collider.gameObject == gameObject)
        {
            OnCursorClick();
        }
    }

    private void OnCursorClick()
    {
        Messenger<Vector3>.Broadcast(Messages.MOVETILECLICKED, transform.position);
    }

    private void OnMouseDown()
    {
        OnCursorClick();
    }

    private void OnDestroy()
    {
        Messenger<RaycastHit>.RemoveListener(Messages.CURSORCLICK, CheckRaycast);
    }
}
