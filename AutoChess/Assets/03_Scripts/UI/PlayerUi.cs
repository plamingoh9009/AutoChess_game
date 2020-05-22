using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUi : MonoBehaviour
{
    GoldUi goldUi;
    UnitCount unitCount;
    GoldStatue goldStatue;
    GameObject goldTextObj;
    private void Awake()
    {
        goldUi = GetComponent<GoldUi>();
        unitCount = GetComponent<UnitCount>();
        goldStatue = MyFunc.GetObject(MyFunc.ObjType.FIXED_OBJECT).
            transform.Find("GoldDragons").GetComponent<GoldStatue>();
        goldTextObj = transform.Find("Gold").gameObject;
    }
    private void Start()
    {
        goldUi.SetupObjs(goldStatue.gameObject, goldTextObj);
        goldStatue.SetupGoldStatues(goldUi);
        unitCount.SetupList();
    }
}
