using System.Collections;
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

    private void Awake()
    {
        Messenger<Vector3>.AddListener(Messages.MOVETILECLICKED, UpdateGhost);
        Messenger<RaycastHit>.AddListener(Messages.CURSORCLICK, CheckRaycast);
    }

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
        //Cancel
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Escape))
        {
            DestroyMoveGrid();
            if(activeGhost)
            {
                Destroy(activeGhost);
            }
            if (transform.parent.position.x == 0)
            {
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                transform.parent.localPosition = new Vector3(0, 0, 0);
            }
        }
        //Confirm
        if (Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.Return))
        {
            if (activeGhost)
            {
                transform.root.position = activeGhost.transform.position;
                Destroy(activeGhost);
            }
            DestroyMoveGrid();
            transform.parent.localPosition = new Vector3(0, 0, 0);
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        OnCursorClick();
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
        transform.parent.localPosition = new Vector3(10000, 0, 0);
        GenerateMoveGrid();
    }

    private void GenerateMoveGrid()
    {
        for(int i = -main.moveAmount + 1; i <= main.moveAmount - 1; ++i)
        {
            for(int j = -main.moveAmount + 1; j <= main.moveAmount - 1; ++j)
            {
                gridPositions.Add(Instantiate(moveGridPrefab, new Vector3(transform.root.position.x + (distX * i), transform.root.position.y + (distY * j)), Quaternion.Euler(-90, 0, 0)).GetComponent<MoveGrid>());
            }
        }
        gridPositions.Add(Instantiate(moveGridPrefab, new Vector3(transform.root.position.x + (distX * main.moveAmount), transform.root.position.y), Quaternion.Euler(-90, 0, 0)).GetComponent<MoveGrid>());
        gridPositions.Add(Instantiate(moveGridPrefab, new Vector3(transform.root.position.x + (distX * -main.moveAmount), transform.root.position.y), Quaternion.Euler(-90, 0, 0)).GetComponent<MoveGrid>());
        gridPositions.Add(Instantiate(moveGridPrefab, new Vector3(transform.root.position.x, transform.root.position.y + (distY * main.moveAmount)), Quaternion.Euler(-90, 0, 0)).GetComponent<MoveGrid>());
        gridPositions.Add(Instantiate(moveGridPrefab, new Vector3(transform.root.position.x, transform.root.position.y + (distY * -main.moveAmount)), Quaternion.Euler(-90, 0, 0)).GetComponent<MoveGrid>());
    }

    private void DestroyMoveGrid()
    {
        while(gridPositions.Count > 0)
        {
            Destroy(gridPositions[0].gameObject);
            gridPositions.RemoveAt(0);
        }
    }

    private void UpdateGhost(Vector3 position)
    {
        if (activeGhost)
        {
            Destroy(activeGhost);
        }
        activeGhost = Instantiate(ghostManPrefab, position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        Messenger<RaycastHit>.RemoveListener(Messages.CURSORCLICK, CheckRaycast);
        Messenger<Vector3>.RemoveListener(Messages.MOVETILECLICKED, UpdateGhost);
    }
}
