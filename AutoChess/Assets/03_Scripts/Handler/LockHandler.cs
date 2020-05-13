using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHandler : MonoBehaviour
{
    public bool _isLocked { get; private set; }
    string _lockObjName;
    string _unlockObjName;
    RollChampions _containerHandler;
    #region Start()
    private void Awake()
    {
        _isLocked = false;
        _lockObjName = "Lock";
        _unlockObjName = "UnLock";
    }
    private void Start()
    {
        _containerHandler = GameObject.Find("Shop/ShopObj/ChampContainer").GetComponent<RollChampions>();
    }
    #endregion

    #region Click lock button
    // 내가 클릭하면 UI를 바꾼다.
    public void OnClick()
    {
        ChangeLockState();
    }
    void ChangeLockState()
    {
        if(_isLocked)
        {
            transform.Find(_lockObjName).gameObject.SetActive(false);
            transform.Find(_unlockObjName).gameObject.SetActive(true);
            _isLocked = !_isLocked;
            _containerHandler.isLocked = false;
        }// if: 잠겨 있다면 풀어준다.
        else
        {
            transform.Find(_lockObjName).gameObject.SetActive(true);
            transform.Find(_unlockObjName).gameObject.SetActive(false);
            _isLocked = !_isLocked;
            _containerHandler.isLocked = true;
        }
    }
    #endregion
}
