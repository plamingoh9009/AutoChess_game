using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInven : MonoBehaviour
{
    TileHandler tileHandler;
    GoldUi goldUi;
    UnitCount unitCount;
    Inventory inven;

    private void Awake()
    {
        tileHandler = MyFunc.GetObject(MyFunc.ObjType.FIXED_OBJECT).
            transform.Find("Tiles").GetComponent<TileHandler>();
        goldUi = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<GoldUi>();
        unitCount = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<UnitCount>();
        inven = MyFunc.GetObject(MyFunc.ObjType.INVENTORY).GetComponent<Inventory>();
    }
    private void Start()
    {
        tileHandler.SetupTiles();
        inven.SetupComponents(goldUi, unitCount, tileHandler);
    }
}
