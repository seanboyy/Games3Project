using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultUnit : MonoBehaviour
{

    public GameObject unitMenu;

    GameObject activeMenu;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseEnter()
    {
        //activeMenu = Instantiate(unitMenu);
        Debug.Log("Creating Menu");
    }

    private void OnMouseDown()
    {
        activeMenu = Instantiate(unitMenu);
    }
}
