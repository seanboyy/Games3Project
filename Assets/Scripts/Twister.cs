using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twister : Unit
{
    public int rotationAmount = 1;

    public void ActivateTwistButton()
    {
        contextMenu.HideContextMenu();
        TwistBoard(gridElement.gameObject);
    }

    private void TwistBoard(GameObject twistLoc)
    {
        if (gridElement.northNeighbor && gridElement.eastNeighbor && gridElement.southNeighbor && gridElement.westNeighbor)
            for (int i = 0; i < rotationAmount; ++i)
            {
                #region DisconnectWalls
                if (gridElement.northNeighbor.GetComponent<GridElement>().northNeighbor)
                {
                    gridElement.northNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().southWall = false;
                    gridElement.northNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().southWall = false;
                    gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().southWall = false;
                }
                if (gridElement.eastNeighbor.GetComponent<GridElement>().eastNeighbor)
                {
                    gridElement.eastNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().westWall = false;
                    gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().westWall = false;
                    gridElement.eastNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().westWall = false;
                }
                if (gridElement.southNeighbor.GetComponent<GridElement>().southNeighbor)
                {
                    gridElement.southNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().northWall = false;
                    gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().northWall = false;
                    gridElement.southNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().northWall = false;
                }
                if (gridElement.westNeighbor.GetComponent<GridElement>().westNeighbor)
                {
                    gridElement.westNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().eastWall = false;
                    gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().eastWall = false;
                    gridElement.westNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().eastWall = false;
                }
                #endregion
                #region Rotate Tiles
                gridElement.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                gridElement.northNeighbor.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                gridElement.eastNeighbor.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                gridElement.southNeighbor.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                gridElement.westNeighbor.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                #endregion
                #region Rotate Pieces
                if (gridElement.piece)
                    gridElement.piece.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                if (gridElement.northNeighbor.GetComponent<GridElement>().piece)
                    gridElement.northNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                if (gridElement.eastNeighbor.GetComponent<GridElement>().piece)
                    gridElement.eastNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                if (gridElement.southNeighbor.GetComponent<GridElement>().piece)
                    gridElement.southNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                if (gridElement.westNeighbor.GetComponent<GridElement>().piece)
                    gridElement.westNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                if (gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().piece)
                    gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                if (gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().piece)
                    gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                if (gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().piece)
                    gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                if (gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().piece)
                    gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, -Vector3.forward, 90);
                #endregion
                #region Fix Neighbors
                gridElement.FindNeighbors();
                gridElement.northNeighbor.GetComponent<GridElement>().FindNeighbors();
                gridElement.eastNeighbor.GetComponent<GridElement>().FindNeighbors();
                gridElement.southNeighbor.GetComponent<GridElement>().FindNeighbors();
                gridElement.westNeighbor.GetComponent<GridElement>().FindNeighbors();
                gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().FindNeighbors();
                gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().FindNeighbors();
                gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().FindNeighbors();
                gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().FindNeighbors();
                if (gridElement.northNeighbor.GetComponent<GridElement>().northNeighbor)
                {
                    gridElement.northNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().FindNeighbors();
                    gridElement.northNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().FindNeighbors();
                    gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().FindNeighbors();
                }
                if (gridElement.eastNeighbor.GetComponent<GridElement>().eastNeighbor)
                {
                    gridElement.eastNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().FindNeighbors();
                    gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().FindNeighbors();
                    gridElement.eastNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().FindNeighbors();
                }
                if (gridElement.southNeighbor.GetComponent<GridElement>().southNeighbor)
                {
                    gridElement.southNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().FindNeighbors();
                    gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().FindNeighbors();
                    gridElement.southNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().FindNeighbors();
                }
                if (gridElement.westNeighbor.GetComponent<GridElement>().westNeighbor)
                {
                    gridElement.westNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().FindNeighbors();
                    gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().FindNeighbors();
                    gridElement.westNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().FindNeighbors();
                }
                #endregion
                #region FixWalls
                //this needs to correctly update neighbors too.
                UpdateWalls(gridElement);
                UpdateWalls(gridElement.northNeighbor.GetComponent<GridElement>());
                UpdateWalls(gridElement.eastNeighbor.GetComponent<GridElement>());
                UpdateWalls(gridElement.southNeighbor.GetComponent<GridElement>());
                UpdateWalls(gridElement.westNeighbor.GetComponent<GridElement>());
                UpdateWalls(gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>());
                UpdateWalls(gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>());
                UpdateWalls(gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>());
                UpdateWalls(gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>());
                #endregion
                #region FixWallSprites
                gridElement.UpdateWalls();
                gridElement.northNeighbor.GetComponent<GridElement>().UpdateWalls();
                gridElement.eastNeighbor.GetComponent<GridElement>().UpdateWalls();
                gridElement.southNeighbor.GetComponent<GridElement>().UpdateWalls();
                gridElement.westNeighbor.GetComponent<GridElement>().UpdateWalls();
                gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().UpdateWalls();
                gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().UpdateWalls();
                gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().UpdateWalls();
                gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().UpdateWalls();
                if (gridElement.northNeighbor.GetComponent<GridElement>().northNeighbor)
                {
                    gridElement.northNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().UpdateWalls();
                    gridElement.northNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().UpdateWalls();
                    gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().UpdateWalls();
                }
                if (gridElement.eastNeighbor.GetComponent<GridElement>().eastNeighbor)
                {
                    gridElement.eastNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().UpdateWalls();
                    gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().UpdateWalls();
                    gridElement.eastNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().UpdateWalls();
                }
                if (gridElement.southNeighbor.GetComponent<GridElement>().southNeighbor)
                {
                    gridElement.southNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().UpdateWalls();
                    gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().UpdateWalls();
                    gridElement.southNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().UpdateWalls();
                }
                if (gridElement.westNeighbor.GetComponent<GridElement>().westNeighbor)
                {
                    gridElement.westNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().UpdateWalls();
                    gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().UpdateWalls();
                    gridElement.westNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().UpdateWalls();
                }

                #endregion
                #region Fix Piece Rotation and Position
                if (gridElement.piece)
                    UpdateRotation(gridElement.piece.GetComponent<GamePiece>());
                if (gridElement.northNeighbor.GetComponent<GridElement>().piece)
                    UpdateRotation(gridElement.northNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.eastNeighbor.GetComponent<GridElement>().piece)
                    UpdateRotation(gridElement.eastNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.southNeighbor.GetComponent<GridElement>().piece)
                    UpdateRotation(gridElement.southNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.westNeighbor.GetComponent<GridElement>().piece)
                    UpdateRotation(gridElement.westNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().piece)
                    UpdateRotation(gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().piece)
                    UpdateRotation(gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().piece)
                    UpdateRotation(gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().piece)
                    UpdateRotation(gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().piece.GetComponent<GamePiece>());
                #endregion
            }
    }

    private void UpdateWalls(GridElement element)
    {
        bool temp = element.eastWall;
        bool temp2 = element.northWall;
        if (element.northWall)
        {
            element.eastWall = true;
            element.northWall = false;
        }
        if (element.westWall)
        {
            element.northWall = true;
            element.westWall = false;
        }
        if (element.southWall)
        {
            element.westWall = true;
            element.southWall = false;
        }
        if (temp)
        {
            element.southWall = true;
            if (!temp2) element.eastWall = false;
        }
    }

    private void UpdateRotation(GamePiece piece)
    {
        piece.gameObject.transform.rotation = Quaternion.identity;
        piece.rotated++;
        piece.UpdateVisual();
    }
}
