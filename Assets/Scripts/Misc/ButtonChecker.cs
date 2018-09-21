using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ButtonChecker : MonoBehaviour
{
    void Update()
    {
        //X
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            Console.WriteLine("pressed button 0");
        }
        //O
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            Console.WriteLine("pressed button 1");
        }
        //Square
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            Console.WriteLine("pressed button 2");
        }
        //Triangle
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            Console.WriteLine("pressed button 3");
        }
        //L1
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            Console.WriteLine("pressed button 4");
        }
        //R1
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            Console.WriteLine("pressed button 5");
        }
        //Pad
        if (Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            Console.WriteLine("pressed button 6");
        }
        //Options
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            Console.WriteLine("pressed button 7");
        }
        //L3
        if (Input.GetKeyDown(KeyCode.JoystickButton8))
        {
            Console.WriteLine("pressed button 8");
        }
        //R3
        if (Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            Console.WriteLine("pressed button 9");
        }
        //D-UP
        if (Input.GetKeyDown(KeyCode.JoystickButton10))
        {
            Console.WriteLine("pressed button 10");
        }
        //D-RIGHT
        if (Input.GetKeyDown(KeyCode.JoystickButton11))
        {
            Console.WriteLine("pressed button 11");
        }
        //D-DOWN
        if (Input.GetKeyDown(KeyCode.JoystickButton12))
        {
            Console.WriteLine("pressed button 12");
        }
        //D-LEFT
        if (Input.GetKeyDown(KeyCode.JoystickButton13))
        {
            Console.WriteLine("pressed button 13");
        }
    }
}
