using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    GoldUi gold;
    private void Awake()
    {
        gold = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<GoldUi>();
    }

    public void OnClick()
    {
        gold.AddGold(10);
    }
}
