using UnityEngine;
using System.Collections;
using System;

public class Tools : MonoBehaviour
{
    public static int SummationChangeBy(int from, int timesToChange, int Increase_Decrease_By = 1)
    {
        int summation = 0;
        int timesChanged = 0;
        while (timesChanged < timesToChange)
        {
            summation += from;
            from += Increase_Decrease_By;
            timesChanged++;
        }
        return summation;
    }

    public static int SummationDownFrom(int n)
    {
        if (n <= 1)
        {
            return 1;
        }
        return n + SummationDownFrom(n - 1);
    }

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
