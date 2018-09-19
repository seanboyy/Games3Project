using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To type the next 4 lines, start by typing /// and then Tab.
/// <summary>
/// Keeps a GameObject on screen. 
/// Note that this ONLY works for an orthographic Main Camera at [ 0, 0, 0 ].
/// </summary>
public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = false;
    public bool wrap = false;
    public bool deleteIfOffScren = false;

    [Header("Custom Bounds")]
    public bool customUpper = false;
    public bool customLower = false;
    public bool customRight = false;
    public bool customLeft = false;

    public float upperBound;
    public float lowerBound;
    public float leftBound;
    public float rightBound;

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;

    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;

        if (!customUpper)
            upperBound = camHeight;
        if (!customLower)
            lowerBound = -camHeight;
        if (!customRight)
            rightBound = camWidth;
        if (!customLeft)
            leftBound = -camWidth;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if (pos.x > rightBound - radius)
        {
            if (wrap)
            {
                transform.position = new Vector3(leftBound + radius, pos.y, pos.z);
            }
            else
            {
                pos.x = rightBound - radius;
                isOnScreen = false;
                offRight = true;
            }
        }

        if (pos.x < leftBound + radius)
        {
            if (wrap)
            {
                transform.position = new Vector3(rightBound - radius, pos.y, pos.z);
            }
            else
            {
                pos.x = leftBound + radius;
                isOnScreen = false;
                offLeft = true;
            }
        }

        if (pos.y > upperBound - radius)
        {
            if (wrap)
            {
                transform.position = new Vector3(pos.x, lowerBound + radius, pos.z);
            }
            else
            {
                pos.y = upperBound - radius;
                isOnScreen = false;
                offUp = true;
            }
        }

        if (pos.y < lowerBound + radius)
        {
            if (wrap)
            {
                transform.position = new Vector3(pos.x, upperBound, pos.z);
            }
            else
            {
                pos.y = lowerBound + radius;
                isOnScreen = false;
                offDown = true;
            }
        }

        isOnScreen = !(offRight || offLeft || offUp || offDown);
        if (!isOnScreen && deleteIfOffScren)
        {
            Destroy(gameObject);
        }
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }
    }

    // Draw the bounds in the Scene pane using OnDrawGizmos()
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
