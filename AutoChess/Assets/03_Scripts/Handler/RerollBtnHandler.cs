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
        //_lockHandler = GameObject.Find("Shop/ShopObj/ChampContainer")
    }

    public void OnClickReroll()
    {

        _containerHandler.isLocked = false;
        _containerHandler.Reroll();
    }
}
