using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHandler : MonoBehaviour
{
    #region Initialize
    public bool _isLocked { get; private set; }
    string _lockObjName;
    string _unlockObjName;
    private void Awake()
    {
        _isLocked = false;
        _lockObjName = "Lock";
        _unlockObjName = "UnLock";
    }
    #endregion

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
        }// if: 잠겨 있다면 풀어준다.
        else
        {
            transform.Find(_lockObjName).gameObject.SetActive(true);
            transform.Find(_unlockObjName).gameObject.SetActive(false);
            _isLocked = !_isLocked;
        }
    }
}
