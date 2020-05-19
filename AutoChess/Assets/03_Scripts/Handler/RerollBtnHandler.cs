using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollBtnHandler : MonoBehaviour
{
    RollChampions _containerHandler;
    LockHandler _lockHandler;
    GoldUi goldUi;
    int rerollCost;

    private void Awake()
    {
        _containerHandler = GameObject.Find("Shop/ShopObj/ChampContainer").GetComponent<RollChampions>();
        _lockHandler = GameObject.Find("Shop/ShopObj/ShopUi/LockButton").GetComponent<LockHandler>();
        goldUi = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<GoldUi>();
        rerollCost = 2;
    }

    public void OnClickReroll()
    {
        if(goldUi.gold >= rerollCost)
        {
            RunReroll();
            goldUi.AddGold(rerollCost * -1);
        }
    }
    void RunReroll()
    {
        if (_lockHandler.isLocked)
        {
            _lockHandler.OnClick();
        }
        _containerHandler.Reroll();
    }
}
