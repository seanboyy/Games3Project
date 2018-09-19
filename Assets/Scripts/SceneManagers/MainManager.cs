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

    // Use this for initialization
    void Start()
    {
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
            yield return new WaitForSecondsRealtime(3.2143F);
        }
        yield return null;
    }
}
