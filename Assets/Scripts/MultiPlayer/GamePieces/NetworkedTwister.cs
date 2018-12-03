using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedTwister : NetworkedUnit
{
    public int rotationAmount = 1;

    protected override void Start()
    {
        base.Start();
        unitType = UnitType.Twister;
    }

    public void ActivateTwistButton()
    {
        if (canAct)
        {
            contextMenu.HideContextMenu();
            TwistBoard(gridElement.gameObject);
            grid.activeGO = null;
            grid.SetElementColor(grid.selectedGO, NetworkedMenu.selectedColor, NetworkedMenu.defaultColor);
            canAct = false;
        }
    }

    private void TwistBoard(GameObject twistLoc)
    {
        if (gridElement.northNeighbor && gridElement.eastNeighbor && gridElement.southNeighbor && gridElement.westNeighbor)
            for (int i = 0; i < rotationAmount; ++i)
            {
                List<GameObject> neighbors = new List<GameObject>();
                #region DisconnectWalls
                if (gridElement.northNeighbor.GetComponent<NetworkedGridElement>().northNeighbor)
                {
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().southWall = false;
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().southWall = false;
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().southWall = false;
                }
                if (gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor)
                {
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().westWall = false;
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().westWall = false;
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().westWall = false;
                }
                if (gridElement.southNeighbor.GetComponent<NetworkedGridElement>().southNeighbor)
                {
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().northWall = false;
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().northWall = false;
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().northWall = false;
                }
                if (gridElement.westNeighbor.GetComponent<NetworkedGridElement>().westNeighbor)
                {
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().eastWall = false;
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().eastWall = false;
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().eastWall = false;
                }
                #endregion
                #region Rotate Tiles
                gridElement.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.northNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.eastNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.southNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.westNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                #endregion
                #region Rotate Pieces
                if (gridElement.piece)
                {
                    gridElement.piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.piece);
                }
                if (gridElement.northNeighbor.GetComponent<NetworkedGridElement>().piece)
                {
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.northNeighbor.GetComponent<NetworkedGridElement>().piece);
                }
                if (gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().piece)
                {
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().piece);
                }
                if (gridElement.southNeighbor.GetComponent<NetworkedGridElement>().piece)
                {
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.southNeighbor.GetComponent<NetworkedGridElement>().piece);
                }
                if (gridElement.westNeighbor.GetComponent<NetworkedGridElement>().piece)
                {
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.westNeighbor.GetComponent<NetworkedGridElement>().piece);
                }
                if (gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().piece)
                {
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().piece);
                }
                if (gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().piece)
                {
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().piece);
                }
                if (gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().piece)
                {
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().piece);
                }
                if (gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().piece)
                {
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().piece);
                }
                #endregion
                #region Fix Neighbors
                gridElement.FindNeighbors();
                gridElement.northNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                gridElement.southNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                gridElement.westNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                if (gridElement.northNeighbor.GetComponent<NetworkedGridElement>().northNeighbor)
                {
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                }
                if (gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor)
                {
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                }
                if (gridElement.southNeighbor.GetComponent<NetworkedGridElement>().southNeighbor)
                {
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                }
                if (gridElement.westNeighbor.GetComponent<NetworkedGridElement>().westNeighbor)
                {
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().FindNeighbors();
                }
                #endregion
                #region FixWalls
                //this needs to correctly update neighbors too.
                UpdateWalls(gridElement);
                UpdateWalls(gridElement.northNeighbor.GetComponent<NetworkedGridElement>());
                UpdateWalls(gridElement.eastNeighbor.GetComponent<NetworkedGridElement>());
                UpdateWalls(gridElement.southNeighbor.GetComponent<NetworkedGridElement>());
                UpdateWalls(gridElement.westNeighbor.GetComponent<NetworkedGridElement>());
                UpdateWalls(gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>());
                UpdateWalls(gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>());
                UpdateWalls(gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>());
                UpdateWalls(gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>());
                #endregion
                #region FixWallSprites
                gridElement.UpdateWalls();
                gridElement.northNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                gridElement.southNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                gridElement.westNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                if (gridElement.northNeighbor.GetComponent<NetworkedGridElement>().northNeighbor)
                {
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                    gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                }
                if (gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor)
                {
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                    gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                }
                if (gridElement.southNeighbor.GetComponent<NetworkedGridElement>().southNeighbor)
                {
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                    gridElement.southNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                }
                if (gridElement.westNeighbor.GetComponent<NetworkedGridElement>().westNeighbor)
                {
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                    gridElement.westNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().UpdateWalls();
                }

                #endregion
                #region Fix Piece Rotation and Position
                if (gridElement.piece)
                    UpdateRotation(gridElement.piece.GetComponent<GamePiece>());
                if (gridElement.northNeighbor.GetComponent<NetworkedGridElement>().piece)
                    UpdateRotation(gridElement.northNeighbor.GetComponent<NetworkedGridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().piece)
                    UpdateRotation(gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.southNeighbor.GetComponent<NetworkedGridElement>().piece)
                    UpdateRotation(gridElement.southNeighbor.GetComponent<NetworkedGridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.westNeighbor.GetComponent<NetworkedGridElement>().piece)
                    UpdateRotation(gridElement.westNeighbor.GetComponent<NetworkedGridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().piece)
                    UpdateRotation(gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().piece)
                    UpdateRotation(gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().piece)
                    UpdateRotation(gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>().piece.GetComponent<GamePiece>());
                if (gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().piece)
                    UpdateRotation(gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>().piece.GetComponent<GamePiece>());
                #endregion
                #region Fix Grid Attributes and Colors
                UpdateGridAttributes();
                #endregion
                #region Fix Flag if carried
                foreach (GameObject neighbor in neighbors)
                {
                    NetworkedUnit neighborNetworkedUnit = neighbor.GetComponent<NetworkedUnit>();
                    if (neighborNetworkedUnit && neighborNetworkedUnit.flag)
                    {
                        Vector3 flagPosn = neighbor.transform.position;
                        flagPosn.z = neighborNetworkedUnit.flag.transform.position.z;
                        neighborNetworkedUnit.flag.transform.position = flagPosn;
                    }
                }
                #endregion
            }
    }

    private void UpdateWalls(NetworkedGridElement element)
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
    }

    private void UpdateGridAttributes()
    {
        NetworkedGridElement north = gridElement.northNeighbor.GetComponent<NetworkedGridElement>();
        NetworkedGridElement east = gridElement.eastNeighbor.GetComponent<NetworkedGridElement>();
        NetworkedGridElement south = gridElement.southNeighbor.GetComponent<NetworkedGridElement>();
        NetworkedGridElement west = gridElement.westNeighbor.GetComponent<NetworkedGridElement>();
        NetworkedGridElement northeast = gridElement.eastNeighbor.GetComponent<NetworkedGridElement>().northNeighbor.GetComponent<NetworkedGridElement>();
        NetworkedGridElement southeast = gridElement.southNeighbor.GetComponent<NetworkedGridElement>().eastNeighbor.GetComponent<NetworkedGridElement>();
        NetworkedGridElement northwest = gridElement.northNeighbor.GetComponent<NetworkedGridElement>().westNeighbor.GetComponent<NetworkedGridElement>();
        NetworkedGridElement southwest = gridElement.westNeighbor.GetComponent<NetworkedGridElement>().southNeighbor.GetComponent<NetworkedGridElement>();
        bool temp = north.spawnable;
        bool temp2 = west.spawnable;
        bool temp3 = north.goal;
        bool temp4 = west.goal;
        bool temp5 = northeast.spawnable;
        bool temp6 = northwest.spawnable;
        bool temp7 = northeast.goal;
        bool temp8 = northwest.goal;
        PlayerEnum temp9 = north.owner;
        PlayerEnum temp10 = west.owner;
        PlayerEnum temp11 = northeast.owner;
        PlayerEnum temp12 = northwest.owner;
        if (north.owner != PlayerEnum.none)
        {
            west.owner = north.owner;
            north.owner = PlayerEnum.none;
        }
        if (east.owner != PlayerEnum.none)
        {
            north.owner = east.owner;
            east.owner = PlayerEnum.none;
        }
        if (south.owner != PlayerEnum.none)
        {
            east.owner = south.owner;
            south.owner = PlayerEnum.none;
        }
        if (temp10 != PlayerEnum.none)
        {
            south.owner = temp10;
            if (temp9 == PlayerEnum.none) west.owner = PlayerEnum.none;
        }
        if (northeast.owner != PlayerEnum.none)
        {
            northwest.owner = northeast.owner;
            northeast.owner = PlayerEnum.none;
        }
        if (southeast.owner != PlayerEnum.none)
        {
            northeast.owner = southeast.owner;
            southeast.owner = PlayerEnum.none;
        }
        if (southwest.owner != PlayerEnum.none)
        {
            southeast.owner = southwest.owner;
            southwest.owner = PlayerEnum.none;
        }
        if (temp12 != PlayerEnum.none)
        {
            southwest.owner = temp12;
            if (temp11 == PlayerEnum.none) northwest.owner = PlayerEnum.none;
        }
        if (north.spawnable)
        {
            west.spawnable = true;
            north.spawnable = false;
        }
        if (east.spawnable)
        {
            north.spawnable = true;
            east.spawnable = false;
        }
        if (south.spawnable)
        {
            east.spawnable = true;
            south.spawnable = false;
        }
        if (temp2)
        {
            south.spawnable = true;
            if (!temp) west.spawnable = false;
        }
        if (northeast.spawnable)
        {
            northwest.spawnable = true;
            northeast.spawnable = false;
        }
        if (southeast.spawnable)
        {
            northeast.spawnable = true;
            southeast.spawnable = false;
        }
        if (southwest.spawnable)
        {
            southeast.spawnable = true;
            southwest.spawnable = false;
        }
        if (temp6)
        {
            southwest.spawnable = true;
            if (!temp5) northwest.spawnable = false;
        }
        if (north.goal)
        {
            west.goal = true;
            north.goal = false;
        }
        if (east.goal)
        {
            north.goal = true;
            east.goal = false;
        }
        if (south.goal)
        {
            east.goal = true;
            south.goal = false;
        }
        if (temp4)
        {
            south.goal = true;
            if (!temp3) west.goal = false;
        }
        if (northeast.goal)
        {
            northwest.goal = true;
            northeast.goal = false;
        }
        if (southeast.goal)
        {
            northeast.goal = true;
            southeast.goal = false;
        }
        if (southwest.goal)
        {
            southeast.goal = true;
            southwest.goal = false;
        }
        if (temp8)
        {
            southwest.goal = true;
            if (!temp7) northwest.goal = false;
        }
        grid.SetElementColor(north.gameObject, Menu.defaultColor);
        grid.SetElementColor(east.gameObject, Menu.defaultColor);
        grid.SetElementColor(south.gameObject, Menu.defaultColor);
        grid.SetElementColor(west.gameObject, Menu.defaultColor);
        grid.SetElementColor(northeast.gameObject, Menu.defaultColor);
        grid.SetElementColor(northwest.gameObject, Menu.defaultColor);
        grid.SetElementColor(southeast.gameObject, Menu.defaultColor);
        grid.SetElementColor(southwest.gameObject, Menu.defaultColor);
    }
}
