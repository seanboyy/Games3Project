using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Sprite[] timeBarImgs;

    public Image timeBar;

    public float tileOffset = 0.01F;

    public int moveAmount = 2;

    public float moveDist = 0.5F;

    public int gridWidth = 10;

    public int gridHeight = 10;

    float x, z, distX, distY;

    public GameObject gridTile;

    // Use this for initialization
    void Start()
    {
        x = gridTile.transform.localScale.x;
        z = gridTile.transform.localScale.z;
        distX = x * 10;
        distY = z * 10;
        distX += 2 * tileOffset;
        distY += 2 * tileOffset;
        for (int i = -((gridHeight + (gridHeight % 2) / 2)); i < ((gridHeight + (gridHeight % 2) / 2)); ++i)
        {
            for (int j = -((gridWidth + (gridWidth % 2) / 2)); j < ((gridWidth + (gridWidth % 2) / 2)); ++j)
            {

            }
        }
        timeBar.sprite = timeBarImgs[28];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine("DoTimeBar");
        }
    }

    IEnumerator DoTimeBar()
    {
        for (int i = 28; i >= 0; --i)
        {
            timeBar.sprite = timeBarImgs[i];
            //Value determined by taking number of different time bar sprites (28) and using math to make them all appear in sequence over a 90 second time interval
            yield return new WaitForSecondsRealtime(3.2143F);
        }
        yield return null;
    }
}
