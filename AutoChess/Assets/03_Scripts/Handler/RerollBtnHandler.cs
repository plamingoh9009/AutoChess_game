using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollBtnHandler : MonoBehaviour
{
    RollChampions _containerHandler;
    LockHandler _lockHandler;
    int rerollCost;

    GoldUi goldUi;
    MessageBox msg;

    private void Awake()
    {
        _containerHandler = GameObject.Find("Shop/ShopObj/ChampContainer").GetComponent<RollChampions>();
        _lockHandler = GameObject.Find("Shop/ShopObj/ShopUi/LockButton").GetComponent<LockHandler>();
        rerollCost = 2;

        goldUi = MyFunc.GetObject(MyFunc.ObjType.PLAYER_UI).GetComponent<GoldUi>();
        msg = MyFunc.GetObject(MyFunc.ObjType.MESSAGE_BOX).GetComponent<MessageBox>();
    }

    public void OnClickReroll()
    {
        if(goldUi.gold >= rerollCost)
        {
            RunReroll();
            goldUi.AddGold(rerollCost * -1);
        }
        else
        {
            msg.OnMessageBox(MessageBox.MessageType.NOT_ENOUGH_GOLD);
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
