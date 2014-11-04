using System.Collections;
using System;

public class Tools
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
}