using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twister : Unit
{
    public int rotationAmount = 1;

    protected override void Start()
    {
        base.Start();
        unitType = UnitType.Twister;
    }

    public override bool DisplayActionGrid()
    {
        if (canAct)
        {
            //contextMenu.HideContextMenu();
            TwistBoard(gridElement.gameObject);
            grid.activeGO = null;
            grid.SetElementColor(grid.selectedGO, Menu.selectedColor, Menu.defaultColor);
            canAct = false;
        }
        return true;
    }

    private void TwistBoard(GameObject twistLoc)
    {
        if (gridElement.northNeighbor && gridElement.eastNeighbor && gridElement.southNeighbor && gridElement.westNeighbor)
            for (int i = 0; i < rotationAmount; ++i)
            {
                List<GameObject> neighbors = new List<GameObject>();
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
                gridElement.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.northNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.eastNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.southNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.westNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                #endregion
                #region Rotate Pieces
                if (gridElement.piece)
                {
                    gridElement.piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.piece);
                }
                if (gridElement.northNeighbor.GetComponent<GridElement>().piece)
                {
                    gridElement.northNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.northNeighbor.GetComponent<GridElement>().piece);
                }
                if (gridElement.eastNeighbor.GetComponent<GridElement>().piece)
                {
                    gridElement.eastNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.eastNeighbor.GetComponent<GridElement>().piece);
                }
                if (gridElement.southNeighbor.GetComponent<GridElement>().piece)
                {
                    gridElement.southNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.southNeighbor.GetComponent<GridElement>().piece);
                }
                if (gridElement.westNeighbor.GetComponent<GridElement>().piece)
                {
                    gridElement.westNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.westNeighbor.GetComponent<GridElement>().piece);
                }
                if (gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().piece)
                {
                    gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>().piece);
                }
                if (gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().piece)
                {
                    gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>().piece);
                }
                if (gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().piece)
                {
                    gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>().piece);
                }
                if (gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().piece)
                {
                    gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().piece.transform.RotateAround(gridElement.transform.position, Vector3.back, 90);
                    neighbors.Add(gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>().piece);
                }
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
                #region Fix Grid Attributes and Colors
                UpdateGridAttributes();
                #endregion
                #region Fix Flag if carried
                foreach (GameObject neighbor in neighbors)
                {
                    Unit neighborUnit = neighbor.GetComponent<Unit>();
                    if (neighborUnit && neighborUnit.flag)
                    {
                        Vector3 flagPosn = neighbor.transform.position;
                        flagPosn.z = neighborUnit.flag.transform.position.z;
                        neighborUnit.flag.transform.position = flagPosn;
                    }
                }
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
    }

    private void UpdateGridAttributes()
    {
        GridElement north = gridElement.northNeighbor.GetComponent<GridElement>();
        GridElement east = gridElement.eastNeighbor.GetComponent<GridElement>();
        GridElement south = gridElement.southNeighbor.GetComponent<GridElement>();
        GridElement west = gridElement.westNeighbor.GetComponent<GridElement>();
        GridElement northeast = gridElement.eastNeighbor.GetComponent<GridElement>().northNeighbor.GetComponent<GridElement>();
        GridElement southeast = gridElement.southNeighbor.GetComponent<GridElement>().eastNeighbor.GetComponent<GridElement>();
        GridElement northwest = gridElement.northNeighbor.GetComponent<GridElement>().westNeighbor.GetComponent<GridElement>();
        GridElement southwest = gridElement.westNeighbor.GetComponent<GridElement>().southNeighbor.GetComponent<GridElement>();
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
