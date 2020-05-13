using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyFunc
{
    public enum PathType
    {
        PREFABS,
        CHAMP_INFO
    }
    public static string GetPath(PathType type)
    {
        string path = default(string);
        switch(type)
        {
            case PathType.PREFABS:
                path = "Assets/04_Prefabs/";
                break;
            case PathType.CHAMP_INFO:
                path = "Assets/05_Infos/Champions/";
                break;
            default:
                break;
        }
        return path;
    }
    public static void Swap<T>(ref T element1, ref T element2)
    {
        T temp = element1;
        element1 = element2;
        element2 = temp;
    }
    public static void SetActiveAll(List<GameObject> list, bool isActive = true)
    {
        foreach(var ele in list)
        {
            ele.SetActive(isActive);
        }
    }
}
