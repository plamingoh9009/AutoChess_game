using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUi : MonoBehaviour
{
    public int gold;
    Text currentGold;

    GoldStatue goldStatue;
    private void Awake()
    {
        gold = 2;
        currentGold = MyFunc.GetObject(MyFunc.ObjType.GOLD).
            transform.Find("Current").GetComponent<Text>();
        goldStatue = MyFunc.GetObject(MyFunc.ObjType.FIXED_OBJECT).
            transform.Find("GoldDragons").GetComponent<GoldStatue>();
    }

    public void AddGold(int add)
    {
        gold += add;
        currentGold.text = gold.ToString();

        goldStatue.SyncStatueWithGold();
    }
}
