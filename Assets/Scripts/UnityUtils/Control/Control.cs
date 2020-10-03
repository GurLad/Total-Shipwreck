using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SavedData;

public static class Control
{
    public enum CB { Pickup, Use }
    public enum CM { Keyboard, Controller }
    public enum Axis { X, Y }
    public static float DeadZone = 0.3f;

    public static bool GetButton(CB button)
    {
        return Input.GetButton(button.ToString());
    }

    public static bool GetButtonUp(CB button)
    {
        return Input.GetButtonUp(button.ToString());
    }

    public static bool GetButtonDown(CB button)
    {
        return Input.GetButtonDown(button.ToString());
    }

    public static int GetAxis(Axis axis)
    {
        float input = Input.GetAxis(axis == Axis.X ? "Horizontal" : "Vertical");
        if (Mathf.Abs(input) > DeadZone)
        {
            return (int)Mathf.Sign(input);
        }
        else
        {
            return 0;
        }
    }
}
