using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyFunc
{
    public static void Swap<T>(ref T element1, ref T element2)
    {
        T temp = element1;
        element1 = element2;
        element2 = temp;
    }
}
