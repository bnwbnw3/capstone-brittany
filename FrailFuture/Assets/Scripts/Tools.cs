using UnityEngine;
using System.Collections;
using System;

public class Tools : MonoBehaviour
{
    public static int SummationUpBy(int from, int timesToIncrease, int IncreaseBy = 1)
    {
        int summation = 0;
        int decreaseAmount = 0;
        while (decreaseAmount < timesToIncrease)
        {
            summation += from;
            from += IncreaseBy;
        }
        return summation;
    }

    public static int SummationDownBy(int from, int timesToDecrease, int decreaseBy = 1)
    {
        int summation = 0;
        int decreaseAmount = 0;
        while (decreaseAmount < timesToDecrease)
        {
            summation += from;
            from -= decreaseBy;
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
