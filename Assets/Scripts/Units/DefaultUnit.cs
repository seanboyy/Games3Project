﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultUnit : MonoBehaviour
{

    public GameObject unitMenu;

    private void OnMouseDown()
    {
        unitMenu.SetActive(true);
    }
}
