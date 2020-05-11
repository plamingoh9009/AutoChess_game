using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerClass : MonoBehaviour
{
    public static ManagerClass instance = null;

    private void Awake()
    {
        SetupSinglton();
        DontDestroyOnLoad(gameObject);
    }

    void SetupSinglton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
