﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    public GameObject moveGridPrefab;

    public GameObject ghostManPrefab;

    private MainManager main;

    private List<MoveGrid> gridPositions = new List<MoveGrid>();

    private GameObject activeGhost;

    public int spacesMoved = 0;

    float x, z, distX, distY;

    private void Start()
    {
        main = FindObjectOfType<MainManager>();
        x = moveGridPrefab.transform.localScale.x;
        z = moveGridPrefab.transform.localScale.z;
        distX = x * 10;
        distY = z * 10;
        distX += 2 * main.tileOffset;
        distY += 2 * main.tileOffset;
    }

    private void Update()
    {
        foreach (MoveGrid move in gridPositions)
        {
            if (move.isClicked)
            {
                /*
                if(activeGhost != null)
                {
                    Destroy(activeGhost);
                }
                activeGhost = Instantiate(ghostManPrefab, move.transform.position, Quaternion.identity);
                move.isClicked = false;
                */
                Debug.Log("Clicked a tile");
                move.isClicked = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyMoveGrid();
            if(activeGhost != null)
            {
                Destroy(activeGhost);
            }
            transform.parent.position += new Vector3(-10000, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            transform.root.position = activeGhost.transform.position;
            Destroy(activeGhost);
            DestroyMoveGrid();
            transform.parent.position += new Vector3(-10000, 0, 0);
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        transform.parent.position += new Vector3(10000, 0, 0);
        GenerateMoveGrid();
    }

    private void GenerateMoveGrid()
    {
        //MoveGrid grid = moveGridPrefab.GetComponent<MoveGrid>();
        for(int i = -main.moveAmount + 1; i <= main.moveAmount - 1; ++i)
        {
            for(int j = -main.moveAmount + 1; j <= main.moveAmount - 1; ++j)
            {
                gridPositions.Add(Instantiate(moveGridPrefab, new Vector3(transform.root.position.x + distX * i, transform.root.position.y + distY * j), Quaternion.Euler(-90, 0, 0)).GetComponent<MoveGrid>());
            }
        }
        gridPositions.Add(Instantiate(moveGridPrefab, new Vector3(transform.root.position.x + distX * main.moveAmount, 0), Quaternion.Euler(-90, 0, 0)).GetComponent<MoveGrid>());
        gridPositions.Add(Instantiate(moveGridPrefab, new Vector3(transform.root.position.x + distX * -main.moveAmount, 0), Quaternion.Euler(-90, 0, 0)).GetComponent<MoveGrid>());
        gridPositions.Add(Instantiate(moveGridPrefab, new Vector3(0, transform.root.position.y + distY * main.moveAmount), Quaternion.Euler(-90, 0, 0)).GetComponent<MoveGrid>());
        gridPositions.Add(Instantiate(moveGridPrefab, new Vector3(0, transform.root.position.y + distY * -main.moveAmount), Quaternion.Euler(-90, 0, 0)).GetComponent<MoveGrid>());
    }

    private void DestroyMoveGrid()
    {
        foreach (MoveGrid move in gridPositions)
        {
            Destroy(move.gameObject);
        }
    }
}
