using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum TurnType
    {
        READY,
        FIGHT
    }

    public static GameManager instance = null;
    public TurnType myTurn { get; set; }

    private void Awake()
    {
        SetupSinglton();
        myTurn = TurnType.READY;
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
