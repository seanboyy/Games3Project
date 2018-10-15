using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twister : Unit
{
    public float rotationAmount = 90F;

    public void ActivateTwistButton()
    {
        action = "twist";
        contextMenu.HideContextMenu();
        TwistBoard(gridElement.gameObject);
    }

    public override void PerformAction(GameObject actionLocGO)
    {
        if (action == "move") base.PerformAction(actionLocGO);
        //if (action == "twist") TwistBoard(actionLocGO);
    }

    private void TwistBoard(GameObject twistLoc)
    {
        action = "";
        Debug.Log("Doing Twist");
        gridElement.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        gridElement.northNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        gridElement.eastNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        gridElement.southNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        gridElement.westNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        if (gridElement.piece) gridElement.piece.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        if (gridElement.northNeighbor.GetComponent<GridElement>().piece) gridElement.northNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        if (gridElement.eastNeighbor.GetComponent<GridElement>().piece) gridElement.eastNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        if (gridElement.southNeighbor.GetComponent<GridElement>().piece) gridElement.southNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        if (gridElement.westNeighbor.GetComponent<GridElement>().piece) gridElement.westNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        if (gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().piece) gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        if (gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().piece) gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        if (gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().piece) gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);
        if (gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().piece) gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.forward, rotationAmount);

    }
}
