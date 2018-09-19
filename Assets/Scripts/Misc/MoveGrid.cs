using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrid : MonoBehaviour {
    private void OnMouseDown()
    {
        Messenger<Vector3>.Broadcast(Messages.MOVETILECLICKED, transform.position);
    }
}
