using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RerollBtnHandler : MonoBehaviour
{
    RollChampions _containerHandler;
    LockHandler _lockHandler;

    private void Start()
    {
        _containerHandler = GameObject.Find("Shop/ShopObj/ChampContainer").GetComponent<RollChampions>();
        _lockHandler = GameObject.Find("Shop/ShopObj/ShopUi/LockButton").GetComponent<LockHandler>();
    }

    public void OnClickReroll()
    {
        if (_lockHandler.isLocked)
        {
            _lockHandler.OnClick();
        }
        _containerHandler.Reroll();
    }
}
