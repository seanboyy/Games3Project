using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    public float xSensitivity = 1F;
    public float ySensitivity = 1F;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            RaycastHit info = DoRaycast();
            Messenger<RaycastHit>.Broadcast(Messages.CURSORCLICK, info);
        }
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        x *= Time.deltaTime;
        y *= Time.deltaTime;
        transform.position += new Vector3(xSensitivity * x, ySensitivity * y, 0);
    }

    RaycastHit DoRaycast()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0, -20), Vector3.forward);
        RaycastHit info;
        Physics.Raycast(ray, out info);
        if(info.collider != null)
        {
            Console.WriteLine(info.collider.gameObject.name);
        }
        return info;
    }
}
