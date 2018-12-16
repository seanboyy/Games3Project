using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2DContextMenu : ContextMenu {
    public override void HandleHorizontalMovement(float horizontal)
    {
        if (!activeUIMenu) return;
        if (horizontal > 0)
            SelectElement(selectedGO.GetComponent<_2DContextButton>().westNeighbor);
        else
            SelectElement(selectedGO.GetComponent<_2DContextButton>().eastNeighbor);
    }
}
