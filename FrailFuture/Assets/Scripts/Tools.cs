using UnityEngine;
using System.Collections;
using System;

public class Tools : MonoBehaviour
{
    public static void AssertTrue(bool condition, string message = "")
    {
        if (!condition)
        {
            BreakToDebug(message);
            throw new Exception(message);
        }
    }

    public static void AssertFalse(bool condition, string message = "")
    {
        if (condition)
        {
            BreakToDebug(message);
            throw new Exception(message);
        }
    }

    public static void BreakToDebug(string message)
    {
         if (Application.isEditor) {
            // Only break in editor to allow examination of the current scene state.
            Debug.Break();
        }
        else {
            // There's no standard way to return an error code to the OS,
            // so just quit regularly.
            Application.Quit();
        } 
    }
}
